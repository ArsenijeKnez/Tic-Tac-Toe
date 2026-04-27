using UnityEngine;
using UnityEngine.UI;
using TMPro;

/// <summary>
/// Handles in-game UI updates such as timer, move counters, and end-game flow.
/// </summary>
public class GameUI : MonoBehaviour
{
    public TextMeshProUGUI timerText;
    public TextMeshProUGUI player1MovesText;
    public TextMeshProUGUI player2MovesText;

    private float timer;
    private bool running = true;

    void Update()
    {
        if (!running)
            return;

        timer += Time.deltaTime;
        timerText.text = timer.ToString("F1") + "s";
    }

    public void StopTimer()
    {
        running = false;
    }

    public void MovePlayed(int p1Moves, int p2Moves, CellState currentPlayer)
    {
        // This updates the move counters.
        if (currentPlayer == CellState.X)
        {
            player1MovesText.text = $"X Moves: {p1Moves}";
        }
        else
        {
            player2MovesText.text = $"O Moves: {p2Moves}";
        }
    }

    public void GameEnded(CellState result)
    {
        // This stops the game timer and persists game statistics.
        StopTimer();

        bool p1 = result == CellState.X;
        bool p2 = result == CellState.O;

        StatsManager.Instance.SaveGame(p1, p2, timer);
        UIManager.Instance.ShowGameOver(result, timer);
    }

    public void OpenSettings()
    {
        UIManager.Instance.OnSettingsPressed();
    }
}