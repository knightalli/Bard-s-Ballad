using UnityEngine;

public class Door : MonoBehaviour
{
    public bool wasLocked = false;

    public void Lock()
    {
        if (!gameObject.activeSelf)
        {
            wasLocked = true;
        }
        gameObject.SetActive(true);
    }

    public void Unlock()
    {
        if (wasLocked)
        {
            gameObject.SetActive(false);
            wasLocked = false;
        }
    }
} 