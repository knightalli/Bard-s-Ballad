using UnityEngine;

public class RoomManager : MonoBehaviour
{
    public static RoomManager Instance { get; private set; }
    private const int TOTAL_ROOMS = 8; // Общее количество комнат

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public bool IsLastRoomRemaining(int visitedRoomsCount)
    {
        // Если посещенных комнат на 1 меньше общего количества, значит осталась только комната босса
        return visitedRoomsCount == TOTAL_ROOMS - 1;
    }
} 