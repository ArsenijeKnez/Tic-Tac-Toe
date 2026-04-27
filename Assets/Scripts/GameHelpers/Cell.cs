using UnityEngine;
using UnityEngine.EventSystems;

public class Cell : MonoBehaviour
{
    public int x;
    public int y;

    private Board parentBoard;

    public void Init(int x, int y, Board board)
    {
        this.x = x;
        this.y = y;
        parentBoard = board;
    }

    void OnMouseDown()
    {
        if (EventSystem.current != null && EventSystem.current.IsPointerOverGameObject())
            return;

        parentBoard.OnCellClicked(this);
    }
}