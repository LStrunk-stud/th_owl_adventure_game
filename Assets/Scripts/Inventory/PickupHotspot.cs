using UnityEngine;

/// Attach to any world object the player can pick up.
/// Can also be used as embedded loot inside OpenableObject — set isEmbedded = true.
public class PickupHotspot : MonoBehaviour
{
    [SerializeField] private ItemData    item;

    [Tooltip("Optional: dialogue that plays over the player when this item is picked up.")]
    [SerializeField] private DialogueData pickupDialogue;

    [Tooltip("If true, this hotspot is controlled by a parent object (e.g. chest). " +
             "It won't destroy itself or check backpack on pickup.")]
    [SerializeField] private bool isEmbedded = false;

    void Start()
    {
        if (isEmbedded) return; // embedded loot is managed by parent
        if (item == null) return;
        if (GameManager.Instance.IsItemCollected(item.itemID))
            gameObject.SetActive(false);
    }

    public void Pickup()
    {
        if (item == null)
        {
            Debug.LogWarning($"[PickupHotspot] No ItemData assigned on '{gameObject.name}'.");
            return;
        }

        // Embedded loot skips backpack check — parent object controls when this fires
        if (!isEmbedded && !item.isBackpack && !InventoryManager.Instance.BackpackUnlocked)
        {
            Debug.Log($"[PickupHotspot] Can't pick up '{item.itemName}' — backpack not unlocked.");
            return;
        }

        InventoryManager.Instance.AddItem(item);

        if (pickupDialogue != null)
            DialogueManager.Instance.PlaySimpleDialogue(pickupDialogue);

        // Only destroy self when not embedded — embedded loot lives on the parent object
        if (!isEmbedded)
            Destroy(gameObject);
    }
}