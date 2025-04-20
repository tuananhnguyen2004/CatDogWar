using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class GridCell
{
    [SerializeField] private Grid grid;
    public Image visual;
    public Vector2 Position => visual.transform.position;
    public DraggableItem assignedItem;
    private bool isAttacked;

    public GridCell(Image visual, Grid grid)
    {
        this.grid = grid;
        this.visual = visual;
    }

    public void GetAttacked()
    {
        if (isAttacked) return;

        isAttacked = true;
        if (assignedItem != null)
        {
            visual.color = GameManager.Instance.hitColor;
            grid.GetAttacked(assignedItem);
            Debug.Log("This grid cell has assigned item: " + assignedItem);
        }
        else
        {
            GameManager.Instance.SwitchTurn();
            visual.color = GameManager.Instance.missedColor;
        }
    }
}