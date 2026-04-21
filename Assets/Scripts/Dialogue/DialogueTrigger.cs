using UnityEngine;

/// Attach to any NPC.
/// Flow:
///   First talk  -> greetingDialogue (intro lines) -> mainOptionsDialogue (loop)
///   Later talks -> repeatingDialogue (short lines) -> mainOptionsDialogue (loop)
///   Each chosen option returns here automatically after its lines finish.
public class DialogueTrigger : MonoBehaviour
{
    [Header("Dialogues")]
    [Tooltip("Plays once on first contact. Should end with no options — Manager jumps to Main Options.")]
    [SerializeField] private DialogueData greetingDialogue;

    [Tooltip("The option menu the player always returns to after each choice.")]
    [SerializeField] private DialogueData mainOptionsDialogue;

    [Tooltip("Short lines on repeat contact, then jumps to Main Options.")]
    [SerializeField] private DialogueData repeatingDialogue;

    [Header("Anchor")]
    [SerializeField] private Transform speakerAnchor;

    private bool _hasSpoken = false;

    public DialogueData MainOptionsDialogue => mainOptionsDialogue;

    public void StartDialogue()
    {
        if (DialogueManager.Instance.IsPlaying) return;

        Transform anchor = speakerAnchor != null ? speakerAnchor : transform;

        if (!_hasSpoken)
        {
            _hasSpoken = true;

            if (greetingDialogue != null)
            {
                DialogueManager.Instance.PlayDialogue(greetingDialogue, anchor, this);
                return;
            }
        }
        else if (repeatingDialogue != null)
        {
            DialogueManager.Instance.PlayDialogue(repeatingDialogue, anchor, this);
            return;
        }

        // Fallback: go straight to main options
        if (mainOptionsDialogue != null)
            DialogueManager.Instance.PlayDialogue(mainOptionsDialogue, anchor, this);
    }
}