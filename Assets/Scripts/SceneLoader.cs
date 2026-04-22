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
            PlayerMovement.Instance.canMove = false; // held until fade-in completes
        }
    }

    public void LoadRoom(string sceneName, string spawnName)
    {
        GameManager.Instance.SaveLastRoom(sceneName, spawnName);
        StartCoroutine(LoadRoutine(sceneName, spawnName));
    }

    private IEnumerator LoadRoutine(string sceneName, string spawnName)
    {
        // Screen is already black from TransitionHotspot fade-out
        // (or instant black if called directly e.g. from GameManager.StartNewGame)
        SceneTransition.Instance.SetAlpha(1f);

        yield return SceneManager.LoadSceneAsync(sceneName);

        // Place player at spawn point
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

        // Fade back in, then allow movement
        yield return StartCoroutine(SceneTransition.Instance.FadeIn());

        if (PlayerMovement.Instance != null)
            PlayerMovement.Instance.canMove = true;

        // Reset transition hotspots in the new scene
        var hotspots = FindObjectsByType<TransitionHotspot>(FindObjectsSortMode.None);
        foreach (var h in hotspots)
            h.ResetTrigger();
    }
}