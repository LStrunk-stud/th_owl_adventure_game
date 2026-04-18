using System;
using UnityEngine;

/// Central dialogue controller. Lives on PERSISTOBJECTS.
/// Call PlayDialogue() from any DialogueTrigger to start a conversation.
public class DialogueManager : MonoBehaviour
{
    public static DialogueManager Instance { get; private set; }

    // Other systems listen to these to react to dialogue state changes
    public event Action OnDialogueStarted;
    public event Action OnDialogueEnded;

    public bool IsPlaying { get; private set; }

    private DialogueData    _currentData;
    private int             _lineIndex;
    private Transform       _speakerTransform;

    void Awake()
    {
        if (Instance != null && Instance != this) { Destroy(gameObject); return; }
        Instance = this;
    }

    // ── Public API ────────────────────────────────────────────────────────────

    /// Start a dialogue. speakerTransform is the world-space position the box follows.
    public void PlayDialogue(DialogueData data, Transform speakerTransform)
    {
        if (data == null || data.lines.Length == 0) return;

        _currentData      = data;
        _lineIndex        = 0;
        _speakerTransform = speakerTransform;
        IsPlaying         = true;

        PlayerMovement.Instance.canMove = false;
        OnDialogueStarted?.Invoke();

        DialogueUI.Instance.ShowLine(_currentData.lines[_lineIndex], _speakerTransform);
    }

    /// Called by DialogueUI when the player clicks to advance.
    public void Advance()
    {
        if (!IsPlaying) return;

        _lineIndex++;

        // More lines to show
        if (_lineIndex < _currentData.lines.Length)
        {
            DialogueUI.Instance.ShowLine(_currentData.lines[_lineIndex], _speakerTransform);
            return;
        }

        // All lines done — show options if any
        if (_currentData.options != null && _currentData.options.Length > 0)
        {
            DialogueUI.Instance.ShowOptions(_currentData.options, _speakerTransform);
            return;
        }

        // No options — end dialogue
        EndDialogue();
    }

    /// Called by DialogueUI when a dialogue option is chosen.
    public void ChooseOption(DialogueOption option)
    {
        if (option.nextDialogue != null)
        {
            // Chain into the next dialogue
            PlayDialogue(option.nextDialogue, _speakerTransform);
        }
        else
        {
            EndDialogue();
        }
    }

    public void EndDialogue()
    {
        IsPlaying = false;
        PlayerMovement.Instance.canMove = true;
        DialogueUI.Instance.Hide();
        OnDialogueEnded?.Invoke();
    }
}
