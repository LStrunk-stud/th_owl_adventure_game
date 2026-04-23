using UnityEngine;

public class PickupHotspot : MonoBehaviour
{
    [SerializeField] private ItemData     item;

    [Header("Dialogues")]
    [Tooltip("Plays over player when item is picked up successfully.")]
    [SerializeField] private DialogueData pickupDialogue;

    [Tooltip("Plays when player clicks item but has no backpack yet.")]
    [SerializeField] private DialogueData noBackpackDialogue;

    [Tooltip("Is controlled by a parent object (e.g. chest) — skips destroy and backpack check.")]
    [SerializeField] private bool isEmbedded = false;

    void Start()
    {
        if (isEmbedded) return;
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

        // Can't pick up without backpack
        if (!isEmbedded && !item.isBackpack && !InventoryManager.Instance.BackpackUnlocked)
        {
            // Use local dialogue if set, otherwise fall back to global one in GameManager
            var dialogue = noBackpackDialogue ?? GameManager.Instance.noBackpackDialogue;
            if (dialogue != null)
                DialogueManager.Instance.PlaySimpleDialogue(dialogue);
            else
                Debug.Log($"[PickupHotspot] Can't pick up '{item.itemName}' — backpack not unlocked.");
            return;
        }

        InventoryManager.Instance.AddItem(item);

        if (pickupDialogue != null)
            DialogueManager.Instance.PlaySimpleDialogue(pickupDialogue);

        if (!isEmbedded)
            Destroy(gameObject);
    }
}