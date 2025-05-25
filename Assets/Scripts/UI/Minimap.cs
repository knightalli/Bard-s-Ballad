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

    private void Start()
    {
        Debug.Log("Minimap: Start");
        
        // Проверяем префабы
        if (roomIconPrefab == null) Debug.LogError("Minimap: roomIconPrefab не назначен!");
        if (currentRoomIconPrefab == null) Debug.LogError("Minimap: currentRoomIconPrefab не назначен!");
        if (bossRoomIconPrefab == null) Debug.LogError("Minimap: bossRoomIconPrefab не назначен!");
        if (startRoomIconPrefab == null) Debug.LogError("Minimap: startRoomIconPrefab не назначен!");
        if (visitedRoomIconPrefab == null) Debug.LogError("Minimap: visitedRoomIconPrefab не назначен!");

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
            
            Debug.Log($"Minimap: Настроен RectTransform, позиция: {rectTransform.anchoredPosition}, размер: {rectTransform.sizeDelta}");
        }
        else
        {
            Debug.LogError("Minimap: Не найден компонент RectTransform!");
        }

        // Находим существующий контейнер для иконок
        contentRect = transform.Find("MinimapContent")?.GetComponent<RectTransform>();
        if (contentRect == null)
        {
            Debug.LogError("Minimap: Не найден MinimapContent!");
            return;
        }

        // Находим RoomsPlacer
        RoomsPlacer roomsPlacer = FindObjectOfType<RoomsPlacer>();
        if (roomsPlacer != null)
        {
            // Подписываемся на событие создания комнаты
            roomsPlacer.OnRoomCreated += AddRoomToMinimap;
            Debug.Log("Minimap: Подписался на события RoomsPlacer");
        }
        else
        {
            Debug.LogError("Minimap: RoomsPlacer не найден!");
        }
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
        Debug.Log($"Minimap: Добавлена комната {room.name} в позиции: {roomPos}, реальная позиция: {room.transform.position}, isStartRoom: {room.isStartRoom}, уже есть иконка: {roomIcons.ContainsKey(roomPos)}");

        // Сохраняем комнату
        rooms[roomPos] = room;

        // Определяем тип комнаты
        if (room.isStartRoom)
        {
            startRoomPosition = roomPos;
            Debug.Log($"Minimap: Обработка стартовой комнаты, позиция: {roomPos}, startRoomIconPrefab: {(startRoomIconPrefab != null ? "назначен" : "не назначен")}");
            // Создаем иконку стартовой комнаты только один раз
            if (!roomIcons.ContainsKey(roomPos))
            {
                CreateRoomIcon(roomPos, startRoomIconPrefab);
                Debug.Log($"Minimap: Создана иконка стартовой комнаты в позиции {roomPos}");
            }
            else
            {
                Debug.Log($"Minimap: Иконка стартовой комнаты уже существует в позиции {roomPos}");
            }
            // Устанавливаем как текущую комнату
            UpdateCurrentRoom(roomPos);
        }
        else if (room is BossRoom)
        {
            bossRoomPosition = roomPos;
            CreateRoomIcon(roomPos, bossRoomIconPrefab);
            Debug.Log($"Minimap: Создана иконка комнаты босса в позиции {roomPos}");
        }
        else
        {
            CreateRoomIcon(roomPos, roomIconPrefab);
            Debug.Log($"Minimap: Создана иконка обычной комнаты в позиции {roomPos}");
        }

        Debug.Log($"Minimap: Всего иконок: {roomIcons.Count}");
    }

    private void CreateRoomIcon(Vector2Int position, GameObject iconPrefab)
    {
        if (iconPrefab == null)
        {
            Debug.LogError($"Minimap: Префаб иконки не назначен для позиции {position}");
            return;
        }

        if (contentRect == null)
        {
            Debug.LogError("Minimap: contentRect не назначен!");
            return;
        }

        // Создаем иконку
        GameObject icon = Instantiate(iconPrefab, contentRect);
        RectTransform iconRect = icon.GetComponent<RectTransform>();
        
        if (iconRect == null)
        {
            Debug.LogError($"Minimap: У иконки нет компонента RectTransform!");
            return;
        }

        // Устанавливаем позицию относительно центра миникарты
        float x = position.x * 30;
        float y = position.y * 30;
        iconRect.anchoredPosition = new Vector2(x, y);
        Debug.Log($"Minimap: Создана иконка в позиции {position}, anchoredPosition: {iconRect.anchoredPosition}");

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