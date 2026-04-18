using UnityEngine;

/// Attach to any NPC. Holds the dialogue data and starts the conversation.
/// The speaker transform is this object's transform by default,
/// but can be overridden (e.g. to point at a speech bubble anchor).
public class DialogueTrigger : MonoBehaviour
{
    [SerializeField] private DialogueData dialogueData;

    [Tooltip("The point the dialogue box follows. Leave empty to use this transform.")]
    [SerializeField] private Transform speakerAnchor;

    public void StartDialogue()
    {
        if (dialogueData == null)
        {
            Debug.LogWarning($"[DialogueTrigger] No DialogueData assigned on '{gameObject.name}'.");
            return;
        }

        // Don't start a new dialogue while one is already playing
        if (DialogueManager.Instance.IsPlaying) return;

        Transform anchor = speakerAnchor != null ? speakerAnchor : transform;
        DialogueManager.Instance.PlayDialogue(dialogueData, anchor);
    }
}
