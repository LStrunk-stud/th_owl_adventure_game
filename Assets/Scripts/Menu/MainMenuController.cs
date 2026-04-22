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
        if (continueButton != null)
            continueButton.gameObject.SetActive(GameManager.Instance.HasSaveData());
    }

    // ── Button callbacks ──────────────────────────────────────────────────────

    public void StartNewGame()
    {
        // Delegates to GameManager — sets PendingReset flag, resets inventory, loads scene
        GameManager.Instance.StartNewGame(firstSceneName);
    }

    public void ContinueGame()
    {
        if (GameManager.Instance.HasSaveData())
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
}