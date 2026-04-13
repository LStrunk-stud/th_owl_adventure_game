using UnityEngine;

public class CursorManager : MonoBehaviour
{
    public Texture2D normalCursor;
    public Texture2D clickCursor;

    void Start()
    {
        SetCursor(normalCursor);
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            SetCursor(clickCursor);
        }

        if (Input.GetMouseButtonUp(0))
        {
            SetCursor(normalCursor);
        }
    }

    void SetCursor(Texture2D texture)
    {
        if (texture == null) return;

        Vector2 hotspot = new Vector2(texture.width / 2f, texture.height / 2f);

        Cursor.SetCursor(texture, hotspot, CursorMode.Auto);
    }
}