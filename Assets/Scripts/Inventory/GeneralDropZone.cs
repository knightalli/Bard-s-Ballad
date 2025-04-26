// GeneralDropZone.cs
using UnityEngine;
using UnityEngine.EventSystems;

public class GeneralDropZone : MonoBehaviour, IDropHandler
{
    public Inventory inventory;

    public void OnDrop(PointerEventData e)
    {
        var drag = e.pointerDrag?.GetComponent<Drag>();
        if (drag != null)
            inventory.HandleDrop(drag.item, -1, false);
        drag.OnEndDrag(e);
    }
}
