using System;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Instance { get; private set; }

    public event Action OnInventoryChanged;
    public event Action OnBackpackUnlocked;
    public event Action OnInventoryReset;   // NEW: UI listens to fully reset itself

    private readonly List<ItemData> _items = new();
    private bool _backpackUnlocked = false;

    void Awake()
    {
        if (Instance != null && Instance != this) { Destroy(gameObject); return; }
        Instance = this;
    }

    void Start()
    {
        if (GameManager.Instance.PendingReset)
        {
            GameManager.Instance.ClearPendingReset();
            return;
        }
        RestoreFromSave();
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
            GameManager.Instance.MarkBackpackUnlocked();
            GameManager.Instance.MarkItemCollected(item.itemID);
            OnBackpackUnlocked?.Invoke();
            return;
        }

        if (!_backpackUnlocked)
        {
            Debug.LogWarning($"[InventoryManager] Cannot pick up '{item.itemName}' — backpack not unlocked.");
            return;
        }

        _items.Add(item);
        GameManager.Instance.MarkItemCollected(item.itemID);
        OnInventoryChanged?.Invoke();
    }

    public void RemoveItem(ItemData item)
    {
        if (_items.Remove(item))
            OnInventoryChanged?.Invoke();
    }

    public bool HasItem(ItemData item) => _items.Contains(item);

    /// Wipes runtime state and notifies UI to fully reset.
    public void ResetInventory()
    {
        _items.Clear();
        _backpackUnlocked = false;
        OnInventoryReset?.Invoke();   // UI uses this to hide everything
    }

    // ── Save / Restore ────────────────────────────────────────────────────────

    private void RestoreFromSave()
    {
        if (GameManager.Instance.IsBackpackUnlocked())
        {
            _backpackUnlocked = true;
            OnBackpackUnlocked?.Invoke();
        }

        var allItems = Resources.LoadAll<ItemData>("Items");
        foreach (var item in allItems)
        {
            if (item.isBackpack) continue;
            if (GameManager.Instance.IsItemCollected(item.itemID))
                _items.Add(item);
        }

        if (_items.Count > 0)
            OnInventoryChanged?.Invoke();
    }
}