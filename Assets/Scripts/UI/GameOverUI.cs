using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameOverUI : MonoBehaviour
{
    public TextMeshProUGUI resultText;
    public TextMeshProUGUI durationText;

    public void Setup(CellState result, float duration)
    {
        resultText.text = result == CellState.X
            ? "Player 1 Wins!"
            : result == CellState.O
                ? "Player 2 Wins!"
                : "It's a Tie!";

        durationText.text = "Duration: " + duration.ToString("F1") + "s";
    }

    public void Exit()
    {
        UIManager.Instance.OnExitToMenu();
    }

    public void Retry()
    {
        UIManager.Instance.OnRetry();
    }
}
