using UnityEngine;
using UnityEngine.UI;

/// Attached to the InventoryUI root in the GameplayCanvas.
/// Starts hidden, unlocks when the backpack is picked up,
/// and rebuilds the slot list whenever the inventory changes.
public class InventoryUI : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GameObject inventoryPanel;   // The foldable slot bar
    [SerializeField] private Button toggleButton;         // The backpack icon button
    [SerializeField] private Transform slotContainer;     // Layout group holding slots
    [SerializeField] private GameObject slotPrefab;       // InventorySlot prefab

    private bool _panelVisible = false;
    private bool _unlocked = false;

    void Start()
    {
        // Hide everything until backpack is picked up
        inventoryPanel.SetActive(false);
        toggleButton.gameObject.SetActive(false);

        toggleButton.onClick.AddListener(TogglePanel);

        InventoryManager.Instance.OnBackpackUnlocked += HandleBackpackUnlocked;
        InventoryManager.Instance.OnInventoryChanged  += RebuildSlots;
    }

    void OnDestroy()
    {
        if (InventoryManager.Instance == null) return;
        InventoryManager.Instance.OnBackpackUnlocked -= HandleBackpackUnlocked;
        InventoryManager.Instance.OnInventoryChanged  -= RebuildSlots;
    }

    // ── Event Handlers ────────────────────────────────────────────────────────

    private void HandleBackpackUnlocked()
    {
        _unlocked = true;
        toggleButton.gameObject.SetActive(true);
        // Open the panel immediately so the player notices it
        SetPanelVisible(true);
    }

    private void RebuildSlots()
    {
        // Clear existing slots
        foreach (Transform child in slotContainer)
            Destroy(child.gameObject);

        // Create one slot per item
        foreach (var item in InventoryManager.Instance.GetItems())
        {
            var slotGO = Instantiate(slotPrefab, slotContainer);
            slotGO.GetComponent<InventorySlot>().Setup(item);
        }
    }

    // ── Toggle ────────────────────────────────────────────────────────────────

    private void TogglePanel()
    {
        if (!_unlocked) return;
        SetPanelVisible(!_panelVisible);
    }

    private void SetPanelVisible(bool visible)
    {
        _panelVisible = visible;
        inventoryPanel.SetActive(visible);
    }
}