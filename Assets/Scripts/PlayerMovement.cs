using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    public static PlayerMovement Instance;

    private PlayerControls controls;

    public float speed = 3f;
    private Vector3 targetPos;
    private bool moving = false;

    public PolygonCollider2D walkArea;

    void Awake()
    {
        Instance = this;
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

    void Start()
    {
        targetPos = transform.position;
    }

    private void OnClick(InputAction.CallbackContext ctx)
    {
        if (!enabled) return;
        
        Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
        mouseWorldPos.z = 0f;

        if (!walkArea.OverlapPoint(mouseWorldPos))
            return;

        targetPos = mouseWorldPos;
        moving = true;
    }

    void Update()
    {
        if (!moving) return;

        transform.position = Vector3.MoveTowards(
            transform.position,
            targetPos,
            speed * Time.deltaTime
        );

        if (Vector3.Distance(transform.position, targetPos) < 0.05f)
            moving = false;
    }
}