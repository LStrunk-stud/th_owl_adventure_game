using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    public static PlayerMovement Instance;

    [SerializeField] private Camera worldCamera;
    [SerializeField] private PolygonCollider2D walkArea;
    [SerializeField] private float speed = 3f;
    [SerializeField] private float stopDistance = 0.05f;

    public bool canMove = true;

    private PlayerControls controls;
    private Vector3 targetPos;
    private bool moving;

    void Awake()
    {
        Instance = this;
        controls = new PlayerControls();
        targetPos = transform.position;
    }

    void Start()
    {
        if (worldCamera == null) worldCamera = Camera.main;
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

    void OnClick(InputAction.CallbackContext ctx)
    {
        if (!canMove) return;
        if (worldCamera == null || walkArea == null) return;

        float depth = Mathf.Abs(worldCamera.transform.position.z - transform.position.z);

        Vector2 mouseScreen = Mouse.current.position.ReadValue();
        Vector3 mouseWorld = worldCamera.ScreenToWorldPoint(new Vector3(mouseScreen.x, mouseScreen.y, depth));
        mouseWorld.z = transform.position.z;

        if (!walkArea.OverlapPoint(mouseWorld)) return;

        targetPos = mouseWorld;
        moving = true;
    }

    void Update()
    {
        if (!canMove || !moving) return;

        transform.position = Vector3.MoveTowards(transform.position, targetPos, speed * Time.deltaTime);

        if (Vector3.Distance(transform.position, targetPos) <= stopDistance)
            moving = false;
    }
}
