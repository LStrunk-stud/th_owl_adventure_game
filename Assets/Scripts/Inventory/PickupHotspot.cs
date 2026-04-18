using UnityEngine;

/// Attach to any world object the player can pick up.
/// Assign the matching ItemData asset in the Inspector.
public class PickupHotspot : MonoBehaviour
{
    [SerializeField] private ItemData item;

    public void Pickup()
    {
        if (item == null)
        {
            Debug.LogWarning($"[PickupHotspot] No ItemData assigned on '{gameObject.name}'.");
            return;
        }

        // Don't pick up regular items if backpack isn't unlocked yet
        if (!item.isBackpack && !InventoryManager.Instance.BackpackUnlocked)
        {
            Debug.Log($"[PickupHotspot] Can't pick up '{item.itemName}' — backpack not unlocked.");
            return;
        }

        InventoryManager.Instance.AddItem(item);
        Destroy(gameObject);
    }
}