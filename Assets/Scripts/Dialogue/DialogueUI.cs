using UnityEngine;
using UnityEngine.UI;
using TMPro;

/// Controls the dialogue panel in the GameplayCanvas.
/// Follows the speaker in world space and displays lines + options.
public class DialogueUI : MonoBehaviour
{
    public static DialogueUI Instance { get; private set; }

    [Header("Panel")]
    [SerializeField] private GameObject     dialoguePanel;
    [SerializeField] private TextMeshProUGUI speakerNameText;
    [SerializeField] private TextMeshProUGUI dialogueText;

    [Header("Options")]
    [SerializeField] private GameObject     optionsContainer;
    [SerializeField] private GameObject     optionButtonPrefab;

    [Header("Follow Settings")]
    [SerializeField] private Vector3        worldOffset = new Vector3(0f, 1.5f, 0f);
    [SerializeField] private Camera         worldCamera;

    private Transform _currentSpeaker;
    private bool      _showingOptions;

    void Awake()
    {
        if (Instance != null && Instance != this) { Destroy(gameObject); return; }
        Instance = this;
    }

    void Start()
    {
        if (!worldCamera) worldCamera = Camera.main;
        Hide();
    }

    void Update()
    {
        // Follow speaker in world space
        if (dialoguePanel.activeSelf && _currentSpeaker != null)
            PositionAboveSpeaker(_currentSpeaker);

        // Advance on click — only when showing lines, not options
        if (!_showingOptions && DialogueManager.Instance.IsPlaying)
        {
            if (Input.GetMouseButtonDown(0) &&
                !UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject())
            {
                DialogueManager.Instance.Advance();
            }
        }
    }

    // ── Public ────────────────────────────────────────────────────────────────

    public void ShowLine(DialogueLine line, Transform speaker)
    {
        _currentSpeaker = speaker;
        _showingOptions = false;

        ClearOptions();
        optionsContainer.SetActive(false);
        dialoguePanel.SetActive(true);

        speakerNameText.text  = line.speakerName;
        speakerNameText.color = line.speakerColor;
        dialogueText.text     = line.text;
        dialogueText.color    = line.speakerColor;

        PositionAboveSpeaker(speaker);
    }

    public void ShowOptions(DialogueOption[] options, Transform speaker)
    {
        _currentSpeaker = speaker;
        _showingOptions = true;

        ClearOptions();
        optionsContainer.SetActive(true);

        // Hide the main text area while options are shown
        dialogueText.text = "";
        speakerNameText.text = "";

        foreach (var option in options)
        {
            var btnGO  = Instantiate(optionButtonPrefab, optionsContainer.transform);
            var label  = btnGO.GetComponentInChildren<TextMeshProUGUI>();
            var button = btnGO.GetComponent<Button>();

            if (label)  label.text = option.optionText;

            // Capture option in closure
            var captured = option;
            button.onClick.AddListener(() => DialogueManager.Instance.ChooseOption(captured));
        }

        PositionAboveSpeaker(speaker);
    }

    public void Hide()
    {
        dialoguePanel.SetActive(false);
        ClearOptions();
        _currentSpeaker = null;
        _showingOptions = false;
    }

    // ── Helpers ───────────────────────────────────────────────────────────────

    private void PositionAboveSpeaker(Transform speaker)
    {
        if (worldCamera == null || speaker == null) return;

        Vector3 worldPos   = speaker.position + worldOffset;
        Vector3 screenPos  = worldCamera.WorldToScreenPoint(worldPos);

        dialoguePanel.transform.position = screenPos;
    }

    private void ClearOptions()
    {
        foreach (Transform child in optionsContainer.transform)
            Destroy(child.gameObject);
    }
}
