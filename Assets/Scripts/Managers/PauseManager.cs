using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseManager : MonoBehaviour
{

    [Header("Panels")]
    [SerializeField] private GameObject pausePanel;
    [SerializeField] private GameObject pauseMenuContainer;
    [SerializeField] private GameObject settingsPanel;

    private bool isPaused;

    void Awake()
    {
        ResumeGame();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused) ResumeGame();
            else PauseGame();
        }
    }

    public void PauseGame()
    {
        isPaused = true;
        Time.timeScale = 0f;

        if (pausePanel) pausePanel.SetActive(true);
        if (pauseMenuContainer) pauseMenuContainer.SetActive(true);
        if (settingsPanel) settingsPanel.SetActive(false);
    }

    public void ResumeGame()
    {
        isPaused = false;
        Time.timeScale = 1f;

        if (pausePanel) pausePanel.SetActive(false);
        if (settingsPanel) settingsPanel.SetActive(false);
        if (pauseMenuContainer) pauseMenuContainer.SetActive(true);
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void GoToMainMenu()
    {
        Time.timeScale = 1f;
        isPaused = false;

        if (pausePanel) pausePanel.SetActive(false);
        if (pauseMenuContainer)pauseMenuContainer.SetActive(false);
        if (settingsPanel) settingsPanel.SetActive(false);

        SceneManager.LoadScene("MainMenu");
    }

    public void OpenSettings()
    {
        if (pauseMenuContainer) pauseMenuContainer.SetActive(false);
        if (settingsPanel) settingsPanel.SetActive(true);
    }

    public void CloseSettings()
    {
        if (settingsPanel) settingsPanel.SetActive(false);
        if (pauseMenuContainer) pauseMenuContainer.SetActive(true);
    }
}