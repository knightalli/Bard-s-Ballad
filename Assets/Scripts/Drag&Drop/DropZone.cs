// DropZone.cs
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DropZone : MonoBehaviour, IDropHandler
{
    int slotIndex;
    bool isActiveSlot;
    Inventory inventory;
    Image iconImage;
    public Sprite emptySprite;

    private void Awake()
    {
        iconImage = transform.Find("Icon").GetComponent<Image>();
    }

    public void Setup(ItemSO item, bool isActive, Inventory inv, int index)
    {
        inventory = inv;
        isActiveSlot = isActive;
        slotIndex = index;
        if (item != null) SetIcon(item);
        else SetEmpty();
    }

    public void OnDrop(PointerEventData e)
    {
        Drag drag = e.pointerDrag?.GetComponent<Drag>();
        if (drag != null)
            inventory.HandleDrop(drag.item, slotIndex, isActiveSlot);
    }

    public void SetIcon(ItemSO item)
    {
        iconImage.sprite = item.icon;
        iconImage.enabled = true;
        Drag dr = iconImage.gameObject.GetComponent<Drag>()
                  ?? iconImage.gameObject.AddComponent<Drag>();
        dr.item = item;
        dr.inventory = inventory;
        dr.icon = item.icon;
    }

    public void SetEmpty()
    {
        Drag dr = iconImage.gameObject.GetComponent<Drag>();
        if (dr != null)
            Destroy(dr);
        iconImage.enabled = false;
    }
}
