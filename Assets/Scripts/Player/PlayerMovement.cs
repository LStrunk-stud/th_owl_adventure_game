using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMovement : MonoBehaviour
{
    public static PlayerMovement Instance;

    [SerializeField] private float speed = 3f;
    [SerializeField] private float stopDistance = 0.05f;

    private PolygonCollider2D walkArea;
    private Rigidbody2D rb;

    // Default true — SceneLoader sets it to false during transitions
    public bool canMove = true;

    private Vector3 targetPos;
    private bool moving;

    void Awake()
    {
        Instance = this;
        rb = GetComponent<Rigidbody2D>();
        targetPos = transform.position;
    }

    void FixedUpdate()
    {
        if (!moving) return;

        Vector2 newPos = Vector2.MoveTowards(
            rb.position,
            targetPos,
            speed * Time.fixedDeltaTime
        );

        rb.MovePosition(newPos);

        if (Vector2.Distance(rb.position, targetPos) <= stopDistance)
            moving = false;
    }

    public void SetWalkArea(PolygonCollider2D area)
    {
        walkArea = area;
    }

    /// Normal movement — respects walkArea bounds.
    public bool MoveTo(Vector3 worldPos)
    {
        if (!canMove) return false;
        if (!walkArea || !walkArea.OverlapPoint(worldPos)) return false;

        targetPos = new Vector3(worldPos.x, worldPos.y, transform.position.z);
        moving = true;
        return true;
    }

    /// Forced movement — ignores walkArea and canMove. Used by transitions.
    public void ForceMoveTo(Vector3 worldPos)
    {
        targetPos = new Vector3(worldPos.x, worldPos.y, transform.position.z);
        moving = true;
    }

    public bool IsMoving() => moving;

    public void StopMoving()
    {
        moving = false;
        targetPos = transform.position;
    }
}