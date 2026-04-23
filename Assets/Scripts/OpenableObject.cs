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

    [Header("Loot")]
    [Tooltip("Embedded PickupHotspot — set IsEmbedded = true on it.")]
    [SerializeField] private PickupHotspot lootPickup;

    [Header("Dialogue")]
    [Tooltip("Plays over player when opened and no loot inside (or after loot pickup dialogue).")]
    [SerializeField] private DialogueData openDialogue;

    [Tooltip("Plays over player when searched again (already open).")]
    [SerializeField] private DialogueData searchedDialogue;

    [Header("State")]
    [SerializeField] private bool startOpen = false;

    [Header("Extra Events")]
    public UnityEvent OnOpened;
    public UnityEvent OnSearched;

    private bool _isOpen = false;

    void Start()
    {
        _isOpen = startOpen;
        UpdateSprite();
    }

    public void Interact()
    {
        if (!_isOpen)
        {
            _isOpen = true;
            UpdateSprite();

            if (lootPickup != null)
            {
                // Loot pickup handles its own dialogue (pickupDialogue on PickupHotspot)
                // Only play openDialogue if no loot or loot has no dialogue
                lootPickup.Pickup();

                // Play open dialogue only if loot has no pickup dialogue
                if (openDialogue != null)
                    DialogueManager.Instance.PlaySimpleDialogue(openDialogue);
            }
            else if (openDialogue != null)
            {
                DialogueManager.Instance.PlaySimpleDialogue(openDialogue);
            }

            OnOpened?.Invoke();
        }
        else
        {
            if (searchedDialogue != null)
                DialogueManager.Instance.PlaySimpleDialogue(searchedDialogue);

            OnSearched?.Invoke();
        }
    }

    private void UpdateSprite()
    {
        if (spriteRenderer == null) return;
        spriteRenderer.sprite = _isOpen ? openSprite : closedSprite;
    }
}