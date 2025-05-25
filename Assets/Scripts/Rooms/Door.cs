using UnityEngine;

public class Door : MonoBehaviour
{
    public bool isLocked = false;
    public bool wasLocked = false;
    public bool toBoss = false; // Флаг для двери, ведущей к боссу

    private void Start()
    {
        // Если дверь заблокирована при старте, отмечаем её как wasLocked только если это не дверь к боссу
        if (isLocked && !toBoss)
        {
            wasLocked = true;
            gameObject.SetActive(true);
        }
    }

    public void Lock()
    {
        // Устанавливаем wasLocked только если дверь была открыта (не активна) и не ведёт к боссу
        if (!gameObject.activeSelf && !toBoss)
        {
            wasLocked = true;
        }
        
        isLocked = true;
        gameObject.SetActive(true);
    }

    public void Unlock()
    {
        isLocked = false;
        if (wasLocked && !toBoss) // Открываем дверь только если она была заблокирована и не ведёт к боссу
        {
            gameObject.SetActive(false);
            wasLocked = false;
        }
    }
} 