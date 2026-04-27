using UnityEngine;

public class GameSettings : MonoBehaviour
{
    public static GameSettings Instance;
    public int ThemeIndex { get; private set; }

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void SetTheme(int index)
    {
        ThemeIndex = index;
    }
}