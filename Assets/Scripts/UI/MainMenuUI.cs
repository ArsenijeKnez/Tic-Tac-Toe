using UnityEngine;

/// <summary>
/// Handles main menu actions such as theme selection, navigation, and button events.
/// </summary>
public class MainMenuUI : MonoBehaviour
{
    public static MainMenuUI Instance;

    public NotificationPopup notificationPopup;
    public StatsUI statsUI;
    public ThemeButton[] themeButtons;

    private ThemeButton currentSelected;

    void Awake()
    {
        Instance = this;
    }

    public void SelectTheme(ThemeButton button)
    {
        if (currentSelected != null)
            currentSelected.SetSelected(false);

        currentSelected = button;
        currentSelected.SetSelected(true);

        GameSettings.Instance.SetTheme(currentSelected.themeIndex);
    }

    public void Play() => UIManager.Instance.OnPlayPressed();

    public void StartGame()
    {
        // Prevents starting the game before the player selects a theme.
        if (currentSelected == null)
        {
            notificationPopup.Show("Select a theme before starting.");
            return;
        }

        UIManager.Instance.OnStartGame();
    }

    public void Stats()
    {
        statsUI.Refresh();
        UIManager.Instance.OnStatsPressed();
    }

    public void Settings() => UIManager.Instance.OnSettingsPressed();

    public void Exit() => UIManager.Instance.OnExitPressed();

    public void ConfirmExit() => UIManager.Instance.ConfirmExit();
}