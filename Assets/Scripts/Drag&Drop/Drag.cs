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
    CanvasGroup cg;
    Image originalImage;

    void Awake()
    {
        originalImage = GetComponent<Image>();
        cg = GetComponent<CanvasGroup>();
        if (cg == null)
            cg = gameObject.AddComponent<CanvasGroup>();
    }

    public void OnBeginDrag(PointerEventData e)
    {
        originalImage.color = new Color(1, 1, 1, 0);
        cg.blocksRaycasts = false;

        dragIcon = new GameObject("DragIcon");
        var img = dragIcon.AddComponent<Image>();
        img.sprite = icon;
        img.raycastTarget = false;
        dragIcon.transform.SetParent(inventory.dragRoot, false);
        dragIcon.transform.SetAsLastSibling();
    }

    public void OnDrag(PointerEventData e)
    {
        if (dragIcon != null)
            dragIcon.transform.position = e.position;
    }

    public void OnEndDrag(PointerEventData e)
    {
        if (dragIcon != null)
        {
            Destroy(dragIcon);
        }

        originalImage.color = Color.white;
        cg.blocksRaycasts = true;
    }
}
