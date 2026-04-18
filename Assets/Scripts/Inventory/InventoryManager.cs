using System;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Instance { get; private set; }

    // Fired whenever the item list changes — UI listens to this
    public event Action OnInventoryChanged;

    // Fired once when the backpack is picked up — UI listens to unlock itself
    public event Action OnBackpackUnlocked;

    private readonly List<ItemData> _items = new();
    private bool _backpackUnlocked = false;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    // ── Public API ────────────────────────────────────────────────────────────

    public bool BackpackUnlocked => _backpackUnlocked;

    /// Returns a read-only snapshot of the current items.
    public IReadOnlyList<ItemData> GetItems() => _items.AsReadOnly();

    /// Adds an item. If it is the backpack, unlocks the inventory instead.
    public void AddItem(ItemData item)
    {
        if (item == null) return;

        if (item.isBackpack)
        {
            if (_backpackUnlocked) return;
            _backpackUnlocked = true;
            OnBackpackUnlocked?.Invoke();
            return;
        }

        if (!_backpackUnlocked)
        {
            Debug.LogWarning($"[InventoryManager] Cannot pick up '{item.itemName}' — backpack not unlocked yet.");
            return;
        }

        _items.Add(item);
        OnInventoryChanged?.Invoke();
    }

    /// Removes an item (e.g. after using it).
    public void RemoveItem(ItemData item)
    {
        if (_items.Remove(item))
            OnInventoryChanged?.Invoke();
    }

    public bool HasItem(ItemData item) => _items.Contains(item);
}