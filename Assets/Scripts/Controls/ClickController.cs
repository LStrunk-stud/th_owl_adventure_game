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
        if (EventSystem.current != null && EventSystem.current.IsPointerOverGameObject()) return;
        if (!PlayerMovement.Instance.canMove) return;

        Vector3 world = GetMouseWorld();

        var hit = Physics2D.Raycast(world, Vector2.zero, 0f, hotspotLayer);

        if (hit.collider != null)
        {
            var hotspot = hit.collider.GetComponent<TransitionHotspot>();
            if (hotspot != null)
            {
                hotspot.Activate();
                return;
            }
        }

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