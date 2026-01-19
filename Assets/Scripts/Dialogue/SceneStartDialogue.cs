using UnityEngine;

public class SceneStartDialogue : MonoBehaviour
{
    void Start()
    {
        DialogueUI.Instance.StartDialogue(new string[]
        {
            "Verdammt… ich bin schon wieder zu spät.",
            "Der Assistent hätte mich früher wecken sollen.",
            "Erstmal zusammenreißen."
        });
    }
}