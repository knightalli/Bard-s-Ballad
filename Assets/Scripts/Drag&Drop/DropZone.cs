// DropZone.cs
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DropZone : MonoBehaviour, IDropHandler
{
    [SerializeField] private int slotIndex;
    [SerializeField] private bool isActive;
    [SerializeField] private Inventory inventory;
    [SerializeField] private Image iconImage;
    [SerializeField] private Sprite emptySprite;

    public void Setup(ItemSO item, bool active, Inventory inv, int index)
    {
        inventory = inv;
        isActive = active;
        slotIndex = index;
        if (item != null) SetIcon(item);
        else SetEmpty();
    }

    public void OnDrop(PointerEventData e)
    {
        var drag = e.pointerDrag?.GetComponent<Drag>();
        if (drag != null)
            inventory.HandleDrop(drag.item, slotIndex, isActive);
        drag.OnEndDrag(e);
    }

    public void SetIcon(ItemSO item)
    {
        iconImage.sprite = item.icon;
        var dr = iconImage.gameObject.GetComponent<Drag>()
                 ?? iconImage.gameObject.AddComponent<Drag>();
        dr.item = item;
        dr.inventory = inventory;
        dr.icon = item.icon;
    }

    public void SetEmpty()
    {
        var dr = iconImage.gameObject.GetComponent<Drag>();
        if (dr != null) Destroy(dr);
        iconImage.sprite = emptySprite;
    }
}
