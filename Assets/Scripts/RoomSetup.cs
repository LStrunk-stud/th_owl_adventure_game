using UnityEngine;

public class RoomSetup : MonoBehaviour
{
    [SerializeField] private PolygonCollider2D walkArea;
    [SerializeField] private SpawnPoint defaultSpawnPoint;

    void Awake()
    {
        if (PlayerMovement.Instance != null && walkArea != null)
            PlayerMovement.Instance.SetWalkArea(walkArea);
    }

    void Start()
    {
        if (PlayerMovement.Instance == null) return;

        // PERSISTOBJECTS not loaded = direct scene start in editor
        // Enable movement and place player at default spawn
        bool persistObjectsLoaded = SceneTransition.Instance != null;
        if (!persistObjectsLoaded)
        {
            PlayerMovement.Instance.canMove = true;

            if (defaultSpawnPoint != null)
                PlayerMovement.Instance.transform.position =
                    defaultSpawnPoint.transform.position;
        }
    }
}