using UnityEngine;
using TMPro;

public class StatsUI : MonoBehaviour
{
    [Header("Text Fields")]
    public TextMeshProUGUI totalGamesText;
    public TextMeshProUGUI p1WinsText;
    public TextMeshProUGUI p2WinsText;
    public TextMeshProUGUI drawsText;
    public TextMeshProUGUI avgTimeText;

    void OnEnable()
    {
        Refresh();
    }

    public void Refresh()
    {
        var stats = StatsManager.Instance;

        totalGamesText.text = stats.TotalGames.ToString();
        p1WinsText.text = stats.Player1Wins.ToString();
        p2WinsText.text = stats.Player2Wins.ToString();
        drawsText.text = stats.Draws.ToString();
        avgTimeText.text = FormatTime(stats.AverageTime);
    }

    string FormatTime(float time)
    {
        int minutes = Mathf.FloorToInt(time / 60f);
        int seconds = Mathf.FloorToInt(time % 60f);

        return $"{minutes:00}:{seconds:00}";
    }
}