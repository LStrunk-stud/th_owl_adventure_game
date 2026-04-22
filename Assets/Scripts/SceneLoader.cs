using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    public static SceneLoader Instance;

    [SerializeField] private string gameplayPrefix = "Room";
    [SerializeField] private GameObject gameplayCanvas;

    void Awake()
    {
        Instance = this;
    }

    void OnEnable()  => SceneManager.sceneLoaded += OnSceneLoaded;
    void OnDisable() => SceneManager.sceneLoaded -= OnSceneLoaded;

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        bool isGameplayScene = scene.name.StartsWith(gameplayPrefix);

        if (gameplayCanvas != null)
            gameplayCanvas.SetActive(isGameplayScene);

        if (PlayerMovement.Instance != null)
        {
            PlayerMovement.Instance.gameObject.SetActive(isGameplayScene);
            PlayerMovement.Instance.canMove = isGameplayScene;
        }
    }

    public void LoadRoom(string sceneName, string spawnName)
    {
        // Save last room so Continue knows where to resume
        GameManager.Instance.SaveLastRoom(sceneName, spawnName);
        StartCoroutine(LoadRoutine(sceneName, spawnName));
    }

    IEnumerator LoadRoutine(string sceneName, string spawnName)
    {
        if (PlayerMovement.Instance != null)
            PlayerMovement.Instance.canMove = false;

        yield return SceneManager.LoadSceneAsync(sceneName);

        var spawns = FindObjectsByType<SpawnPoint>(FindObjectsSortMode.None);
        foreach (var sp in spawns)
        {
            if (sp.spawnName == spawnName)
            {
                PlayerMovement.Instance.transform.position = sp.transform.position;
                break;
            }
        }

        if (PlayerMovement.Instance != null)
            PlayerMovement.Instance.canMove = true;
    }
}