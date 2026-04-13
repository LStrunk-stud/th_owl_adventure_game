using UnityEngine;

public class TransitionHotspot : MonoBehaviour
{
    [Header("Target")]
    public string targetScene;
    public string targetSpawnName; 

    [Header("Position")]
    public Transform standPoint; 

    private bool triggered = false;

    public void Activate()
    {
        if (triggered) return;

        bool canMove = PlayerMovement.Instance.MoveTo(standPoint.position);

        if (canMove)
        {
            triggered = true;
            StartCoroutine(WaitForArrival());
        }
    }

    private System.Collections.IEnumerator WaitForArrival()
    {
        while (PlayerMovement.Instance.IsMoving())
            yield return null;

        SceneLoader.Instance.LoadRoom(targetScene, targetSpawnName);
    }
}