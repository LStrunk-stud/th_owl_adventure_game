using UnityEngine;
using UnityEngine.UI;

public class InventoryUI : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GameObject inventoryPanel;
    [SerializeField] private Button     toggleButton;
    [SerializeField] private Transform  slotContainer;
    [SerializeField] private GameObject slotPrefab;

    private bool _panelVisible = false;
    private bool _unlocked     = false;

    void Start()
    {
        inventoryPanel.SetActive(false);
        toggleButton.gameObject.SetActive(false);
        toggleButton.onClick.AddListener(TogglePanel);

        InventoryManager.Instance.OnBackpackUnlocked += HandleBackpackUnlocked;
        InventoryManager.Instance.OnInventoryChanged  += RebuildSlots;

        // Restore state if backpack was already unlocked in a previous session
        // InventoryManager.Start() fires OnBackpackUnlocked before InventoryUI.Start()
        // only if execution order guarantees it — so we check directly as fallback
        if (InventoryManager.Instance.BackpackUnlocked && !_unlocked)
        {
            _unlocked = true;
            toggleButton.gameObject.SetActive(true);
            // Don't auto-open panel on restore — just show the button
        }

        // Rebuild slots for any items already in the inventory from save
        RebuildSlots();
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
        SetPanelVisible(true);
    }

    private void RebuildSlots()
    {
        foreach (Transform child in slotContainer)
            Destroy(child.gameObject);

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