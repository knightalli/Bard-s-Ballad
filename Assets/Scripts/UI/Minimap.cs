using UnityEngine;
using UnityEngine.UI;

public class Minimap : MonoBehaviour
{
    [SerializeField] private GameObject roomIconPrefab;
    [SerializeField] private GameObject currentRoomIconPrefab;
    [SerializeField] private GameObject bossRoomIconPrefab;
    [SerializeField] private GameObject startRoomIconPrefab;
    [SerializeField] private Transform minimapContent;
    [SerializeField] private float iconSize = 50f;
    [SerializeField] private float spacing = 10f;
    [SerializeField] private Vector2 minimapOffset = new Vector2(-20f, -20f);
    [SerializeField] private Vector2 minimapSize = new Vector2(400f, 400f);

    private RoomsPlacer roomsPlacer;
    private GameObject[,] roomIcons;
    private GameObject currentRoomIcon;
    private bool isInitialized = false;
    private Canvas minimapCanvas;

    void Start()
    {
        Debug.Log("Minimap: Start");
        
        // Проверяем префабы
        Debug.Log($"Minimap: Проверка префабов:");
        Debug.Log($"roomIconPrefab: {(roomIconPrefab != null ? "назначен" : "не назначен")}");
        Debug.Log($"currentRoomIconPrefab: {(currentRoomIconPrefab != null ? "назначен" : "не назначен")}");
        Debug.Log($"bossRoomIconPrefab: {(bossRoomIconPrefab != null ? "назначен" : "не назначен")}");
        Debug.Log($"startRoomIconPrefab: {(startRoomIconPrefab != null ? "назначен" : "не назначен")}");

        if (roomIconPrefab == null) Debug.LogError("Minimap: roomIconPrefab не назначен!");
        if (currentRoomIconPrefab == null) Debug.LogError("Minimap: currentRoomIconPrefab не назначен!");
        if (bossRoomIconPrefab == null) Debug.LogError("Minimap: bossRoomIconPrefab не назначен!");
        if (startRoomIconPrefab == null) Debug.LogError("Minimap: startRoomIconPrefab не назначен!");

        // Создаем Canvas если его нет
        if (minimapContent == null)
        {
            Debug.Log("Minimap: Создаем новый Canvas для миникарты");
            
            // Создаем Canvas
            GameObject canvasObj = new GameObject("MinimapCanvas");
            minimapCanvas = canvasObj.AddComponent<Canvas>();
            minimapCanvas.renderMode = RenderMode.ScreenSpaceOverlay;
            minimapCanvas.sortingOrder = 100;
            
            // Добавляем CanvasScaler
            CanvasScaler scaler = canvasObj.AddComponent<CanvasScaler>();
            scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
            scaler.referenceResolution = new Vector2(1920, 1080);
            scaler.screenMatchMode = CanvasScaler.ScreenMatchMode.MatchWidthOrHeight;
            scaler.matchWidthOrHeight = 0.5f;
            
            // Добавляем GraphicRaycaster
            canvasObj.AddComponent<GraphicRaycaster>();
            
            // Создаем контейнер для иконок
            GameObject contentObj = new GameObject("MinimapContent");
            contentObj.transform.SetParent(canvasObj.transform, false);
            minimapContent = contentObj.transform;
            
            // Настраиваем RectTransform для контейнера
            RectTransform contentRect = contentObj.AddComponent<RectTransform>();
            contentRect.anchorMin = new Vector2(1, 1);
            contentRect.anchorMax = new Vector2(1, 1);
            contentRect.pivot = new Vector2(1, 1);
            contentRect.anchoredPosition = minimapOffset;
            contentRect.sizeDelta = minimapSize;
            
            // Создаем фон для миникарты
            GameObject backgroundObj = new GameObject("MinimapBackground");
            backgroundObj.transform.SetParent(canvasObj.transform, false);
            Image backgroundImage = backgroundObj.AddComponent<Image>();
            backgroundImage.color = new Color(0, 0, 0, 0.5f);
            
            // Настраиваем RectTransform для фона
            RectTransform backgroundRect = backgroundObj.GetComponent<RectTransform>();
            backgroundRect.anchorMin = new Vector2(1, 1);
            backgroundRect.anchorMax = new Vector2(1, 1);
            backgroundRect.pivot = new Vector2(1, 1);
            backgroundRect.anchoredPosition = minimapOffset;
            backgroundRect.sizeDelta = minimapSize;
            
            Debug.Log($"Minimap: Canvas и контейнер созданы, размер: {minimapSize}");
        }
        else
        {
            // Настраиваем существующий RectTransform
            RectTransform contentRect = minimapContent as RectTransform;
            if (contentRect != null)
            {
                contentRect.anchorMin = new Vector2(1, 1);
                contentRect.anchorMax = new Vector2(1, 1);
                contentRect.pivot = new Vector2(1, 1);
                contentRect.anchoredPosition = minimapOffset;
                contentRect.sizeDelta = minimapSize;
                Debug.Log($"Minimap: Настроен RectTransform для minimapContent, размер: {contentRect.sizeDelta}, позиция: {contentRect.anchoredPosition}");
            }
        }

        Debug.Log("Minimap: Ищем RoomsPlacer...");
        roomsPlacer = FindObjectOfType<RoomsPlacer>();
        if (roomsPlacer == null)
        {
            Debug.LogError("Minimap: RoomsPlacer не найден на сцене!");
            return;
        }
        Debug.Log("Minimap: RoomsPlacer найден!");
    }

    void Update()
    {
        if (!isInitialized)
        {
            if (roomsPlacer != null && roomsPlacer.spawnedRooms != null)
            {
                InitializeMinimap();
                isInitialized = true;
            }
            return;
        }

        // Находим текущую комнату игрока
        var player = GameObject.FindGameObjectWithTag("Player");
        if (player == null) return;

        // Ищем комнату, в которой находится игрок
        for (int x = 0; x < roomsPlacer.spawnedRooms.GetLength(0); x++)
        {
            for (int y = 0; y < roomsPlacer.spawnedRooms.GetLength(1); y++)
            {
                if (roomsPlacer.spawnedRooms[x, y] != null)
                {
                    // Проверяем, находится ли игрок в этой комнате
                    if (roomsPlacer.spawnedRooms[x, y].GetComponent<BoxCollider2D>().bounds.Contains(player.transform.position))
                    {
                        // Обновляем позицию иконки текущей комнаты
                        Vector2 newPos = new Vector2(
                            (x - 5) * (iconSize + spacing),
                            (y - 5) * (iconSize + spacing)
                        );
                        RectTransform currentIconRect = currentRoomIcon.GetComponent<RectTransform>();
                        if (currentIconRect != null)
                        {
                            currentIconRect.anchoredPosition = newPos;
                            Debug.Log($"Minimap: Обновлена позиция текущей комнаты: {newPos}");
                        }
                        return;
                    }
                }
            }
        }
    }

    private void InitializeMinimap()
    {
        Debug.Log($"Minimap: Инициализация миникарты, размер массива комнат: {roomsPlacer.spawnedRooms.GetLength(0)}x{roomsPlacer.spawnedRooms.GetLength(1)}");

        roomIcons = new GameObject[roomsPlacer.spawnedRooms.GetLength(0), roomsPlacer.spawnedRooms.GetLength(1)];

        // Создаем иконки для каждой комнаты
        for (int x = 0; x < roomsPlacer.spawnedRooms.GetLength(0); x++)
        {
            for (int y = 0; y < roomsPlacer.spawnedRooms.GetLength(1); y++)
            {
                if (roomsPlacer.spawnedRooms[x, y] != null)
                {
                    Debug.Log($"Minimap: Создаем иконку для комнаты в позиции ({x}, {y})");
                    
                    GameObject iconPrefab = roomIconPrefab;
                    
                    // Выбираем правильную иконку в зависимости от типа комнаты
                    if (roomsPlacer.spawnedRooms[x, y].isStartRoom)
                    {
                        iconPrefab = startRoomIconPrefab;
                        Debug.Log("Minimap: Это стартовая комната");
                    }
                    else if (roomsPlacer.spawnedRooms[x, y] is BossRoom)
                    {
                        iconPrefab = bossRoomIconPrefab;
                        Debug.Log("Minimap: Это комната босса");
                    }

                    // Создаем иконку комнаты
                    GameObject roomIcon = Instantiate(iconPrefab, minimapContent);
                    RectTransform iconRect = roomIcon.GetComponent<RectTransform>();
                    if (iconRect != null)
                    {
                        // Настраиваем RectTransform
                        iconRect.anchorMin = new Vector2(0.5f, 0.5f);
                        iconRect.anchorMax = new Vector2(0.5f, 0.5f);
                        iconRect.pivot = new Vector2(0.5f, 0.5f);
                        iconRect.sizeDelta = new Vector2(iconSize, iconSize);
                        
                        // Устанавливаем позицию
                        Vector2 iconPos = new Vector2(
                            (x - 5) * (iconSize + spacing),
                            (y - 5) * (iconSize + spacing)
                        );
                        iconRect.anchoredPosition = iconPos;
                        
                        Debug.Log($"Minimap: Размер иконки: {iconRect.sizeDelta}");
                        Debug.Log($"Minimap: Позиция иконки: {iconRect.anchoredPosition}");
                    }
                    
                    // Убедимся, что иконка видима
                    Image iconImage = roomIcon.GetComponent<Image>();
                    if (iconImage != null)
                    {
                        iconImage.raycastTarget = false;
                        Color iconColor = iconImage.color;
                        iconColor.a = 1f;
                        iconImage.color = iconColor;
                        Debug.Log($"Minimap: Цвет иконки: {iconColor}");
                    }
                    
                    roomIcons[x, y] = roomIcon;
                }
            }
        }

        // Создаем иконку текущей комнаты
        currentRoomIcon = Instantiate(currentRoomIconPrefab, minimapContent);
        
        // Настраиваем RectTransform текущей иконки
        RectTransform currentIconRect = currentRoomIcon.GetComponent<RectTransform>();
        if (currentIconRect != null)
        {
            currentIconRect.anchorMin = new Vector2(0.5f, 0.5f);
            currentIconRect.anchorMax = new Vector2(0.5f, 0.5f);
            currentIconRect.pivot = new Vector2(0.5f, 0.5f);
            currentIconRect.sizeDelta = new Vector2(iconSize * 1.2f, iconSize * 1.2f);
            Debug.Log($"Minimap: Размер текущей иконки: {currentIconRect.sizeDelta}");
        }
        
        // Убедимся, что иконка текущей комнаты видима
        Image currentIconImage = currentRoomIcon.GetComponent<Image>();
        if (currentIconImage != null)
        {
            currentIconImage.raycastTarget = false;
            Color iconColor = currentIconImage.color;
            iconColor.a = 1f;
            currentIconImage.color = iconColor;
            Debug.Log($"Minimap: Цвет текущей иконки: {iconColor}");
        }
        
        Debug.Log("Minimap: Инициализация завершена");
    }
} 