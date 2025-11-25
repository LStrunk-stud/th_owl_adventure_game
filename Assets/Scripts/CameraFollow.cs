using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;
    public Collider2D backgroundArea; // Polygon- oder BoxCollider
    public float smooth = 5f;

    private float minX;
    private float maxX;
    private float fixedY;
    private float fixedZ;

    void Start()
    {
        fixedY = transform.position.y;
        fixedZ = transform.position.z;

        // Kamera-Breite berechnen
        float halfHeight = Camera.main.orthographicSize;
        float halfWidth = halfHeight * Camera.main.aspect;

        // Hintergrund-Grenzen auslesen
        Bounds b = backgroundArea.bounds;

        minX = b.min.x + halfWidth;
        maxX = b.max.x - halfWidth;
    }

    void LateUpdate()
    {
        // Ziel X-Position
        float targetX = target.position.x;

        // Begrenzen
        float clampedX = Mathf.Clamp(targetX, minX, maxX);

        Vector3 newPos = new Vector3(clampedX, fixedY, fixedZ);

        transform.position = Vector3.Lerp(
            transform.position,
            newPos,
            smooth * Time.deltaTime
        );
    }
}