using UnityEngine;

/// Attach to any world object the player can pick up.
/// Automatically hides itself if the item was already collected in a previous session.
public class PickupHotspot : MonoBehaviour
{
    [SerializeField] private ItemData item;

    void Start()
    {
        if (item == null) return;

        // Hide if already collected (scene reload or returning from another scene)
        if (GameStateManager.Instance.IsItemCollected(item.itemID))
        {
            gameObject.SetActive(false);
        }
    }

    public void Pickup()
    {
        if (item == null)
        {
            Debug.LogWarning($"[PickupHotspot] No ItemData assigned on '{gameObject.name}'.");
            return;
        }

        if (!item.isBackpack && !InventoryManager.Instance.BackpackUnlocked)
        {
            Debug.Log($"[PickupHotspot] Can't pick up '{item.itemName}' — backpack not unlocked.");
            return;
        }

        InventoryManager.Instance.AddItem(item);
        Destroy(gameObject);
    }
}