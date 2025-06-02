using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class Minimap : MonoBehaviour
{
    public GameObject roomIconPrefab;
    public GameObject currentRoomIconPrefab;
    public GameObject bossRoomIconPrefab;
    public GameObject startRoomIconPrefab;
    public GameObject visitedRoomIconPrefab;

    private Dictionary<Vector2Int, GameObject> roomIcons = new Dictionary<Vector2Int, GameObject>();
    private Dictionary<Vector2Int, Room> rooms = new Dictionary<Vector2Int, Room>();
    private Vector2Int currentRoomPosition;
    private Vector2Int bossRoomPosition;
    private Vector2Int startRoomPosition;
    private RectTransform contentRect;

    private void Awake()
    {
        // Настраиваем RectTransform контейнера миникарты
        RectTransform rectTransform = GetComponent<RectTransform>();
        if (rectTransform != null)
        {
            // Устанавливаем якоря в правый верхний угол
            rectTransform.anchorMin = new Vector2(1, 1);
            rectTransform.anchorMax = new Vector2(1, 1);
            rectTransform.pivot = new Vector2(1, 1);
            
            // Устанавливаем позицию (отступ от края экрана)
            rectTransform.anchoredPosition = new Vector2(-20, -20);
            
            // Устанавливаем размер
            rectTransform.sizeDelta = new Vector2(300, 300);
        }
        else
        {

        }

        // Находим существующий контейнер для иконок
        contentRect = transform.Find("MinimapContent")?.GetComponent<RectTransform>();
        if (contentRect == null)
        {

            return;
        }

        // Находим RoomsPlacer
        RoomsPlacer roomsPlacer = FindObjectOfType<RoomsPlacer>();
        if (roomsPlacer != null)
        {
            // Подписываемся на событие создания комнаты
            roomsPlacer.OnRoomCreated += AddRoomToMinimap;
        }
        else
        {

        }
    }

    private void Update()
    {
            Debug.Log("MinimapContent children:");

            for (int i = 0; i < contentRect.childCount; i++)
            {
                Transform child = contentRect.GetChild(i);
                Debug.Log($"Child {i}: {child.name}");
            }

            Debug.Log($"Total children: {contentRect.childCount}");        
    }

    private void OnDestroy()
    {
        // Отписываемся от событий при уничтожении
        RoomsPlacer roomsPlacer = FindObjectOfType<RoomsPlacer>();
        if (roomsPlacer != null)
        {
            roomsPlacer.OnRoomCreated -= AddRoomToMinimap;
        }
    }

    private void AddRoomToMinimap(Room room)
    {
        if (room == null) return;

        Vector2Int roomPos = new Vector2Int(
            Mathf.RoundToInt(room.transform.position.x / 22),
            Mathf.RoundToInt(room.transform.position.y / 22)
        );

        // Сохраняем комнату
        rooms[roomPos] = room;

        // Определяем тип комнаты
        if (room.isStartRoom)
        {
            startRoomPosition = roomPos;
            // Создаем иконку стартовой комнаты только один раз
            if (!roomIcons.ContainsKey(roomPos))
            {
                CreateRoomIcon(roomPos, startRoomIconPrefab);
            }
            else
            {

            }
            // Устанавливаем как текущую комнату
            UpdateCurrentRoom(roomPos);
        }
        else if (room is BossRoom)
        {
            bossRoomPosition = roomPos;
            CreateRoomIcon(roomPos, bossRoomIconPrefab);
        }
        else
        {
            CreateRoomIcon(roomPos, roomIconPrefab);
        }

    }

    private void CreateRoomIcon(Vector2Int position, GameObject iconPrefab)
    {
        if (iconPrefab == null)
        {
            return;
        }

        if (contentRect == null)
        {
            return;
        }

        // Создаем иконку
        GameObject icon = Instantiate(iconPrefab, contentRect);
        RectTransform iconRect = icon.GetComponent<RectTransform>();
        
        if (iconRect == null)
        {
            return;
        }

        // Устанавливаем позицию относительно центра миникарты
        float x = position.x * 30;
        float y = position.y * 30;
        iconRect.anchoredPosition = new Vector2(x, y);

        // Сохраняем иконку
        roomIcons[position] = icon;
    }

    public void UpdateCurrentRoom(Vector2Int position)
    {
        // Удаляем старую иконку текущей комнаты
        if (roomIcons.ContainsKey(currentRoomPosition))
        {
            // Если это стартовая комната, восстанавливаем её иконку
            if (currentRoomPosition == startRoomPosition)
            {
                Destroy(roomIcons[currentRoomPosition]);
                roomIcons.Remove(currentRoomPosition);
                CreateRoomIcon(startRoomPosition, startRoomIconPrefab);
            }
            else
            {
                Destroy(roomIcons[currentRoomPosition]);
                roomIcons.Remove(currentRoomPosition);
                // Если комната была посещена, создаем иконку посещенной комнаты
                if (rooms.ContainsKey(currentRoomPosition) && rooms[currentRoomPosition].isVisited)
                {
                    CreateRoomIcon(currentRoomPosition, visitedRoomIconPrefab);
                }
            }
        }

        // Создаем новую иконку текущей комнаты
        currentRoomPosition = position;
        GameObject currentIcon = Instantiate(currentRoomIconPrefab, contentRect);
        RectTransform iconRect = currentIcon.GetComponent<RectTransform>();
        
        if (iconRect != null)
        {
            float x = position.x * 30;
            float y = position.y * 30;
            iconRect.anchoredPosition = new Vector2(x, y);
            
            // Устанавливаем иконку текущей комнаты поверх других
            currentIcon.transform.SetAsLastSibling();
        }

        // Сохраняем иконку
        roomIcons[position] = currentIcon;

        // Помечаем комнату как посещенную
        if (rooms.ContainsKey(position))
        {
            rooms[position].isVisited = true;
        }
    }
} 