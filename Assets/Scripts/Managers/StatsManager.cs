using UnityEngine;

public class StatsManager : MonoBehaviour
{
    public static StatsManager Instance;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public int TotalGames
    {
        get => PlayerPrefs.GetInt("TotalGames", 0);
        set => PlayerPrefs.SetInt("TotalGames", value);
    }

    public int Player1Wins
    {
        get => PlayerPrefs.GetInt("P1Wins", 0);
        set => PlayerPrefs.SetInt("P1Wins", value);
    }

    public int Player2Wins
    {
        get => PlayerPrefs.GetInt("P2Wins", 0);
        set => PlayerPrefs.SetInt("P2Wins", value);
    }

    public int Draws
    {
        get => PlayerPrefs.GetInt("Draws", 0);
        set => PlayerPrefs.SetInt("Draws", value);
    }

    public float TotalTime
    {
        get => PlayerPrefs.GetFloat("TotalTime", 0);
        set => PlayerPrefs.SetFloat("TotalTime", value);
    }

    public float AverageTime => TotalGames == 0 ? 0 : TotalTime / TotalGames;

    public void SaveGame(bool isP1Win, bool isP2Win, float duration)
    {
        TotalGames++;

        if (isP1Win)
        {
            Player1Wins++;
        }
        else if (isP2Win)
        {
            Player2Wins++;
        }
        else
        {
            Draws++;
        }

        TotalTime += duration;
        PlayerPrefs.Save();
    }
}