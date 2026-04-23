using System.Collections;
using UnityEngine;

public class TransitionHotspot : MonoBehaviour
{
    public enum TransitionType
    {
        WalkThenFade,   // Player walks to standPoint, then fades — for indoor doors
        FadeOnly,       // Instant fade without walking — for teleports
    }

    [Header("Target")]
    public string targetScene;
    public string targetSpawnName;

    [Header("Transition")]
    public TransitionType transitionType = TransitionType.WalkThenFade;
    [Tooltip("Fade duration in seconds. 0 = use default from SceneTransition.")]
    public float fadeDuration = 0f;

    [Header("Walk Position")]
    [Tooltip("Where the player walks before the transition. Required for WalkThenFade.")]
    public Transform standPoint;

    private bool _triggered = false;

    public void Activate()
    {
        if (_triggered) return;
        if (string.IsNullOrEmpty(targetScene)) return;

        _triggered = true;
        StartCoroutine(RunTransition());
    }

    public void ResetTrigger() => _triggered = false;

    private IEnumerator RunTransition()
    {
        PlayerMovement.Instance.canMove = false;

        if (transitionType == TransitionType.WalkThenFade && standPoint != null)
            yield return StartCoroutine(WalkToStandPoint());

        yield return StartCoroutine(
            SceneTransition.Instance.FadeOut(fadeDuration > 0 ? fadeDuration : -1f)
        );

        SceneLoader.Instance.LoadRoom(targetScene, targetSpawnName);
    }

    private IEnumerator WalkToStandPoint()
    {
        PlayerMovement.Instance.ForceMoveTo(standPoint.position);

        while (PlayerMovement.Instance.IsMoving())
            yield return null;
    }
}