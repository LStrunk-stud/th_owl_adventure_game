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

        InventoryManager.Instance.AddItem(item);

        // Remove the object from the world after pickup
        Destroy(gameObject);
    }
}
