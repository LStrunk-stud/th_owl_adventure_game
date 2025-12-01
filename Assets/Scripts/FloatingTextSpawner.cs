using UnityEngine;
using TMPro;

public class FloatingTextSpawner : MonoBehaviour
{
    public static FloatingTextSpawner Instance;
    public GameObject floatingTextPrefab;
    public Canvas canvas;

    private GameObject currentNPCText;
    private GameObject currentPlayerText;

    void Awake()
    {
        Instance = this;
    }

    public void ShowFloatingText(Transform target, string text)
    {
        if (target.CompareTag("NPC"))
        {
            if (currentNPCText != null)
                Destroy(currentNPCText);
        }
        else
        {
            if (currentPlayerText != null)
                Destroy(currentPlayerText);
        }

        var go = Instantiate(floatingTextPrefab, canvas.transform);
        go.SetActive(true);

        go.GetComponent<FloatingText>().target = target;
        go.GetComponentInChildren<TMP_Text>().text = text;

        if (target.CompareTag("NPC"))
            currentNPCText = go;
        else
            currentPlayerText = go;
    }
}