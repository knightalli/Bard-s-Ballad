using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class RoomsPlacer : MonoBehaviour
{
    public Room[] RoomPrefabs;
    public BossRoom[] BossRoomPrefabs;
    public Room StartingRoom;

    private Room[,] spawnedRooms;
    private int roomsToGenerate = 12;
    private int currentRoomCount = 0;
    private Vector2Int bossRoomPosition;

    void Start()
    {
        spawnedRooms = new Room[11,11];
        spawnedRooms[5,5] = StartingRoom;
        currentRoomCount = 1;

        // Генерируем обычные комнаты
        for (int i = 0; i < roomsToGenerate - 2; i++) // -2 потому что одна комната уже есть (стартовая) и одна будет боссом
        {
            PlaceOneRoom();
        }

        // Размещаем комнату босса
        PlaceBossRoom();
    }

    private void PlaceBossRoom()
    {
        // Находим самую дальнюю комнату от стартовой
        Vector2Int farthestRoom = FindFarthestRoom();
        
        // Находим свободное место рядом с самой дальней комнатой
        HashSet<Vector2Int> vacantPlaces = new HashSet<Vector2Int>();
        int maxX = spawnedRooms.GetLength(0) - 1;
        int maxY = spawnedRooms.GetLength(1) - 1;

        if (farthestRoom.x > 0 && spawnedRooms[farthestRoom.x - 1, farthestRoom.y] == null) 
            vacantPlaces.Add(new Vector2Int(farthestRoom.x - 1, farthestRoom.y));
        if (farthestRoom.y > 0 && spawnedRooms[farthestRoom.x, farthestRoom.y - 1] == null) 
            vacantPlaces.Add(new Vector2Int(farthestRoom.x, farthestRoom.y - 1));
        if (farthestRoom.x < maxX && spawnedRooms[farthestRoom.x + 1, farthestRoom.y] == null) 
            vacantPlaces.Add(new Vector2Int(farthestRoom.x + 1, farthestRoom.y));
        if (farthestRoom.y < maxY && spawnedRooms[farthestRoom.x, farthestRoom.y + 1] == null) 
            vacantPlaces.Add(new Vector2Int(farthestRoom.x, farthestRoom.y + 1));

        if (vacantPlaces.Count == 0) return;

        // Выбираем случайное свободное место
        Vector2Int bossPosition = vacantPlaces.ElementAt(Random.Range(0, vacantPlaces.Count));
        
        // Создаем комнату босса
        BossRoom bossRoom = Instantiate(BossRoomPrefabs[Random.Range(0, BossRoomPrefabs.Length)]);
        
        // Размещаем комнату
        bossRoom.transform.position = new Vector3(bossPosition.x - 5, bossPosition.y - 5, 0) * 22;
        spawnedRooms[bossPosition.x, bossPosition.y] = bossRoom;
        
        // Сохраняем позицию комнаты босса
        bossRoomPosition = bossPosition;
        
        // Определяем направление входа (от дальней комнаты к комнате босса)
        Vector2Int entranceDirection = bossPosition - farthestRoom;
        bossRoom.SetEntranceDirection(entranceDirection);
        
        // Соединяем с соседней комнатой
        ConnectBossRoom(bossRoom, bossPosition, farthestRoom);
    }

    private Vector2Int FindFarthestRoom()
    {
        Vector2Int farthestRoom = new Vector2Int(5, 5); // Стартовая комната
        float maxDistance = 0;

        for (int x = 0; x < spawnedRooms.GetLength(0); x++)
        {
            for (int y = 0; y < spawnedRooms.GetLength(1); y++)
            {
                if (spawnedRooms[x, y] != null)
                {
                    float distance = Vector2Int.Distance(new Vector2Int(5, 5), new Vector2Int(x, y));
                    if (distance > maxDistance)
                    {
                        maxDistance = distance;
                        farthestRoom = new Vector2Int(x, y);
                    }
                }
            }
        }

        return farthestRoom;
    }

    private void ConnectBossRoom(BossRoom bossRoom, Vector2Int bossPos, Vector2Int neighborPos)
    {
        Vector2Int direction = bossPos - neighborPos;
        Room neighborRoom = spawnedRooms[neighborPos.x, neighborPos.y];

        if (direction == Vector2Int.up)
        {
            if (bossRoom.DoorD != null && neighborRoom.DoorU != null)
            {
                bossRoom.DoorD.SetActive(false);
                neighborRoom.DoorU.SetActive(false);
            }
        }
        else if (direction == Vector2Int.down)
        {
            if (bossRoom.DoorU != null && neighborRoom.DoorD != null)
            {
                bossRoom.DoorU.SetActive(false);
                neighborRoom.DoorD.SetActive(false);
            }
        }
        else if (direction == Vector2Int.left)
        {
            if (bossRoom.DoorR != null && neighborRoom.DoorL != null)
            {
                bossRoom.DoorR.SetActive(false);
                neighborRoom.DoorL.SetActive(false);
            }
        }
        else if (direction == Vector2Int.right)
        {
            if (bossRoom.DoorL != null && neighborRoom.DoorR != null)
            {
                bossRoom.DoorL.SetActive(false);
                neighborRoom.DoorR.SetActive(false);
            }
        }
    }

    private void PlaceOneRoom()
    {
        HashSet<Vector2Int> vacantPlaces = new HashSet<Vector2Int>();
        for (int x = 0; x < spawnedRooms.GetLength(0); x++)
        {
            for (int y = 0; y < spawnedRooms.GetLength(1); y++)
            {
                if (spawnedRooms[x, y] == null) continue;
                
                int maxX = spawnedRooms.GetLength(0) - 1;
                int maxY = spawnedRooms.GetLength(1) - 1;

                if (x > 0 && spawnedRooms[x - 1, y] == null) vacantPlaces.Add(new Vector2Int(x - 1, y));
                if (y > 0 && spawnedRooms[x, y - 1] == null) vacantPlaces.Add(new Vector2Int(x, y - 1));
                if (x < maxX && spawnedRooms[x + 1, y] == null) vacantPlaces.Add(new Vector2Int(x + 1, y));
                if (y < maxY && spawnedRooms[x, y + 1] == null) vacantPlaces.Add(new Vector2Int(x, y + 1));
            }
        }

        Room newRoom = Instantiate(RoomPrefabs[Random.Range(0, RoomPrefabs.Length)]);

        int limit = 500;
        while (limit-- > 0)
        {
            Vector2Int position = vacantPlaces.ElementAt(Random.Range(0, vacantPlaces.Count));
            newRoom.RotateRandomly();

            if (ConnectToSomething(newRoom, position))
            {
                newRoom.transform.position = new Vector3(position.x - 5, position.y - 5, 0) * 22;
                spawnedRooms[position.x, position.y] = newRoom;
                currentRoomCount++;
                break;
            }
        }       
    }

    private bool ConnectToSomething(Room room, Vector2Int p)
    {
        int maxX = spawnedRooms.GetLength(0) - 1;
        int maxY = spawnedRooms.GetLength(1) - 1;

        List<Vector2Int> neighbours = new List<Vector2Int>();

        if (room.DoorU != null && p.y < maxY && spawnedRooms[p.x, p.y + 1]?.DoorD != null) neighbours.Add(Vector2Int.up);
        if (room.DoorD != null && p.y > 0 && spawnedRooms[p.x, p.y - 1]?.DoorU != null) neighbours.Add(Vector2Int.down);
        if (room.DoorL != null && p.x > 0 && spawnedRooms[p.x - 1, p.y]?.DoorR != null) neighbours.Add(Vector2Int.left);
        if (room.DoorR != null && p.x < maxX && spawnedRooms[p.x + 1, p.y]?.DoorL != null) neighbours.Add(Vector2Int.right);

        if (neighbours.Count == 0) return false;

        Vector2Int selectedDirection = neighbours[Random.Range(0, neighbours.Count)];
        Room selectedRoom = spawnedRooms[p.x + selectedDirection.x, p.y + selectedDirection.y];

        if (selectedDirection == Vector2Int.up)
        {
            room.DoorU.SetActive(false);
            selectedRoom.DoorD.SetActive(false);
        }
        else if (selectedDirection == Vector2Int.down)
        {
            room.DoorD.SetActive(false);
            selectedRoom.DoorU.SetActive(false);
        }
        else if (selectedDirection == Vector2Int.left)
        {
            room.DoorL.SetActive(false);
            selectedRoom.DoorR.SetActive(false);
        }
        else if (selectedDirection == Vector2Int.right)
        {
            room.DoorR.SetActive(false);
            selectedRoom.DoorL.SetActive(false);
        }

        return true;
    }
}
