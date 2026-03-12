using UnityEngine;

public class RoomSetup : MonoBehaviour
{
    [SerializeField] private PolygonCollider2D walkArea;

    void Awake()
    {
        if (PlayerMovement.Instance != null && walkArea != null)
        {
            PlayerMovement.Instance.SetWalkArea(walkArea);
        }
    }
}
