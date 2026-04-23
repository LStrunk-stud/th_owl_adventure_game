using UnityEngine;
using UnityEngine.Events;

/// Attach to an interior object that can be opened/searched.
/// Wire to InteractHotspot.OnInteract in the Inspector.
public class OpenableObject : MonoBehaviour
{
    [Header("Sprites")]
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private Sprite         closedSprite;
    [SerializeField] private Sprite         openSprite;

    [Header("State")]
    [SerializeField] private bool startOpen = false;

    [Header("Events")]
    public UnityEvent OnOpened;    // fires when opened for the first time
    public UnityEvent OnSearched;  // fires when opened again (already open)

    private bool _isOpen = false;

    void Start()
    {
        _isOpen = startOpen;
        UpdateSprite();
    }

    /// Call this from InteractHotspot.OnInteract
    public void Interact()
    {
        if (!_isOpen)
        {
            _isOpen = true;
            UpdateSprite();
            OnOpened?.Invoke();
        }
        else
        {
            OnSearched?.Invoke();
        }
    }

    private void UpdateSprite()
    {
        if (spriteRenderer == null) return;
        spriteRenderer.sprite = _isOpen ? openSprite : closedSprite;
    }
}