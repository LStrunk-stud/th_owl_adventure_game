using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuController : MonoBehaviour
{
    [Header("Panels")]
    public GameObject mainMenuPanel;
    public GameObject settingsPanel;

    [Header("Buttons")]
    [SerializeField] private Button continueButton;

    [Header("Scene")]
    [SerializeField] private string firstSceneName = "Room_ApartmentBedroom";

    void Start()
    {
        // Show "Continue" only if a save exists
        if (continueButton != null)
            continueButton.gameObject.SetActive(HasSave());
    }

    // ── Button callbacks ──────────────────────────────────────────────────────

    public void StartNewGame()
    {
        PlayerPrefs.DeleteAll();
        PlayerPrefs.Save();
        SceneManager.LoadScene(firstSceneName);
    }

    public void ContinueGame()
    {
        if (HasSave())
            SceneManager.LoadScene(firstSceneName);
    }

    public void OpenSettings()
    {
        mainMenuPanel.SetActive(false);
        settingsPanel.SetActive(true);
    }

    public void CloseSettings()
    {
        settingsPanel.SetActive(false);
        mainMenuPanel.SetActive(true);
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    // ── Helpers ───────────────────────────────────────────────────────────────

    private bool HasSave()
        => PlayerPrefs.GetInt("has_save", 0) == 1;
}