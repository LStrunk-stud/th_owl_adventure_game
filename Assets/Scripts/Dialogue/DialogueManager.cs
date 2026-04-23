using System;
using UnityEngine;

public class DialogueManager : MonoBehaviour
{
    public static DialogueManager Instance { get; private set; }

    public event Action OnDialogueStarted;
    public event Action OnDialogueEnded;

    public bool IsPlaying { get; private set; }

    [Header("End Option")]
    [Tooltip("One is picked at random each conversation.")]
    [SerializeField] private string[] endOptionVariants = new string[]
    {
        "Tschüss.",
        "Bis später.",
        "Ich muss weiter.",
        "Das war's erstmal.",
        "Wir reden später."
    };

    private DialogueData    _currentData;
    private int             _lineIndex;
    private Transform       _speakerTransform;
    private DialogueTrigger _currentTrigger;
    private string          _currentEndLabel;

    void Awake()
    {
        if (Instance != null && Instance != this) { Destroy(gameObject); return; }
        Instance = this;
    }

    // ── Public API ────────────────────────────────────────────────────────────

    public void PlayDialogue(DialogueData data, Transform speakerTransform, DialogueTrigger trigger = null)
    {
        if (data == null) { Debug.LogError("[DialogueManager] PlayDialogue: data is null!"); return; }

        _currentData      = data;
        _lineIndex        = 0;
        _speakerTransform = speakerTransform;
        _currentTrigger   = trigger;
        IsPlaying         = true;

        // Pick a fresh end label at the start of each new top-level conversation
        if (data == trigger?.MainOptionsDialogue || _currentEndLabel == null)
            _currentEndLabel = endOptionVariants[UnityEngine.Random.Range(0, endOptionVariants.Length)];

        PlayerMovement.Instance.canMove = false;
        OnDialogueStarted?.Invoke();

        if (data.lines == null || data.lines.Length == 0)
        {
            ShowOptionsOrEnd();
            return;
        }

        DialogueUI.Instance.ShowLine(data.lines[0], _speakerTransform);
    }

    public void Advance()
    {
        if (!IsPlaying) return;

        _lineIndex++;

        if (_lineIndex < _currentData.lines.Length)
        {
            DialogueUI.Instance.ShowLine(_currentData.lines[_lineIndex], _speakerTransform);
            return;
        }

        ShowOptionsOrEnd();
    }

    public void ChooseOption(DialogueOption option)
    {
        if (option.isEndOption)
        {
            EndDialogue();
            return;
        }

        if (option.nextDialogue != null)
            PlayDialogue(option.nextDialogue, _speakerTransform, _currentTrigger);
        else
            ReturnToMainOptions();
    }

    /// Plays a dialogue anchored to the player — for item pickups and object inspections.
    /// No trigger needed, no main options loop.
    public void PlaySimpleDialogue(DialogueData data)
    {
        if (data == null) return;
        if (PlayerMovement.Instance == null) return;
        PlayDialogue(data, PlayerMovement.Instance.transform, null);
    }

    public void EndDialogue()
    {
        IsPlaying = false;
        _currentEndLabel = null;
        PlayerMovement.Instance.canMove = true;
        DialogueUI.Instance.Hide();
        _currentTrigger = null;
        OnDialogueEnded?.Invoke();
    }

    // ── Helpers ───────────────────────────────────────────────────────────────

    private void ShowOptionsOrEnd()
    {
        bool isMainOptions = _currentTrigger != null
            && _currentData == _currentTrigger.MainOptionsDialogue;

        if (_currentData.options != null && _currentData.options.Length > 0)
        {
            // Only append end option on the main options menu
            DialogueOption[] opts = isMainOptions
                ? BuildOptionsWithEnd(_currentData.options)
                : _currentData.options;

            DialogueUI.Instance.ShowOptions(opts, _speakerTransform);
            return;
        }

        // No options — return to main options loop if possible
        if (_currentTrigger != null
            && _currentTrigger.MainOptionsDialogue != null
            && !isMainOptions)
        {
            ReturnToMainOptions();
            return;
        }

        EndDialogue();
    }

    private void ReturnToMainOptions()
    {
        if (_currentTrigger != null && _currentTrigger.MainOptionsDialogue != null)
            PlayDialogue(_currentTrigger.MainOptionsDialogue, _speakerTransform, _currentTrigger);
        else
            EndDialogue();
    }

    private DialogueOption[] BuildOptionsWithEnd(DialogueOption[] original)
    {
        // Don't add if designer already placed an end option manually
        foreach (var o in original)
            if (o.isEndOption) return original;

        var result = new DialogueOption[original.Length + 1];
        original.CopyTo(result, 0);
        result[result.Length - 1] = new DialogueOption
        {
            optionText  = _currentEndLabel,
            isEndOption = true
        };
        return result;
    }
}