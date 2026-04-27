using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Central UI controller for popup management and scene navigation.
/// </summary>
public class UIManager : MonoBehaviour
{
    public static UIManager Instance;
    public GameObject popupBackground;

    [Header("Popups")]
    [SerializeField] private GameObject themePopup;
    [SerializeField] private GameObject statsPopup;
    [SerializeField] private GameObject settingsPopup;
    [SerializeField] private GameObject exitPopup;
    [SerializeField] private GameObject gameOverPopup;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    public void OnPlayPressed() => OpenPopup(themePopup);

    public void OnStartGame()
    {
        CloseAllPopups();
        SceneManager.LoadScene("GameScene");
    }

    public void OnStatsPressed() => OpenPopup(statsPopup);

    public void OnSettingsPressed() => OpenPopup(settingsPopup);

    public void OnExitPressed() => OpenPopup(exitPopup);

    public void ConfirmExit()
    {
        Application.Quit();
    }

    public void ShowGameOver(CellState result, float duration)
    {
        if (!EnsureAssigned(gameOverPopup, nameof(gameOverPopup)))
            return;

        var gameOverUI = gameOverPopup.GetComponent<GameOverUI>();
        if (gameOverUI == null)
        {
            Debug.LogError("GameOverUI component missing on GameOverPopup!");
            return;
        }

        gameOverUI.Setup(result, duration);
        gameOverPopup.SetActive(true);
    }

    public void OnRetry()
    {
        SceneManager.LoadScene("GameScene");
    }

    public void OnExitToMenu()
    {
        SceneManager.LoadScene("PlayScene");
    }

    public void OpenPopup(GameObject popup)
    {
        // Shows a popup and enables the optional overlay background.
        if (!EnsureAssigned(popup, "Popup"))
            return;

        if (popupBackground != null)
            popupBackground.SetActive(true);

        popup.SetActive(true);
        PlaySFX("popup");
    }

    public void ClosePopup(GameObject popup)
    {
        // Hides a popup and disables the overlay if it is currently active.
        if (popupBackground != null && popupBackground.activeSelf)
            popupBackground.SetActive(false);

        if (popup == null)
            return;

        popup.SetActive(false);
    }

    public void CloseAllPopups()
    {
        PlaySFX("popup");
        ClosePopup(themePopup);
        ClosePopup(statsPopup);
        ClosePopup(settingsPopup);
        ClosePopup(exitPopup);
        ClosePopup(gameOverPopup);
    }

    private bool EnsureAssigned(Object obj, string name)
    {
        if (obj == null)
        {
            Debug.LogWarning($"{name} is not assigned in UIManager!");
            return false;
        }

        return true;
    }

    private void PlaySFX(string type)
    {
        if (AudioManager.Instance != null)
            AudioManager.Instance.PlaySFX(type);
    }
}