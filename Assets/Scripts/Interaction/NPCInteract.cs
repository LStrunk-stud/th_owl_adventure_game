using UnityEngine;

public class NPCInteract : MonoBehaviour
{
    public DialogueData dialogue;

    public void Interact()
    {
        DialogueUI.Instance.StartDialogue(dialogue, this.transform);
    }

}
