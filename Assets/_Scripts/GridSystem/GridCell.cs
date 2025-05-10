using System.Threading.Tasks;
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
    public bool IsAttacked => isAttacked;

    public GridCell(Image visual, Grid grid)
    {
        this.grid = grid;
        this.visual = visual;
    }

    public bool GetAttacked()
    {
        if (isAttacked) return false;
        bool isHit;

        isAttacked = true;
        visual.color = Color.white;
        if (assignedItem != null)
        {
            visual.sprite = grid.HitSprite;
            grid.GetAttacked(assignedItem);
            Debug.Log("This grid cell has assigned item: " + assignedItem);
            isHit = true;
        }
        else
        {
            visual.color = GameManager.Instance.missedColor;
            visual.sprite = grid.MissedSprite;
            isHit = false;
            //await Task.Delay(1000);
            //GameManager.Instance.SwitchTurn();
        }
        return isHit;
    }
}