using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DialogueUI : MonoBehaviour
{
    public static DialogueUI Instance;

    public Transform optionsContainer;
    public GameObject optionButtonTemplate;

    private DialogueData currentDialogue;
    private int index = 0;
    private Transform currentNPC;

    void Awake()
    {
        Instance = this;
        gameObject.SetActive(false);

        optionButtonTemplate.SetActive(false);
    }

    public void StartDialogue(DialogueData data, Transform npc)
    {
        currentDialogue = data;
        currentNPC = npc;
        index = 0;

        PlayerMovement.Instance.enabled = false;

        gameObject.SetActive(true);
        ShowLine();
    }

    void ShowLine()
    {
        if (currentDialogue == null ||
            currentDialogue.lines == null ||
            index < 0 ||
            index >= currentDialogue.lines.Length)
        {
            EndDialogue();
            return;
        }

        var line = currentDialogue.lines[index];

        if (line.speakerName == "NPC")
            FloatingTextSpawner.Instance.ShowFloatingText(currentNPC, line.text);
        else
            FloatingTextSpawner.Instance.ShowFloatingText(GameObject.FindWithTag("Player").transform, line.text);

        foreach (Transform child in optionsContainer)
        {
            var btn = child.GetComponent<Button>();
            if (btn != null)
                btn.onClick.RemoveAllListeners();

            Destroy(child.gameObject);
        }

        if (line.options == null || line.options.Length == 0)
        {
            index++;
            ShowLine();
            return;
        }

        if (line.options.Length == 1)
        {
            index++;
            ShowLine();
            return;
        }

        for (int i = 0; i < line.options.Length; i++)
        {
            string option = line.options[i];

            var btnObj = Instantiate(optionButtonTemplate, optionsContainer);
            btnObj.SetActive(true);

            TMP_Text tmp = btnObj.GetComponentInChildren<TMP_Text>();
            tmp.text = option;

            LayoutRebuilder.ForceRebuildLayoutImmediate(btnObj.GetComponent<RectTransform>());

            int captured = i;
            btnObj.GetComponent<Button>().onClick.AddListener(() => SelectOption(captured));
        }

        LayoutRebuilder.ForceRebuildLayoutImmediate(optionsContainer.GetComponent<RectTransform>());
    }

    void SelectOption(int optionIndex)
    {
        index++;
        ShowLine();
    }

    void EndDialogue()
    {
        PlayerMovement.Instance.enabled = true;
        gameObject.SetActive(false);
    }
}