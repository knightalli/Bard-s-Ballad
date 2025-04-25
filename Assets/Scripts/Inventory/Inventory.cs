// Inventory.cs
using System;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public Transform dragRoot;
    public DropZone generalSlotPrefab;
    public Transform generalContent;
    public List<ItemSO> generalItems = new List<ItemSO>();
    public DropZone activeSlotPrefab;
    public Transform activeContent;
    public int maxActive = 3;
    public PlayerStats playerStats;

    private List<DropZone> activeSlots = new List<DropZone>();
    private ItemSO[] activeItems;

    private void Awake()
    {
        activeItems = new ItemSO[maxActive];
    }

    private void Start()
    {
        if (playerStats == null)
            Debug.LogError("Inventory: playerStats not assigned", this);

        for (int i = 0; i < maxActive; i++)
        {
            DropZone slot = Instantiate(activeSlotPrefab, activeContent);
            slot.Setup(null, true, this, i);
            activeSlots.Add(slot);
        }

        RefreshActive();
        RefreshGeneral();
    }

    private void RefreshActive()
    {
        for (int i = 0; i < activeSlots.Count; i++)
        {
            ItemSO item = activeItems[i];
            if (item != null)
                activeSlots[i].SetIcon(item);
            else
                activeSlots[i].SetEmpty();
        }
    }

    public void RefreshGeneral()
    {
        foreach (Transform t in generalContent)
            Destroy(t.gameObject);

        foreach (ItemSO item in generalItems)
        {
            DropZone slot = Instantiate(generalSlotPrefab, generalContent);
            slot.Setup(item, false, this, -1);
        }
    }

    public void HandleDrop(ItemSO item, int targetSlotIndex, bool toActive)
    {
        if (toActive)
        {
            if (targetSlotIndex < 0 || targetSlotIndex >= maxActive)
                return;
            if (!generalItems.Contains(item))
                return;

            ItemSO old = activeItems[targetSlotIndex];
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
            if (idx < 0)
                return;

            playerStats.RemoveBonus(item.bonusValue, (int)item.statType);
            activeItems[idx] = null;
            generalItems.Add(item);
        }

        RefreshActive();
        RefreshGeneral();
    }
}
