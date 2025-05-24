// ItemSO.cs
using UnityEngine;

public enum StatType
{
    Power = 0,
    Cooldown = 1,
    Health = 2
}

[CreateAssetMenu(menuName = "Inventory/Item", fileName = "NewItem")]
public class ItemSO : ScriptableObject
{
    public string itemName;
    public Sprite icon;
    public float bonusValue;
    public StatType statType;
    public AudioClip[] audioClips;
}
