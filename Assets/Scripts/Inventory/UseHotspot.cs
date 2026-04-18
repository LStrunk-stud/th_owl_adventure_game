using UnityEngine;
using UnityEngine.Events;

/// Attach to any world object that reacts to a specific item being used on it.
/// Assign the required ItemData and wire up OnUsed in the Inspector.
public class UseHotspot : MonoBehaviour
{
    [SerializeField] private ItemData requiredItem;
    [SerializeField] private bool consumeItem = true;   // remove item after use?

    [Space]
    public UnityEvent OnUsed;       // fires when the correct item is used
    public UnityEvent OnWrongItem;  // optional: fires when wrong item is used

    /// Called by ClickController when the player clicks this object with an item.
    public void TryUse(ItemData usedItem)
    {
        if (usedItem == null) return;

        if (usedItem == requiredItem)
        {
            if (consumeItem)
                InventoryManager.Instance.RemoveItem(usedItem);

            ItemSelectionState.Instance.Deselect();
            OnUsed?.Invoke();
        }
        else
        {
            OnWrongItem?.Invoke();
        }
    }
}
