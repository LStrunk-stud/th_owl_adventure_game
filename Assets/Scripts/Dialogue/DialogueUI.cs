using UnityEngine;
using TMPro;

public class DialogueUI : MonoBehaviour
{
    public static DialogueUI Instance;

    [Header("UI References")]
    public GameObject panel;        // DialoguePanel
    public GameObject dialogueBox;  // DialogueBox
    public TMP_Text dialogueText;   // DialogueText

    private string[] lines;
    private int index;

    void Awake()
    {
        Instance = this;
        panel.SetActive(false);
    }

    public void StartDialogue(string[] dialogueLines)
    {
        lines = dialogueLines;
        index = 0;

        PlayerMovement.Instance.enabled = false;

        panel.SetActive(true);
        ShowNextLine();
    }

    void Update()
    {
        if (!panel.activeSelf) return;

        if (Input.GetMouseButtonDown(0))
        {
            ShowNextLine();
        }
    }

    void ShowNextLine()
    {
        if (index >= lines.Length)
        {
            EndDialogue();
            return;
        }

        dialogueText.text = lines[index];
        index++;
    }

    void EndDialogue()
    {
        panel.SetActive(false);
        PlayerMovement.Instance.enabled = true;
    }
}