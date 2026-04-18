using UnityEngine;

/// One line of dialogue spoken by a character.
[System.Serializable]
public class DialogueLine
{
    [Tooltip("Name shown in the dialogue box. Use 'Player' for the protagonist.")]
    public string speakerName;

    [Tooltip("Text color for this speaker — white for player, yellow for NPCs etc.")]
    public Color speakerColor = Color.white;

    [TextArea(2, 6)]
    public string text;
}
