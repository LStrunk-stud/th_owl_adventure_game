using UnityEngine;

/// A selectable player response option.
/// Optionally links to a follow-up DialogueData asset.
[System.Serializable]
public class DialogueOption
{
    [Tooltip("The text shown on the option button.")]
    public string optionText;

    [Tooltip("The dialogue that plays when this option is chosen. Leave empty to end dialogue.")]
    public DialogueData nextDialogue;
}
