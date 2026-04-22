using System;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Instance { get; private set; }

    public event Action OnInventoryChanged;
    public event Action OnBackpackUnlocked;

    private readonly List<ItemData> _items = new();
    private bool _backpackUnlocked = false;

    void Awake()
    {
        if (Instance != null && Instance != this) { Destroy(gameObject); return; }
        Instance = this;
    }

    void Start()
    {
        // Restore backpack state from save on startup
        if (GameStateManager.Instance.IsBackpackUnlocked())
            UnlockBackpackSilently();
    }

    // ── Public API ────────────────────────────────────────────────────────────

    public bool BackpackUnlocked => _backpackUnlocked;
    public IReadOnlyList<ItemData> GetItems() => _items.AsReadOnly();

    public void AddItem(ItemData item)
    {
        if (item == null) return;

        if (item.isBackpack)
        {
            if (_backpackUnlocked) return;
            _backpackUnlocked = true;
            GameStateManager.Instance.MarkBackpackUnlocked();
            GameStateManager.Instance.MarkItemCollected(item.itemID);
            OnBackpackUnlocked?.Invoke();
            return;
        }

        if (!_backpackUnlocked)
        {
            Debug.LogWarning($"[InventoryManager] Cannot pick up '{item.itemName}' — backpack not unlocked.");
            return;
        }

        _items.Add(item);
        GameStateManager.Instance.MarkItemCollected(item.itemID);
        OnInventoryChanged?.Invoke();
    }

    public void RemoveItem(ItemData item)
    {
        if (_items.Remove(item))
            OnInventoryChanged?.Invoke();
    }

    public bool HasItem(ItemData item) => _items.Contains(item);

    /// Called by GameManager.StartNewGame() to wipe runtime state.
    public void ResetInventory()
    {
        _items.Clear();
        _backpackUnlocked = false;
        OnInventoryChanged?.Invoke();
    }

    // ── Private ───────────────────────────────────────────────────────────────

    /// Restores backpack state without firing OnBackpackUnlocked event
    /// (UI is not ready yet during Start).
    private void UnlockBackpackSilently()
    {
        _backpackUnlocked = true;
    }
}