using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// Manages the game lifecycle, including player turns, board creation, and game over handling.
/// </summary>
public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public int Theme = 0;
    public Board boardPrefab;
    public GameUI GameUI;

    private Board mainBoard;
    private int player1Moves = 0;
    private int player2Moves = 0;
    private List<IObserver> playerChangeObservers = new List<IObserver>();

    public CellState CurrentPlayer { get; private set; } = CellState.X;

    private void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        // Loads the selected theme and ensures the UI reference is available.
        if (GameSettings.Instance != null)
        {
            Theme = GameSettings.Instance.ThemeIndex;
        }

        if (GameUI == null)
        {
            GameUI = GetComponent<GameUI>();
        }

        CreateGame();
    }

    void CreateGame()
    {
        // Instantiates the board and initializes it with the current theme.
        mainBoard = Instantiate(boardPrefab, Vector3.zero, Quaternion.identity);
        mainBoard.SetBoardToScreen(0.8f);
        mainBoard.Init(OnMovePlayed, Theme);
    }

    void OnMovePlayed(Board board, int x, int y)
    {
        // Counts the move for the active player and updates the UI.
        if (CurrentPlayer == CellState.X)
            player1Moves++;
        else
            player2Moves++;

        GameUI.MovePlayed(player1Moves, player2Moves, CurrentPlayer);
        SwitchPlayer();
    }

    void SwitchPlayer()
    {
        CurrentPlayer = CurrentPlayer == CellState.X ? CellState.O : CellState.X;
        NotifyPlayerChangeObservers();
    }

    public void RegisterPlayerChangeObserver(IObserver observer)
    {
        if (!playerChangeObservers.Contains(observer))
            playerChangeObservers.Add(observer);
    }

    public void UnregisterPlayerChangeObserver(IObserver observer)
    {
        playerChangeObservers.Remove(observer);
    }

    private void NotifyPlayerChangeObservers()
    {
        foreach (var observer in playerChangeObservers)
            observer.UpdateState();
    }

    public void GameOver(CellState winner)
    {
        if (winner != CellState.Empty && AudioManager.Instance != null)
        {
            AudioManager.Instance.PlaySFX("win");
        }

        GameUI.GameEnded(winner);
    }
}