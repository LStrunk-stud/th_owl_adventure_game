using UnityEngine;

/// A self-contained dialogue asset. Assign to a DialogueTrigger on any NPC.
/// Create via: Right-click → Dialogue → Dialogue Data
[CreateAssetMenu(fileName = "NewDialogue", menuName = "Dialogue/Dialogue Data")]
public class DialogueData : ScriptableObject
{
    [Tooltip("All lines spoken in order before options appear.")]
    public DialogueLine[] lines;

    [Tooltip("Options shown after all lines. Leave empty for a linear dialogue with no choices.")]
    public DialogueOption[] options;
}
