using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;

public class ClickController : MonoBehaviour
{
    [SerializeField] private Camera worldCamera;
    [SerializeField] private LayerMask hotspotLayer;

    void Start()
    {
        if (!worldCamera)
            worldCamera = Camera.main;
    }

    void Update()
    {
        if (Mouse.current == null) return;
        if (!Mouse.current.leftButton.wasPressedThisFrame) return;

        // Right-click deselects held item
        if (Mouse.current.rightButton.wasPressedThisFrame)
        {
            ItemSelectionState.Instance.Deselect();
            return;
        }

        // UI clicks are handled by Unity's EventSystem — don't raycast into the world
        if (EventSystem.current != null && EventSystem.current.IsPointerOverGameObject()) return;
        if (!PlayerMovement.Instance.canMove) return;

        Vector3 world = GetMouseWorld();
        var hit = Physics2D.Raycast(world, Vector2.zero, 0f, hotspotLayer);

        if (hit.collider != null)
        {
            // ── Item held: try to use or pick up ────────────────────────────
            if (ItemSelectionState.Instance.HasSelection)
            {
                var use = hit.collider.GetComponent<UseHotspot>();
                if (use != null)
                {
                    use.TryUse(ItemSelectionState.Instance.SelectedItem);
                    return;
                }
                // Clicked on something else while holding an item — do nothing
                return;
            }

            // ── No item held: check for pickup or transition ─────────────────
            var pickup = hit.collider.GetComponent<PickupHotspot>();
            if (pickup != null)
            {
                pickup.Pickup();
                return;
            }

            var transition = hit.collider.GetComponent<TransitionHotspot>();
            if (transition != null)
            {
                transition.Activate();
                return;
            }
        }

        // ── Nothing hit: move player (only when no item is held) ────────────
        if (!ItemSelectionState.Instance.HasSelection)
            PlayerMovement.Instance.MoveTo(world);
    }

    Vector3 GetMouseWorld()
    {
        float depth = Mathf.Abs(worldCamera.transform.position.z - PlayerMovement.Instance.transform.position.z);
        Vector2 mouse = Mouse.current.position.ReadValue();
        Vector3 world = worldCamera.ScreenToWorldPoint(new Vector3(mouse.x, mouse.y, depth));
        world.z = PlayerMovement.Instance.transform.position.z;
        return world;
    }
}