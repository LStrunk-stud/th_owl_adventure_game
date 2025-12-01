using UnityEngine;
using TMPro;

public class FloatingText : MonoBehaviour
{
    public Transform target;
    public Vector3 offset;

    private RectTransform rect;
    
    void Awake()
    {
        rect = GetComponent<RectTransform>();
    }

    void Update()
    {
        if (target == null)
        {
            Destroy(gameObject);
            return;
        }

        Vector2 screenPos = Camera.main.WorldToScreenPoint(target.position + offset);
        rect.position = screenPos;
    }
}
