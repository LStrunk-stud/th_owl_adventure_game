using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public static PlayerMovement Instance;

    [SerializeField] private float speed = 3f;
    [SerializeField] private float stopDistance = 0.05f;

    private PolygonCollider2D walkArea;

    public bool canMove = true;

    private Vector3 targetPos;
    private bool moving;

    void Awake()
    {
        Instance = this;
        targetPos = transform.position;
    }

    void Update()
    {
        if (!canMove || !moving) return;

        transform.position = Vector3.MoveTowards(
            transform.position,
            targetPos,
            speed * Time.deltaTime
        );

        if (Vector3.Distance(transform.position, targetPos) <= stopDistance)
            moving = false;
    }

    public void SetWalkArea(PolygonCollider2D area)
    {
        walkArea = area;
    }

    public bool MoveTo(Vector3 worldPos)
    {
        if (!walkArea || !walkArea.OverlapPoint(worldPos)) return false;

        targetPos = new Vector3(worldPos.x, worldPos.y, transform.position.z);
        moving = true;
        return true;
    }

    public bool IsMoving()
    {
        return moving;
    }
}