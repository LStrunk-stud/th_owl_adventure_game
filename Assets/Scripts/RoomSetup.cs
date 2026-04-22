using UnityEngine;

public class RoomSetup : MonoBehaviour
{
    [SerializeField] private PolygonCollider2D walkArea;

    [Tooltip("Default spawn point when entering this scene directly (editor/debug).")]
    [SerializeField] private SpawnPoint defaultSpawnPoint;

    void Start()
    {
        if (PlayerMovement.Instance == null) return;

        // Set walk area
        if (walkArea != null)
            PlayerMovement.Instance.SetWalkArea(walkArea);

        // If canMove is already true, scene was loaded normally — do nothing
        if (PlayerMovement.Instance.canMove) return;

        // canMove is false — scene was loaded via SceneLoader which handles
        // spawn + fade-in itself, so we don't interfere.
        // BUT if SceneTransition doesn't exist (direct play in editor),
        // we enable movement immediately so the scene is playable.
        if (SceneTransition.Instance == null)
        {
            PlayerMovement.Instance.canMove = true;

            // Also place player at default spawn if assigned
            if (defaultSpawnPoint != null)
                PlayerMovement.Instance.transform.position = defaultSpawnPoint.transform.position;
        }
    }
}