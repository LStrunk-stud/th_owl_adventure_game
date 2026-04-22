using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    public static SceneLoader Instance;

    [SerializeField] private string     gameplayPrefix = "Room";
    [SerializeField] private GameObject gameplayCanvas;

    void Awake() => Instance = this;

    void OnEnable()  => SceneManager.sceneLoaded += OnSceneLoaded;
    void OnDisable() => SceneManager.sceneLoaded -= OnSceneLoaded;

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        bool isGameplay = scene.name.StartsWith(gameplayPrefix);

        if (gameplayCanvas != null)
            gameplayCanvas.SetActive(isGameplay);

        if (PlayerMovement.Instance != null)
        {
            PlayerMovement.Instance.gameObject.SetActive(isGameplay);

            // Only block movement here — LoadRoutine re-enables it after fade-in.
            // canMove stays true if scene was loaded directly (not via LoadRoom).
            if (isGameplay && _loadingViaRoutine)
                PlayerMovement.Instance.canMove = false;
        }
    }

    // Flag so OnSceneLoaded knows if load came from LoadRoom or direct start
    private bool _loadingViaRoutine = false;

    public void LoadRoom(string sceneName, string spawnName)
    {
        GameManager.Instance.SaveLastRoom(sceneName, spawnName);
        StartCoroutine(LoadRoutine(sceneName, spawnName));
    }

    private IEnumerator LoadRoutine(string sceneName, string spawnName)
    {
        _loadingViaRoutine = true;

        yield return SceneManager.LoadSceneAsync(sceneName);

        _loadingViaRoutine = false;

        // Place player at spawn
        var spawns = FindObjectsByType<SpawnPoint>(FindObjectsSortMode.None);
        foreach (var sp in spawns)
        {
            if (sp.spawnName == spawnName)
            {
                PlayerMovement.Instance.transform.position = sp.transform.position;
                PlayerMovement.Instance.StopMoving();
                break;
            }
        }

        // Fade in then enable movement
        yield return StartCoroutine(SceneTransition.Instance.FadeIn());

        if (PlayerMovement.Instance != null)
            PlayerMovement.Instance.canMove = true;

        // Reset transition hotspots
        foreach (var h in FindObjectsByType<TransitionHotspot>(FindObjectsSortMode.None))
            h.ResetTrigger();
    }
}