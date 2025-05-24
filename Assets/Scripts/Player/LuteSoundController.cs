using UnityEngine;
using System.Collections.Generic;

public class LuteSoundController : MonoBehaviour
{
    private List<ItemSO> activeItems = new List<ItemSO>();
    private int[] currentSoundIndices;

    public void UpdateActiveItems(ItemSO[] items)
    {
        activeItems.Clear();
        foreach (var item in items)
        {
            if (item != null && item.audioClips != null && item.audioClips.Length > 0)
            {
                activeItems.Add(item);
                Debug.Log($"Добавлен предмет {item.itemName} с {item.audioClips.Length} звуками");
            }
        }
        
        // Инициализируем индексы для каждого активного предмета
        currentSoundIndices = new int[activeItems.Count];
        Debug.Log($"Всего активных предметов: {activeItems.Count}");
    }

    public AudioClip[] GetNextSounds()
    {
        if (activeItems.Count == 0)
        {
            Debug.Log("Нет активных предметов");
            return null;
        }

        AudioClip[] clipsToPlay = new AudioClip[activeItems.Count];
        
        // Для каждого активного предмета получаем следующий звук
        for (int i = 0; i < activeItems.Count; i++)
        {
            ItemSO item = activeItems[i];
            if (item.audioClips.Length > 0)
            {
                clipsToPlay[i] = item.audioClips[currentSoundIndices[i]];
                currentSoundIndices[i] = (currentSoundIndices[i] + 1) % item.audioClips.Length;
                Debug.Log($"Проигрываем звук {currentSoundIndices[i]} из предмета {item.itemName}");
            }
        }

        return clipsToPlay;
    }
} 