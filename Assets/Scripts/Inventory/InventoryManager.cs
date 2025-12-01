using System.Collections.Generic;
using UnityEngine;
using System;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Instance { get; private set; }

    public int capacity = 8; 
    public Sprite emptySlotSprite;

    private List<ItemData> items;

    public event Action OnInventoryChanged;

    void Awake()
    {
        if (Instance != null && Instance != this) { Destroy(gameObject); return; }
        Instance = this;
        items = new List<ItemData>(capacity);
    }

    public bool AddItem(ItemData item)
    {
        if (items.Count >= capacity) return false;
        items.Add(item);
        OnInventoryChanged?.Invoke();
        return true;
    }

    public ItemData GetItemAt(int index)
    {
        if (index < 0 || index >= items.Count) return null;
        return items[index];
    }

    public int GetItemCount() => items.Count;

    public IReadOnlyList<ItemData> GetItems() => items.AsReadOnly();
}
