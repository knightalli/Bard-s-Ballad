using UnityEngine;
using System.Collections;

public class BossRoom : Room
{
    [Header("Boss Room Settings")]
    public bool isBossRoom = true;
    public string entranceDirection; // Направление, откуда игрок вошел
    [SerializeField] private GameObject bossPrefab; // Префаб босса
    [SerializeField] private Transform bossSpawnPoint; // Точка спавна босса
    private bool bossSpawned = false;
    private bool doorsClosed = false;

    void Start()
    {
        // Закрываем все двери в комнате босса
        if (DoorU != null) 
        {
            DoorU.SetActive(true);
            var door = DoorU.GetComponent<Door>();
            if (door != null)
            {
                door.Lock();
            }
        }
        if (DoorD != null) 
        {
            DoorD.SetActive(true);
            var door = DoorD.GetComponent<Door>();
            if (door != null)
            {
                door.Lock();
            }
        }
        if (DoorL != null) 
        {
            DoorL.SetActive(true);
            var door = DoorL.GetComponent<Door>();
            if (door != null)
            {
                door.Lock();
            }
        }
        if (DoorR != null) 
        {
            DoorR.SetActive(true);
            var door = DoorR.GetComponent<Door>();
            if (door != null)
            {
                door.Lock();
            }
        }

        // Закрываем дверь в соседней комнате, которая ведёт к боссу
        CloseAdjacentDoor();
    }

    private void CloseAdjacentDoor()
    {
        // Находим комнату, которая прилегает к боссу
        var rooms = FindObjectsOfType<Room>();
        bool doorClosed = false;

        foreach (var room in rooms)
        {
            // Пропускаем все комнаты босса
            if (room is BossRoom) continue;

            // Проверяем, является ли комната соседней
            Vector3 roomPosition = room.transform.position;
            Vector3 bossPosition = transform.position;
            bool isAdjacent = false;

            // Если вход сверху, ищем комнату сверху от босса (только по Y)
            if (entranceDirection == "up" && 
                Mathf.Approximately(roomPosition.y, bossPosition.y + 22) && 
                Mathf.Approximately(roomPosition.x, bossPosition.x))
            {
                isAdjacent = true;
            }
            // Если вход снизу, ищем комнату снизу от босса (только по Y)
            else if (entranceDirection == "down" && 
                     Mathf.Approximately(roomPosition.y, bossPosition.y - 22) && 
                     Mathf.Approximately(roomPosition.x, bossPosition.x))
            {
                isAdjacent = true;
            }
            // Если вход слева, ищем комнату слева от босса (только по X)
            else if (entranceDirection == "left" && 
                     Mathf.Approximately(roomPosition.x, bossPosition.x - 22) && 
                     Mathf.Approximately(roomPosition.y, bossPosition.y))
            {
                isAdjacent = true;
            }
            // Если вход справа, ищем комнату справа от босса (только по X)
            else if (entranceDirection == "right" && 
                     Mathf.Approximately(roomPosition.x, bossPosition.x + 22) && 
                     Mathf.Approximately(roomPosition.y, bossPosition.y))
            {
                isAdjacent = true;
            }

            if (isAdjacent)
            {
                // Закрываем дверь, которая ведёт к боссу
                if (entranceDirection == "up" && room.DoorD != null)
                {
                    var door = room.DoorD.GetComponent<Door>();
                    if (door != null)
                    {
                        door.toBoss = true; // Сначала устанавливаем флаг toBoss
                        door.Lock(); // Потом блокируем дверь
                    }
                    else
                    {
                        room.DoorD.SetActive(true);
                    }
                    doorClosed = true;
                    break;
                }
                else if (entranceDirection == "down" && room.DoorU != null)
                {
                    var door = room.DoorU.GetComponent<Door>();
                    if (door != null)
                    {
                        door.toBoss = true; // Сначала устанавливаем флаг toBoss
                        door.Lock(); // Потом блокируем дверь
                    }
                    else
                    {
                        room.DoorU.SetActive(true);
                    }
                    doorClosed = true;
                    break;
                }
                else if (entranceDirection == "left" && room.DoorR != null)
                {
                    var door = room.DoorR.GetComponent<Door>();
                    if (door != null)
                    {
                        door.toBoss = true; // Сначала устанавливаем флаг toBoss
                        door.Lock(); // Потом блокируем дверь
                    }
                    else
                    {
                        room.DoorR.SetActive(true);
                    }
                    doorClosed = true;
                    break;
                }
                else if (entranceDirection == "right" && room.DoorL != null)
                {
                    var door = room.DoorL.GetComponent<Door>();
                    if (door != null)
                    {
                        door.toBoss = true; // Сначала устанавливаем флаг toBoss
                        door.Lock(); // Потом блокируем дверь
                    }
                    else
                    {
                        room.DoorL.SetActive(true);
                    }
                    doorClosed = true;
                    break;
                }
            }
        }

        if (!doorClosed)
        {
            StartCoroutine(WaitForAdjacentRoom());
        }
    }

    private IEnumerator WaitForAdjacentRoom()
    {
        Vector3 bossPosition = transform.position;
        
        while (true)
        {
            yield return new WaitForSeconds(0.5f); // Проверяем каждые полсекунды
            
            var rooms = FindObjectsOfType<Room>();
            foreach (var room in rooms)
            {
                if (room is BossRoom) continue;

                Vector3 roomPosition = room.transform.position;
                bool isAdjacent = false;

                // Если вход сверху, ищем комнату сверху от босса (только по Y)
                if (entranceDirection == "up" && 
                    Mathf.Approximately(roomPosition.y, bossPosition.y + 22) && 
                    Mathf.Approximately(roomPosition.x, bossPosition.x))
                {
                    isAdjacent = true;
                }
                // Если вход снизу, ищем комнату снизу от босса (только по Y)
                else if (entranceDirection == "down" && 
                         Mathf.Approximately(roomPosition.y, bossPosition.y - 22) && 
                         Mathf.Approximately(roomPosition.x, bossPosition.x))
                {
                    isAdjacent = true;
                }
                // Если вход слева, ищем комнату слева от босса (только по X)
                else if (entranceDirection == "left" && 
                         Mathf.Approximately(roomPosition.x, bossPosition.x - 22) && 
                         Mathf.Approximately(roomPosition.y, bossPosition.y))
                {
                    isAdjacent = true;
                }
                // Если вход справа, ищем комнату справа от босса (только по X)
                else if (entranceDirection == "right" && 
                         Mathf.Approximately(roomPosition.x, bossPosition.x + 22) && 
                         Mathf.Approximately(roomPosition.y, bossPosition.y))
                {
                    isAdjacent = true;
                }

                if (isAdjacent)
                {
                    if (entranceDirection == "up" && room.DoorD != null)
                    {
                        var door = room.DoorD.GetComponent<Door>();
                        if (door != null)
                        {
                            door.toBoss = true; // Сначала устанавливаем флаг toBoss
                            door.Lock(); // Потом блокируем дверь
                        }
                        else
                        {
                            room.DoorD.SetActive(true);
                        }
                        yield break;
                    }
                    else if (entranceDirection == "down" && room.DoorU != null)
                    {
                        var door = room.DoorU.GetComponent<Door>();
                        if (door != null)
                        {
                            door.toBoss = true; // Сначала устанавливаем флаг toBoss
                            door.Lock(); // Потом блокируем дверь
                        }
                        else
                        {
                            room.DoorU.SetActive(true);
                        }
                        yield break;
                    }
                    else if (entranceDirection == "left" && room.DoorR != null)
                    {
                        var door = room.DoorR.GetComponent<Door>();
                        if (door != null)
                        {
                            door.toBoss = true; // Сначала устанавливаем флаг toBoss
                            door.Lock(); // Потом блокируем дверь
                        }
                        else
                        {
                            room.DoorR.SetActive(true);
                        }
                        yield break;
                    }
                    else if (entranceDirection == "right" && room.DoorL != null)
                    {
                        var door = room.DoorL.GetComponent<Door>();
                        if (door != null)
                        {
                            door.toBoss = true; // Сначала устанавливаем флаг toBoss
                            door.Lock(); // Потом блокируем дверь
                        }
                        else
                        {
                            room.DoorL.SetActive(true);
                        }
                        yield break;
                    }
                }
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !doorsClosed)
        {
            // Закрываем все двери
            if (DoorU != null) 
            {
                var door = DoorU.GetComponent<Door>();
                if (door != null)
                {
                    door.toBoss = false; // Это не дверь к боссу
                    door.Lock();
                }
                else
                {
                    DoorU.SetActive(true);
                }
            }
            if (DoorD != null) 
            {
                var door = DoorD.GetComponent<Door>();
                if (door != null)
                {
                    door.toBoss = false; // Это не дверь к боссу
                    door.Lock();
                }
                else
                {
                    DoorD.SetActive(true);
                }
            }
            if (DoorL != null) 
            {
                var door = DoorL.GetComponent<Door>();
                if (door != null)
                {
                    door.toBoss = false; // Это не дверь к боссу
                    door.Lock();
                }
                else
                {
                    DoorL.SetActive(true);
                }
            }
            if (DoorR != null) 
            {
                var door = DoorR.GetComponent<Door>();
                if (door != null)
                {
                    door.toBoss = false; // Это не дверь к боссу
                    door.Lock();
                }
                else
                {
                    DoorR.SetActive(true);
                }
            }

            doorsClosed = true;

            // Спавним босса
            if (!bossSpawned && bossPrefab != null && bossSpawnPoint != null)
            {
                var boss = Instantiate(bossPrefab, bossSpawnPoint.position, Quaternion.identity);
                var bossController = boss.GetComponent<BossFightController>();
                if (bossController != null)
                {
                    // Убираем подписку на событие победы над боссом
                    // bossController.BossDefeated += OpenDoors;
                }
                Debug.Log("Босс заспавнен в позиции: " + bossSpawnPoint.position);
                bossSpawned = true;
            }
            else
            {
                Debug.LogError("Не назначен префаб босса или точка спавна!");
            }
        }
    }

    private void OpenDoors()
    {
        // Открываем все двери, кроме той, через которую игрок вошел
        if (DoorU != null && entranceDirection != "up")
        {
            var door = DoorU.GetComponent<Door>();
            if (door != null)
            {
                door.Unlock();
            }
            else
            {
                DoorU.SetActive(false);
            }
        }
        if (DoorD != null && entranceDirection != "down")
        {
            var door = DoorD.GetComponent<Door>();
            if (door != null)
            {
                door.Unlock();
            }
            else
            {
                DoorD.SetActive(false);
            }
        }
        if (DoorL != null && entranceDirection != "left")
        {
            var door = DoorL.GetComponent<Door>();
            if (door != null)
            {
                door.Unlock();
            }
            else
            {
                DoorL.SetActive(false);
            }
        }
        if (DoorR != null && entranceDirection != "right")
        {
            var door = DoorR.GetComponent<Door>();
            if (door != null)
            {
                door.Unlock();
            }
            else
            {
                DoorR.SetActive(false);
            }
        }
    }
} 