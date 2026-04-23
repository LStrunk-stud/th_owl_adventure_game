using UnityEngine;
using UnityEngine.Events;

public class OpenableObject : MonoBehaviour
{
    [Header("Sprites")]
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private Sprite         closedSprite;
    [SerializeField] private Sprite         openSprite;

    [Header("Loot")]
    [SerializeField] private PickupHotspot lootPickup;

    [Header("Dialogue")]
    [Tooltip("Plays when opened.")]
    [SerializeField] private DialogueData openDialogue;

    [Tooltip("Plays after open dialogue when loot is picked up.")]
    [SerializeField] private DialogueData lootDialogue;

    [Tooltip("Plays when searched again after looting.")]
    [SerializeField] private DialogueData searchedDialogue;

    [Header("State")]
    [SerializeField] private bool startOpen = false;

    public UnityEvent OnOpened;
    public UnityEvent OnSearched;

    private bool _isOpen   = false;
    private bool _isLooted = false;

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

            // Give loot immediately
            if (lootPickup != null && !_isLooted)
            {
                _isLooted = true;
                lootPickup.Pickup();
            }

            // Open dialogue first, loot dialogue queues after
            if (openDialogue != null)
                DialogueManager.Instance.PlaySimpleDialogue(openDialogue);

            if (lootDialogue != null)
                DialogueManager.Instance.QueueDialogue(lootDialogue);

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