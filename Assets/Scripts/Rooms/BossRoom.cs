using UnityEngine;

public class BossRoom : Room
{
    [Header("Boss Room Settings")]
    public bool isBossRoom = true;
    private Vector2Int entranceDirection; // Направление, откуда игрок вошел

    public void SetEntranceDirection(Vector2Int direction)
    {
        entranceDirection = direction;
        
        // Отключаем коллайдер только у двери, через которую игрок вошел
        if (direction == Vector2Int.up)
        {
            if (DoorD != null)
            {
                var collider = DoorD.GetComponent<Collider2D>();
                if (collider != null)
                {
                    collider.enabled = false;
                }
            }
        }
        else if (direction == Vector2Int.down)
        {
            if (DoorU != null)
            {
                var collider = DoorU.GetComponent<Collider2D>();
                if (collider != null)
                {
                    collider.enabled = false;
                }
            }
        }
        else if (direction == Vector2Int.left)
        {
            if (DoorR != null)
            {
                var collider = DoorR.GetComponent<Collider2D>();
                if (collider != null)
                {
                    collider.enabled = false;
                }
            }
        }
        else if (direction == Vector2Int.right)
        {
            if (DoorL != null)
            {
                var collider = DoorL.GetComponent<Collider2D>();
                if (collider != null)
                {
                    collider.enabled = false;
                }
            }
        }
    }
} 