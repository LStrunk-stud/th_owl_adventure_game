using UnityEngine;
using UnityEngine.InputSystem;

public class ItemClickPickup : MonoBehaviour
{
    private PlayerControls controls;

    void Awake()
    {
        controls = new PlayerControls();
    }

    void OnEnable()
    {
        controls.Enable();
        controls.Player.Click.performed += OnClick;
    }

    void OnDisable()
    {
        controls.Player.Click.performed -= OnClick;
        controls.Disable();
    }

    private void OnClick(InputAction.CallbackContext ctx)
    {
        Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
        Vector2 mp2 = new Vector2(mouseWorldPos.x, mouseWorldPos.y);

        RaycastHit2D hit = Physics2D.Raycast(mp2, Vector2.zero);
        if (hit.collider != null)
        {
            ItemPickup ip = hit.collider.GetComponent<ItemPickup>();
            if (ip != null)
            {
                ip.Pickup();
            }
        }
    }
}
