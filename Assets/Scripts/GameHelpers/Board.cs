using UnityEngine;
using System.Collections.Generic;

public struct WinResult
{
    public bool isWin;
    public List<Vector2Int> winningCells;
}

public class Board : MonoBehaviour
{
    public int size = 3;
    [SerializeField] private float boardSize = 5f;
    public GameObject cellPrefab;
    public GameObject[] xPrefab;
    public GameObject[] oPrefab;
    public GameObject winLinePrefab;

    private CellState[,] grid;
    private Cell[,] cells;

    public bool IsFinished { get; private set; }
    public CellState Winner { get; private set; }

    private System.Action<Board, int, int> onMoveCallback;
    private int theme = 0;
    private float cellWorldSize;

    public void Init(System.Action<Board, int, int> callback, int theme)
    {
        onMoveCallback = callback;
        this.theme = theme;

        grid = new CellState[size, size];
        cells = new Cell[size, size];
        IsFinished = false;
        Winner = CellState.Empty;

        Generate();
    }

    void Generate()
    {
        float cellSize = boardSize / size;

        // Calculates the bottom-left world position for the board.
        Vector2 bottomLeft = (Vector2)transform.position - Vector2.one * boardSize / 2f;

        for (int x = 0; x < size; x++)
        {
            for (int y = 0; y < size; y++)
            {
                Vector2 cellPosition = bottomLeft + new Vector2(
                    x * cellSize + cellSize / 2f,
                    y * cellSize + cellSize / 2f
                );

                GameObject cellObject = Instantiate(cellPrefab, cellPosition, Quaternion.identity, transform);
                ScaleToCell(cellObject, cellSize);

                cellWorldSize = cellObject.GetComponent<SpriteRenderer>().bounds.size.x;

                Cell cell = cellObject.GetComponent<Cell>();
                cell.Init(x, y, this);

                cells[x, y] = cell;
                grid[x, y] = CellState.Empty;
            }
        }
    }

    void ScaleToCell(GameObject obj, float cellSize)
    {
        SpriteRenderer sr = obj.GetComponent<SpriteRenderer>();
        if (sr == null)
            return;

        Vector2 spriteSize = sr.sprite.bounds.size;
        float scale = Mathf.Min(
            cellSize / spriteSize.x,
            cellSize / spriteSize.y
        );

        obj.transform.localScale = Vector3.one * scale;
    }

    float GetCellWorldSize(GameObject cellObj)
    {
        SpriteRenderer sr = cellObj.GetComponent<SpriteRenderer>();
        if (sr == null)
            return 1f;

        // Bounds represent the rendered size in world units.
        return sr.bounds.size.x;
    }

    void ScaleMarkToCell(GameObject obj, float targetSize)
    {
        SpriteRenderer sr = obj.GetComponent<SpriteRenderer>();
        if (sr == null)
            return;

        float spriteSize = sr.bounds.size.x;
        float scaleFactor = targetSize / spriteSize;

        obj.transform.localScale = Vector3.one * scaleFactor;
    }

    public void SetBoardToScreen(float screenRatio)
    {
        Camera cam = Camera.main;
        float worldHeight = 2f * cam.orthographicSize;
        float worldWidth = worldHeight * cam.aspect;

        boardSize = Mathf.Min(worldWidth, worldHeight) * screenRatio;
    }

    public void SetBoardSize(float size)
    {
        boardSize = size;
    }

    public void OnCellClicked(Cell cell)
    {
        if (IsFinished)
            return;

        if (grid[cell.x, cell.y] != CellState.Empty)
            return;

        CellState player = GameManager.Instance.CurrentPlayer;
        grid[cell.x, cell.y] = player;

        GameObject prefab = player == CellState.X ? xPrefab[theme] : oPrefab[theme];
        GameObject mark = Instantiate(prefab, cell.transform.position, Quaternion.identity, cell.transform);

        // Triggers the placement animation on the mark.
        MarkAnimation markAnim = mark.GetComponent<MarkAnimation>();
        if (markAnim != null)
        {
            markAnim.PlayPlacementAnimation(cellWorldSize);
        }
        else
        {
            //fallback if no animation component is present.
            ScaleMarkToCell(mark, cellWorldSize);
        }

        if (AudioManager.Instance != null)
            AudioManager.Instance.PlaySFX("place");

        var winResult = CheckWin(player);
        if (winResult.isWin)
        {
            IsFinished = true;
            Winner = player;
            DrawWinLine(winResult.winningCells);
            GameManager.Instance.GameOver(Winner);
        }
        else if (IsDraw())
        {
            IsFinished = true;
            Winner = CellState.Empty;
            GameManager.Instance.GameOver(Winner);
        }

        onMoveCallback?.Invoke(this, cell.x, cell.y);
    }

    WinResult CheckWin(CellState player)
    {
        WinResult result = new WinResult { isWin = false, winningCells = new List<Vector2Int>() };

        // Check rows
        for (int y = 0; y < size; y++)
        {
            bool win = true;
            List<Vector2Int> cells = new List<Vector2Int>();
            for (int x = 0; x < size; x++)
            {
                cells.Add(new Vector2Int(x, y));
                if (grid[x, y] != player)
                {
                    win = false;
                    break;
                }
            }

            if (win)
            {
                result.isWin = true;
                result.winningCells = cells;
                return result;
            }
        }

        // Check columns
        for (int x = 0; x < size; x++)
        {
            bool win = true;
            List<Vector2Int> cells = new List<Vector2Int>();
            for (int y = 0; y < size; y++)
            {
                cells.Add(new Vector2Int(x, y));
                if (grid[x, y] != player)
                {
                    win = false;
                    break;
                }
            }

            if (win)
            {
                result.isWin = true;
                result.winningCells = cells;
                return result;
            }
        }

        // Check diagonals
        bool diagonalLeft = true;
        List<Vector2Int> leftCells = new List<Vector2Int>();
        bool diagonalRight = true;
        List<Vector2Int> rightCells = new List<Vector2Int>();

        for (int i = 0; i < size; i++)
        {
            leftCells.Add(new Vector2Int(i, i));
            rightCells.Add(new Vector2Int(i, size - 1 - i));

            if (grid[i, i] != player) diagonalLeft = false;
            if (grid[i, size - 1 - i] != player) diagonalRight = false;
        }

        if (diagonalLeft)
        {
            result.isWin = true;
            result.winningCells = leftCells;
            return result;
        }

        if (diagonalRight)
        {
            result.isWin = true;
            result.winningCells = rightCells;
            return result;
        }

        return result;
    }

    void DrawWinLine(List<Vector2Int> winningCells)
    {
        if (winningCells.Count != 3 || winLinePrefab == null)
            return;

        // Gets the world positions of the first and last winning cells.
        Vector3 startPos = cells[winningCells[0].x, winningCells[0].y].transform.position;
        Vector3 endPos = cells[winningCells[2].x, winningCells[2].y].transform.position;

        // Calculates the center, length, and angle.
        Vector3 center = (startPos + endPos) / 2f;
        float length = Vector3.Distance(startPos, endPos) + 0.6f; // Adds a small margin to ensure the line covers the marks.
        Vector3 direction = endPos - startPos;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        // Instantiates and positions the win line.
        GameObject winLine = Instantiate(winLinePrefab, center, Quaternion.Euler(0, 0, angle), transform);

        // Scales the line to match the length.
        SpriteRenderer sr = winLine.GetComponent<SpriteRenderer>();
        if (sr != null)
        {
            float spriteWidth = sr.sprite.bounds.size.x;
            float scaleX = length / spriteWidth;
            winLine.transform.localScale = new Vector3(scaleX, 1f, 1f);
        }
    }

    bool IsDraw()
    {
        foreach (var cellState in grid)
        {
            if (cellState == CellState.Empty)
                return false;
        }

        return true;
    }
}