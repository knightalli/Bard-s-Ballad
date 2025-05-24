using UnityEngine;

public class BossRoom : Room
{
    [Header("Boss Room Settings")]
    public bool isBossRoom = true;
    public string entranceDirection; // Направление, откуда игрок вошел

    void Start()
    {
        // Отключаем все двери кроме той, через которую игрок вошел
        if (DoorU != null) DoorU.SetActive(entranceDirection != "up");
        if (DoorD != null) DoorD.SetActive(entranceDirection != "down");
        if (DoorL != null) DoorL.SetActive(entranceDirection != "left");
        if (DoorR != null) DoorR.SetActive(entranceDirection != "right");
        
        // Отключаем коллайдер только у двери, через которую игрок вошел
        if (entranceDirection == "up")
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
        else if (entranceDirection == "down")
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
        else if (entranceDirection == "left")
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
        else if (entranceDirection == "right")
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