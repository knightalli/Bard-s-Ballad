using UnityEngine;
using System.Collections.Generic;

public class Room : MonoBehaviour
{
    public GameObject DoorU;
    public GameObject DoorD;
    public GameObject DoorL;
    public GameObject DoorR;
    
    public GameObject RoomMoverU;
    public GameObject RoomMoverD;
    public GameObject RoomMoverL;
    public GameObject RoomMoverR;

    [Header("Enemy Spawn Settings")]
    public Transform[] enemySpawnPoints;
    public GameObject[] possibleEnemies;
    public int minEnemies = 2;
    public int maxEnemies = 4;

    [Header("Upgrade Spawn Settings")]
    public Transform[] upgradeSpawnPoints;
    public GameObject[] possibleUpgrades;
    public int minUpgrades = 1;
    public int maxUpgrades = 2;

    private List<GameObject> spawnedEnemies = new List<GameObject>();
    private List<GameObject> spawnedUpgrades = new List<GameObject>();
    public bool isVisited = false;
    private bool isRoomCleared = false;

    // Список для хранения активных дверей
    private List<GameObject> activeDoors = new List<GameObject>();
    private List<GameObject> activeRoomMovers = new List<GameObject>();

    public bool isStartRoom = false;

    void Start()
    {
        // Добавляем триггер на комнату, если его нет
        if (GetComponent<BoxCollider2D>() == null)
        {
            BoxCollider2D roomTrigger = gameObject.AddComponent<BoxCollider2D>();
            roomTrigger.isTrigger = true;
            roomTrigger.size = new Vector2(20, 20);
        }
    }

    public void OnRoomEnter()
    {
        if (!isRoomCleared)
        {
            // Проверяем, есть ли враги для спавна
            if (possibleEnemies.Length > 0 && minEnemies > 0)
            {
                LockDoors();
                SpawnEnemies();
            }
            else
            {
                // Если врагов нет, помечаем комнату как пройденную
                isRoomCleared = true;
            }
        }

        // Отмечаем комнату как посещённую
        if (!isVisited)
        {
            isVisited = true;
        }
    }

    private void LockDoors()
    {
        if (isStartRoom) return;

        if (RoomMoverU != null && RoomMoverU.activeSelf)
        {
            activeRoomMovers.Add(RoomMoverU);
            RoomMoverU.SetActive(false);
            if (DoorU != null)
            {
                activeDoors.Add(DoorU);
                Door door = DoorU.GetComponent<Door>();
                if (door != null)
                {
                    door.Lock();
                }
                else
                {
                    DoorU.SetActive(true);
                }
            }
        }

        if (RoomMoverD != null && RoomMoverD.activeSelf)
        {
            activeRoomMovers.Add(RoomMoverD);
            RoomMoverD.SetActive(false);
            if (DoorD != null)
            {
                activeDoors.Add(DoorD);
                Door door = DoorD.GetComponent<Door>();
                if (door != null)
                {
                    door.Lock();
                }
                else
                {
                    DoorD.SetActive(true);
                }
            }
        }

        if (RoomMoverL != null && RoomMoverL.activeSelf)
        {
            activeRoomMovers.Add(RoomMoverL);
            RoomMoverL.SetActive(false);
            if (DoorL != null)
            {
                activeDoors.Add(DoorL);
                Door door = DoorL.GetComponent<Door>();
                if (door != null)
                {
                    door.Lock();
                }
                else
                {
                    DoorL.SetActive(true);
                }
            }
        }

        if (RoomMoverR != null && RoomMoverR.activeSelf)
        {
            activeRoomMovers.Add(RoomMoverR);
            RoomMoverR.SetActive(false);
            if (DoorR != null)
            {
                activeDoors.Add(DoorR);
                Door door = DoorR.GetComponent<Door>();
                if (door != null)
                {
                    door.Lock();
                }
                else
                {
                    DoorR.SetActive(true);
                }
            }
        }
    }

    private void UnlockDoors()
    {
        foreach (var roomMover in activeRoomMovers)
        {
            if (roomMover != null)
            {
                roomMover.SetActive(true);
            }
        }

        foreach (var door in activeDoors)
        {
            if (door != null)
            {
                Door doorComponent = door.GetComponent<Door>();
                if (doorComponent != null)
                {
                    // Открываем дверь только если она была заблокирована и не ведёт к боссу
                    if (doorComponent.wasLocked && !doorComponent.toBoss)
                    {
                        doorComponent.Unlock();
                    }
                }
                else
                {
                    door.SetActive(false);
                }
            }
        }

        activeRoomMovers.Clear();
        activeDoors.Clear();
    }

    private void SpawnEnemies()
    {
        if (enemySpawnPoints.Length == 0 || possibleEnemies.Length == 0) return;

        int enemyCount = Random.Range(minEnemies, maxEnemies + 1);
        
        for (int i = 0; i < enemyCount; i++)
        {
            Transform spawnPoint = enemySpawnPoints[Random.Range(0, enemySpawnPoints.Length)];
            GameObject enemyPrefab = possibleEnemies[Random.Range(0, possibleEnemies.Length)];
            
            GameObject enemy = Instantiate(enemyPrefab, spawnPoint.position, Quaternion.identity);
            Enemy enemyComponent = enemy.GetComponent<Enemy>();
            if (enemyComponent != null)
            {
                enemyComponent.SetCurrentRoom(this);
            }
            spawnedEnemies.Add(enemy);
        }
    }

    private void SpawnUpgrades()
    {
        if (possibleUpgrades.Length > 0 && upgradeSpawnPoints.Length > 0)
        {
            int upgradeCount = Random.Range(minUpgrades, maxUpgrades + 1);
            for (int i = 0; i < upgradeCount; i++)
            {
                Transform spawnPoint = upgradeSpawnPoints[Random.Range(0, upgradeSpawnPoints.Length)];
                GameObject upgradePrefab = possibleUpgrades[Random.Range(0, possibleUpgrades.Length)];
                GameObject upgrade = Instantiate(upgradePrefab, spawnPoint.position, Quaternion.identity);
                upgrade.transform.parent = transform;
                spawnedUpgrades.Add(upgrade);
            }
        }
    }

    public void OnEnemyDeath(GameObject enemy)
    {
        spawnedEnemies.Remove(enemy);

        if (spawnedEnemies.Count == 0 && !isRoomCleared)
        {
            isRoomCleared = true;
            UnlockDoors();
            SpawnUpgrades();

            // Проверяем количество посещённых комнат
            if (RoomManager.Instance != null)
            {
                int visitedRooms = 0;
                foreach (var room in FindObjectsOfType<Room>())
                {
                    if (room.isVisited && !(room is BossRoom))
                    {
                        visitedRooms++;
                    }
                }

                // Если посещено 11 комнат, открываем дверь к боссу
                if (RoomManager.Instance.IsLastRoomRemaining(visitedRooms))
                {
                    Debug.Log("Все комнаты посещены, пытаемся открыть дверь к боссу");
                    
                    // Находим комнату босса
                    var bossRoom = FindObjectOfType<BossRoom>();
                    if (bossRoom != null)
                    {
                        Debug.Log($"Найдена комната босса, направление входа: {bossRoom.entranceDirection}");
                        
                        // Находим комнату, которая прилегает к боссу
                        var rooms = FindObjectsOfType<Room>();
                        foreach (var room in rooms)
                        {
                            // Пропускаем все комнаты босса
                            if (room is BossRoom) continue;

                            // Проверяем, является ли комната соседней
                            Vector3 roomPosition = room.transform.position;
                            Vector3 bossPosition = bossRoom.transform.position;
                            bool isAdjacent = false;

                            if (bossRoom.entranceDirection == "up" && Mathf.Approximately(roomPosition.y, bossPosition.y + 22))
                            {
                                isAdjacent = true;
                            }
                            else if (bossRoom.entranceDirection == "down" && Mathf.Approximately(roomPosition.y, bossPosition.y - 22))
                            {
                                isAdjacent = true;
                            }
                            else if (bossRoom.entranceDirection == "left" && Mathf.Approximately(roomPosition.x, bossPosition.x - 22))
                            {
                                isAdjacent = true;
                            }
                            else if (bossRoom.entranceDirection == "right" && Mathf.Approximately(roomPosition.x, bossPosition.x + 22))
                            {
                                isAdjacent = true;
                            }

                            if (isAdjacent)
                            {
                                Debug.Log($"Нашли соседнюю комнату {room.name} на позиции {roomPosition}");

                                // Открываем дверь, которая ведёт к боссу
                                if (bossRoom.entranceDirection == "up" && room.DoorD != null)
                                {
                                    Debug.Log($"Открываем дверь вниз в комнате {room.name}");
                                    var door = room.DoorD.GetComponent<Door>();
                                    if (door != null)
                                    {
                                        door.isLocked = false;
                                        door.gameObject.SetActive(false);
                                    }
                                    else
                                    {
                                        room.DoorD.SetActive(false);
                                    }
                                    break;
                                }
                                else if (bossRoom.entranceDirection == "down" && room.DoorU != null)
                                {
                                    Debug.Log($"Открываем дверь вверх в комнате {room.name}");
                                    var door = room.DoorU.GetComponent<Door>();
                                    if (door != null)
                                    {
                                        door.isLocked = false;
                                        door.gameObject.SetActive(false);
                                    }
                                    else
                                    {
                                        room.DoorU.SetActive(false);
                                    }
                                    break;
                                }
                                else if (bossRoom.entranceDirection == "left" && room.DoorR != null)
                                {
                                    Debug.Log($"Открываем дверь вправо в комнате {room.name}");
                                    var door = room.DoorR.GetComponent<Door>();
                                    if (door != null)
                                    {
                                        door.isLocked = false;
                                        door.gameObject.SetActive(false);
                                    }
                                    else
                                    {
                                        room.DoorR.SetActive(false);
                                    }
                                    break;
                                }
                                else if (bossRoom.entranceDirection == "right" && room.DoorL != null)
                                {
                                    Debug.Log($"Открываем дверь влево в комнате {room.name}");
                                    var door = room.DoorL.GetComponent<Door>();
                                    if (door != null)
                                    {
                                        door.isLocked = false;
                                        door.gameObject.SetActive(false);
                                    }
                                    else
                                    {
                                        room.DoorL.SetActive(false);
                                    }
                                    break;
                                }
                            }
                        }
                    }
                    else
                    {
                        Debug.LogError("Не найдена комната босса!");
                    }
                }
            }
        }
    }

    public void RotateRandomly()
    {
        int count = Random.Range(0, 4);

        for (int i = 0; i < count; i++)
        {
            transform.Rotate(0, 0, 90);

            GameObject tmp = DoorL;
            DoorL = DoorU;
            DoorU = DoorR;
            DoorR = DoorD;
            DoorD = tmp;
        }

        if (count == 1)
        {
            ChangeRoom roomChangerLToD = RoomMoverL.GetComponent<ChangeRoom>();
            roomChangerLToD.playerChangePos = new Vector3(0f, -4f);

            roomChangerLToD.limitChangeBottom = -22;
            roomChangerLToD.limitChangeUpper = -22;
            roomChangerLToD.limitChangeLeft = 0;
            roomChangerLToD.limitChangeRight = 0;

            ChangeRoom roomChangerUToL = RoomMoverU.GetComponent<ChangeRoom>();
            roomChangerUToL.playerChangePos = new Vector3(-4f, 0f);

            roomChangerUToL.limitChangeBottom = 0;
            roomChangerUToL.limitChangeUpper = 0;
            roomChangerUToL.limitChangeLeft = -22;
            roomChangerUToL.limitChangeRight = -22;

            ChangeRoom roomChangerRToU = RoomMoverR.GetComponent<ChangeRoom>();
            roomChangerRToU.playerChangePos = new Vector3(0f, 4f);

            roomChangerRToU.limitChangeBottom = 22;
            roomChangerRToU.limitChangeUpper = 22;
            roomChangerRToU.limitChangeLeft = 0;
            roomChangerRToU.limitChangeRight = 0;

            ChangeRoom roomChangerDToR = RoomMoverD.GetComponent<ChangeRoom>();
            roomChangerDToR.playerChangePos = new Vector3(4f, 0f);

            roomChangerDToR.limitChangeBottom = 0;
            roomChangerDToR.limitChangeUpper = 0;
            roomChangerDToR.limitChangeLeft = 22;
            roomChangerDToR.limitChangeRight = 22;
        }
        else if (count == 2)
        {
            ChangeRoom roomChangerLToR = RoomMoverL.GetComponent<ChangeRoom>();
            roomChangerLToR.playerChangePos = new Vector3(4f, 0f);

            roomChangerLToR.limitChangeBottom = 0;
            roomChangerLToR.limitChangeUpper = 0;
            roomChangerLToR.limitChangeLeft = 22;
            roomChangerLToR.limitChangeRight = 22;

            ChangeRoom roomChangerUToD = RoomMoverU.GetComponent<ChangeRoom>();
            roomChangerUToD.playerChangePos = new Vector3(0f, -4f);

            roomChangerUToD.limitChangeBottom = -22;
            roomChangerUToD.limitChangeUpper = -22;
            roomChangerUToD.limitChangeLeft = 0;
            roomChangerUToD.limitChangeRight = 0;

            ChangeRoom roomChangerRToL = RoomMoverR.GetComponent<ChangeRoom>();
            roomChangerRToL.playerChangePos = new Vector3(-4f, 0f);

            roomChangerRToL.limitChangeBottom = 0;
            roomChangerRToL.limitChangeUpper = 0;
            roomChangerRToL.limitChangeLeft = -22;
            roomChangerRToL.limitChangeRight = -22;

            ChangeRoom roomChangerDToU = RoomMoverD.GetComponent<ChangeRoom>();
            roomChangerDToU.playerChangePos = new Vector3(0f, 4f);

            roomChangerDToU.limitChangeBottom = 22;
            roomChangerDToU.limitChangeUpper = 22;
            roomChangerDToU.limitChangeLeft = 0;
            roomChangerDToU.limitChangeRight = 0;
        }
        else if (count == 3)
        {
            ChangeRoom roomChangerLToU = RoomMoverL.GetComponent<ChangeRoom>();
            roomChangerLToU.playerChangePos = new Vector3(0f, 4f);

            roomChangerLToU.limitChangeBottom = 22;
            roomChangerLToU.limitChangeUpper = 22;
            roomChangerLToU.limitChangeLeft = 0;
            roomChangerLToU.limitChangeRight = 0;

            ChangeRoom roomChangerUToR = RoomMoverU.GetComponent<ChangeRoom>();
            roomChangerUToR.playerChangePos = new Vector3(4f, 0f);

            roomChangerUToR.limitChangeBottom = 0;
            roomChangerUToR.limitChangeUpper = 0;
            roomChangerUToR.limitChangeLeft = 22;
            roomChangerUToR.limitChangeRight = 22;

            ChangeRoom roomChangerRToD = RoomMoverR.GetComponent<ChangeRoom>();
            roomChangerRToD.playerChangePos = new Vector3(0f, -4f);

            roomChangerRToD.limitChangeBottom = -22;
            roomChangerRToD.limitChangeUpper = -22;
            roomChangerRToD.limitChangeLeft = 0;
            roomChangerRToD.limitChangeRight = 0;

            ChangeRoom roomChangerDToL = RoomMoverD.GetComponent<ChangeRoom>();
            roomChangerDToL.playerChangePos = new Vector3(-4f, 0f);

            roomChangerDToL.limitChangeBottom = 0;
            roomChangerDToL.limitChangeUpper = 0;
            roomChangerDToL.limitChangeLeft = -22;
            roomChangerDToL.limitChangeRight = -22;
        }
    }
}
