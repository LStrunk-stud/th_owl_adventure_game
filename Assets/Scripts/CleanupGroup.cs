using UnityEngine;
using UnityEngine.Events;

/// Tracks a group of CleanupItems.
/// Fires OnAllCleaned when every item in the group is cleaned up.
public class CleanupGroup : MonoBehaviour
{
    [SerializeField] private CleanupItem[] items;

    [Tooltip("Dialogue when all items are cleaned up.")]
    [SerializeField] private DialogueData allCleanedDialogue;

    public UnityEvent OnAllCleaned;

    private int _cleanedCount = 0;

    void Start()
    {
        // Auto-find items if not assigned
        if (items == null || items.Length == 0)
            items = GetComponentsInChildren<CleanupItem>();
    }

    public void ReportCleaned(CleanupItem item)
    {
        _cleanedCount++;

        if (_cleanedCount >= items.Length)
        {
            if (allCleanedDialogue != null)
                DialogueManager.Instance.PlaySimpleDialogue(allCleanedDialogue);

            OnAllCleaned?.Invoke();
        }
    }
}
