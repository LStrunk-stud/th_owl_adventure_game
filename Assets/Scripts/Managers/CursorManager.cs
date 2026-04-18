using UnityEngine;

public class CursorManager : MonoBehaviour
{
    public Texture2D normalCursor;
    public Texture2D clickCursor;

    // Set this in the Inspector — fallback icon when an item has no sprite
    [SerializeField] private Texture2D fallbackItemCursor;

    void Start()
    {
        SetCursor(normalCursor);

        ItemSelectionState.Instance.OnItemSelected   += HandleItemSelected;
        ItemSelectionState.Instance.OnItemDeselected += HandleItemDeselected;
    }

    void OnDestroy()
    {
        if (ItemSelectionState.Instance == null) return;
        ItemSelectionState.Instance.OnItemSelected   -= HandleItemSelected;
        ItemSelectionState.Instance.OnItemDeselected -= HandleItemDeselected;
    }

    void Update()
    {
        // Only swap normal ↔ click when no item is held
        if (ItemSelectionState.Instance.HasSelection) return;

        if (Input.GetMouseButtonDown(0)) SetCursor(clickCursor);
        if (Input.GetMouseButtonUp(0))   SetCursor(normalCursor);
    }

    // ── Item snap ─────────────────────────────────────────────────────────────

    private void HandleItemSelected(ItemData item)
    {
        if (item.icon != null)
            SetCursorFromSprite(item.icon);
        else
            SetCursor(fallbackItemCursor != null ? fallbackItemCursor : normalCursor);
    }

    private void HandleItemDeselected()
    {
        SetCursor(normalCursor);
    }

    // ── Helpers ───────────────────────────────────────────────────────────────

    void SetCursor(Texture2D texture)
    {
        if (texture == null) return;
        Vector2 hotspot = new Vector2(texture.width / 2f, texture.height / 2f);
        Cursor.SetCursor(texture, hotspot, CursorMode.Auto);
    }

    void SetCursorFromSprite(Sprite sprite)
    {
        // Extract the Texture2D from the sprite's texture
        // Works for sprites that use the full texture (most icons do)
        Texture2D tex = sprite.texture;
        if (!tex.isReadable)
        {
            Debug.LogWarning($"[CursorManager] Texture '{tex.name}' is not Read/Write enabled. Falling back to default cursor.");
            SetCursor(fallbackItemCursor != null ? fallbackItemCursor : normalCursor);
            return;
        }

        Vector2 hotspot = new Vector2(tex.width / 2f, tex.height / 2f);
        Cursor.SetCursor(tex, hotspot, CursorMode.Auto);
    }
}