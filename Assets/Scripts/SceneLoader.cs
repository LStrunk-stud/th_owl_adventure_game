using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    public static SceneLoader Instance;

    void Awake()
    {
        Instance = this;
    }

    public void LoadRoom(string sceneName, string spawnName)
    {
        StartCoroutine(LoadRoutine(sceneName, spawnName));
    }

    IEnumerator LoadRoutine(string sceneName, string spawnName)
    {
        PlayerMovement.Instance.canMove = false;

        yield return SceneManager.LoadSceneAsync(sceneName);

        // Spawn setzen
        var spawns = FindObjectsByType<SpawnPoint>(FindObjectsSortMode.None);

        foreach (var sp in spawns)
        {
            if (sp.spawnName == spawnName)
            {
                PlayerMovement.Instance.transform.position = sp.transform.position;
                break;
            }
        }

        PlayerMovement.Instance.canMove = true;
    }
}