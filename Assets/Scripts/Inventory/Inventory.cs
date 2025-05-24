// Inventory.cs
using System;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public Transform dragRoot;
    [SerializeField] private DropZone generalSlotPrefab;
    [SerializeField] private Transform generalContent;
    [SerializeField] private List<ItemSO> generalItems = new List<ItemSO>();
    [SerializeField] private DropZone activeSlotPrefab;
    [SerializeField] private Transform activeContent;
    [SerializeField] private int maxActive = 3;
    [SerializeField] private PlayerStats playerStats;

    private List<DropZone> activeSlots = new List<DropZone>();
    private ItemSO[] activeItems;

    void Awake()
    {
        activeItems = new ItemSO[maxActive];
    }

    void Start()
    {
        if (playerStats == null)
            Debug.LogError("playerStats not assigned", this);

        for (int i = 0; i < maxActive; i++)
        {
            var slot = Instantiate(activeSlotPrefab, activeContent, false);
            slot.Setup(null, true, this, i);
            activeSlots.Add(slot);
        }

        RefreshActive();
        RefreshGeneral();
    }

    public void AddItem(ItemSO item)
    {
        generalItems.Add(item);
        RefreshGeneral();
    }

    void RefreshActive()
    {
        for (int i = 0; i < activeSlots.Count; i++)
        {
            var item = activeItems[i];
            if (item != null) activeSlots[i].SetIcon(item);
            else activeSlots[i].SetEmpty();
        }
    }

    public void RefreshGeneral()
    {
        foreach (Transform t in generalContent) Destroy(t.gameObject);

        foreach (var item in generalItems)
        {
            var slot = Instantiate(generalSlotPrefab, generalContent, false);
            slot.Setup(item, false, this, -1);
        }
    }

    public void HandleDrop(ItemSO item, int targetSlotIndex, bool toActive)
    {
        if (toActive)
        {
            if (targetSlotIndex < 0 || targetSlotIndex >= maxActive) return;
            if (!generalItems.Contains(item)) return;

            var old = activeItems[targetSlotIndex];
            if (old != null)
            {
                playerStats.RemoveBonus(old.bonusValue, (int)old.statType);
                generalItems.Add(old);
            }

            generalItems.Remove(item);
            activeItems[targetSlotIndex] = item;
            playerStats.AddBonus(item.bonusValue, (int)item.statType);
        }
        else
        {
            int idx = Array.IndexOf(activeItems, item);
            if (idx < 0) return;

            playerStats.RemoveBonus(item.bonusValue, (int)item.statType);
            activeItems[idx] = null;
            generalItems.Add(item);
        }

        RefreshActive();
        RefreshGeneral();
    }
}
