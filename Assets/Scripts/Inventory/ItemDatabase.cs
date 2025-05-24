// ItemDatabase.cs
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Inventory/Item Database", fileName = "ItemDatabase")]
public class ItemDatabase : ScriptableObject
{
    public List<ItemSO> allItems;
}
