using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DialogueUI : MonoBehaviour
{
    public static DialogueUI Instance { get; private set; }

    [Header("Speech Panel (follows NPC)")]
    [SerializeField] private RectTransform   canvasRect;
    [SerializeField] private RectTransform   speechPanelRect;
    [SerializeField] private TextMeshProUGUI speakerNameText;
    [SerializeField] private TextMeshProUGUI dialogueText;

    [Header("Options Panel (fixed bottom)")]
    [SerializeField] private GameObject optionsPanel;
    [SerializeField] private GameObject optionButtonPrefab;
    [SerializeField] private Transform  optionsContainer;

    [Header("Follow Settings")]
    [SerializeField] private Vector2 screenOffset = new Vector2(0f, 60f);
    [SerializeField] private Camera  worldCamera;

    private Transform _currentSpeaker;
    private bool      _showingOptions;
    private bool      _inputBlocked;

    void Awake()
    {
        if (Instance != null && Instance != this) { Destroy(gameObject); return; }
        Instance = this;
    }

    void Start()
    {
        if (!worldCamera) worldCamera = Camera.main;
        if (canvasRect == null)
            canvasRect = GetComponentInParent<Canvas>().GetComponent<RectTransform>();
        Hide();
    }

    void Update()
    {
        if (speechPanelRect.gameObject.activeSelf && _currentSpeaker != null)
            PositionAboveSpeaker(_currentSpeaker);

        if (_inputBlocked)
        {
            _inputBlocked = false;
            return;
        }

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
        StopAllCoroutines();
        StartCoroutine(ShowLineRoutine(line, speaker));
    }

    private IEnumerator ShowLineRoutine(DialogueLine line, Transform speaker)
    {
        _currentSpeaker = speaker;
        _showingOptions = false;
        _inputBlocked   = true;

        // Hide options, keep speech panel hidden until text is set
        optionsPanel.SetActive(false);
        ClearOptions();
        speechPanelRect.gameObject.SetActive(false);

        yield return null;

        speakerNameText.text  = line.speakerName;
        speakerNameText.color = line.speakerColor;
        dialogueText.text     = line.text;
        dialogueText.color    = line.speakerColor;

        speechPanelRect.gameObject.SetActive(true);

        yield return null;
        PositionAboveSpeaker(speaker);
    }

    public void ShowOptions(DialogueOption[] options, Transform speaker)
    {
        StopAllCoroutines();
        StartCoroutine(ShowOptionsRoutine(options, speaker));
    }

    private IEnumerator ShowOptionsRoutine(DialogueOption[] options, Transform speaker)
    {
        _currentSpeaker = speaker;
        _showingOptions = true;
        _inputBlocked   = true;

        // Keep speech panel visible with last line — just show options below
        ClearOptions();
        optionsPanel.SetActive(false);

        yield return null;

        optionsPanel.SetActive(true);

        foreach (var option in options)
        {
            var btnGO  = Instantiate(optionButtonPrefab, optionsContainer);
            var label  = btnGO.GetComponentInChildren<TextMeshProUGUI>();
            var button = btnGO.GetComponent<Button>();
            if (label) label.text = option.optionText;
            var captured = option;
            button.onClick.AddListener(() =>
            {
                _inputBlocked = true;
                DialogueManager.Instance.ChooseOption(captured);
            });
        }
    }

    public void Hide()
    {
        StopAllCoroutines();
        speechPanelRect.gameObject.SetActive(false);
        optionsPanel.SetActive(false);
        speakerNameText.text = "";
        dialogueText.text    = "";
        ClearOptions();
        _currentSpeaker = null;
        _showingOptions = false;
        _inputBlocked   = false;
    }

    // ── Positioning ───────────────────────────────────────────────────────────

    private void PositionAboveSpeaker(Transform speaker)
    {
        if (worldCamera == null || speaker == null || canvasRect == null) return;

        Vector3 screenPos = worldCamera.WorldToScreenPoint(speaker.position);
        screenPos.x += screenOffset.x;
        screenPos.y += screenOffset.y;

        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            canvasRect,
            new Vector2(screenPos.x, screenPos.y),
            null,
            out Vector2 localPoint
        );

        speechPanelRect.anchoredPosition = localPoint;
    }

    private void ClearOptions()
    {
        foreach (Transform child in optionsContainer)
            Destroy(child.gameObject);
    }
}