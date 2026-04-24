using System.Collections;
using UnityEngine;

/// <summary>
/// Attach to a child GameObject (e.g. "FadeWall") that has a SpriteRenderer.
/// The GameObject also needs a Collider2D set to "Is Trigger".
/// When the player enters the trigger area the sprite fades out,
/// when the player leaves it fades back in.
/// </summary>
[RequireComponent(typeof(Collider2D))]
[RequireComponent(typeof(SpriteRenderer))]
public class FadeWall : MonoBehaviour
{
    [Tooltip("Alpha value when the wall is fully visible.")]
    [Range(0f, 1f)]
    public float visibleAlpha = 1f;

    [Tooltip("Alpha value when the player is behind the wall.")]
    [Range(0f, 1f)]
    public float hiddenAlpha = 0f;

    [Tooltip("How many seconds the fade takes.")]
    public float fadeDuration = 0.25f;

    private SpriteRenderer sr;
    private Coroutine fadeCoroutine;

    void Awake()
    {
        sr = GetComponent<SpriteRenderer>();

        // Make sure the collider is a trigger
        GetComponent<Collider2D>().isTrigger = true;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;
        Fade(hiddenAlpha);
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;
        Fade(visibleAlpha);
    }

    private void Fade(float targetAlpha)
    {
        if (fadeCoroutine != null) StopCoroutine(fadeCoroutine);
        fadeCoroutine = StartCoroutine(FadeRoutine(targetAlpha));
    }

    private IEnumerator FadeRoutine(float targetAlpha)
    {
        float startAlpha = sr.color.a;
        float elapsed = 0f;

        while (elapsed < fadeDuration)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / fadeDuration);
            Color c = sr.color;
            c.a = Mathf.Lerp(startAlpha, targetAlpha, t);
            sr.color = c;
            yield return null;
        }

        Color final = sr.color;
        final.a = targetAlpha;
        sr.color = final;
    }
}
