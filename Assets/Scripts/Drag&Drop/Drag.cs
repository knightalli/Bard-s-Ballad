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
    Image originalImage;

    private void Awake()
    {
        originalImage = GetComponent<Image>();
    }

    public void OnBeginDrag(PointerEventData e)
    {
        originalImage.enabled = false;
        dragIcon = new GameObject("DragIcon");
        Image img = dragIcon.AddComponent<Image>();
        img.sprite = icon;
        dragIcon.transform.SetParent(inventory.dragRoot, false);
        dragIcon.transform.SetAsLastSibling();
        CanvasGroup cg = dragIcon.AddComponent<CanvasGroup>();
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
        originalImage.enabled = true;
    }
}
