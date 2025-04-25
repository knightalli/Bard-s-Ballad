// Drag.cs
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Drag : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public ItemSO item;
    public Inventory inventory;
    public Sprite icon;

    GameObject dragIcon;

    public void OnBeginDrag(PointerEventData e)
    {
        dragIcon = new GameObject("DragIcon");
        var img = dragIcon.AddComponent<Image>();
        img.sprite = icon;
        dragIcon.transform.SetParent(inventory.dragRoot, false);
        dragIcon.transform.SetAsLastSibling();
        var cg = dragIcon.AddComponent<CanvasGroup>();
        cg.blocksRaycasts = false;
    }

    public void OnDrag(PointerEventData e)
    {
        if (dragIcon != null)
            dragIcon.transform.position = e.position;
    }

    public void OnEndDrag(PointerEventData e)
    {
        if (dragIcon != null)
            Destroy(dragIcon);
    }
}
