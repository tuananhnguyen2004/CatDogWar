using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Grid : MonoBehaviour
{
    [Header("Debug")]
    public List<Image> images;
    [SerializeField] private bool isDebug;
    [SerializeField] private Image debugImage;
    [SerializeField] private RectTransform rootCanvas;

    [Header("Grid Settings")]
    [SerializeField] private int gridColumns;
    public int GridColumns => gridColumns;
    [SerializeField] private int gridRows;
    public int GridRows => gridRows;
    [SerializeField] private float cellSize;
    public float CellSize => cellSize;
    [SerializeField] private GridCell[,] cells;
    public GridCell[,] Cells { get => cells; set { } }

    public Dictionary<DraggableItem, int> status = new(); // Keep track of remaining grid contained DraggableItem

    private float leftPadding;
    private float rightPadding;
    private float topPadding;
    private float bottomPadding;
    private Vector2 spacing;

    [Header("Grid Cell Color")]
    [SerializeField] private Color highlightColor;
    [SerializeField] private Color defaultColor;
    [SerializeField] private Color invalidColor;
    private FlexibleGridLayout flexibleGridLayout;

    /// <summary>
    /// Convert world position to grid index
    /// </summary>
    /// <param name="worldPosition"></param>
    /// <returns></returns>
    public Vector2Int GetGridCellIndex(Vector3 worldPosition)
    {
        int c = Mathf.FloorToInt((worldPosition.x - leftPadding) / (cellSize + spacing.x));
        int r = Mathf.FloorToInt((worldPosition.y - topPadding) / (cellSize + spacing.y));
        //Debug.Log("Grid Index: " + r + " " + c);
        return new Vector2Int(r, c);
    }

    /// <summary>
    /// Get grid cell by grid index
    /// </summary>
    /// <param name="gridIndex">grid index represented by [row, column] pair</param>
    /// <returns></returns>
    public GridCell GetGridCellByIndex(Vector2Int gridIndex)
    {
        return cells[gridIndex.x, gridIndex.y];
    }

    /// <summary>
    /// Get grid cell by world position
    /// </summary>
    /// <param name="worldPosition"></param>
    /// <returns></returns>
    public GridCell GetGridCellByWorldPosition(Vector2 worldPosition)
    {
        Vector2Int gridIndex = GetGridCellIndex(worldPosition);
        return cells[gridIndex.x, gridIndex.y];
    }

    /// <summary>
    /// Check if the grid index is valid
    /// </summary>
    /// <param name="gridIndex"></param>
    /// <returns></returns>
    public bool IsValidGridIndex(Vector2Int gridIndex)
    {
        return gridIndex.x >= 0 && gridIndex.x < gridRows && gridIndex.y >= 0 && gridIndex.y < gridColumns;
    }

    public bool IsWithinGrid(Vector2 worldPosition)
    {
        RectTransformUtility.ScreenPointToLocalPointInRectangle(GameManager.Instance.CurrentGrid.GetComponent<RectTransform>(),
                Input.mousePosition, null, out Vector2 localPoint);
        Vector2Int gridIndex = GetGridCellIndex(localPoint);
        return IsValidGridIndex(gridIndex);
    }

    /// <summary>
    /// Highlight grid visual
    /// </summary>
    /// <param name="gridCell"></param>
    public void HighlightCell(GridCell gridCell)
    {
        if (gridCell.assignedItem != null)
        {
            gridCell.visual.color = invalidColor;
        }
        else
        {
            gridCell.visual.color = highlightColor;
        }
    }

    /// <summary>
    /// Unhighlight grid visual
    /// </summary>
    /// <param name="gridCell"></param>
    public void UnhighlightCell(GridCell gridCell)
    {
        gridCell.visual.color = defaultColor;
    }

    /// <summary>
    /// Highlight grid visual
    /// </summary>
    public void ClearHighlight()
    {
        for (int r = 0; r < gridRows; r++)
        {
            for (int c = 0; c < gridColumns; c++)
            {
                UnhighlightCell(cells[r, c]);
            }
        }
    }

    public void ClearItem(DraggableItem item)
    {
        for (int r = 0; r < gridRows; r++)
        {
            for (int c = 0; c < gridColumns; c++)
            {
                if (cells[r, c].assignedItem == item)
                {
                    cells[r, c].assignedItem = null;
                }
            }
        }   
    }

    public void CalculateGridBounds(DraggableItem item, int width, out Vector2 minPos, out Vector2 maxPos)
    {
        minPos = Vector2.zero;
        maxPos = Vector2.zero;

        switch (item.CurrentDirection)
        {
            case Direction.Right:
            case Direction.Left:
                minPos = GetGridCellByIndex(new Vector2Int(0, item.FirstPartLength)).Position;
                maxPos = GetGridCellByIndex(new Vector2Int(gridRows - 1, gridColumns - item.SecondPartLength)).Position;
                break;
            case Direction.Down:
            case Direction.Up:
                minPos = GetGridCellByIndex(new Vector2Int(item.FirstPartLength, 0)).Position;
                maxPos = GetGridCellByIndex(new Vector2Int(gridRows - item.SecondPartLength, gridColumns - 1)).Position;
                break;
        }
    }

    public List<GridCell> GetAffectedGridCells(DraggableItem item)
    {
        List<GridCell> affectedCells = new List<GridCell>();
        Vector2Int gridIndex = item.GetItemCellIndex();
        for (int i = -item.FirstPartLength; i < item.SecondPartLength; ++i)
        {
            Vector2Int cellIndex = default;
            switch (item.CurrentDirection)
            {
                case Direction.Left:
                case Direction.Right:
                    cellIndex = new Vector2Int(gridIndex.x, gridIndex.y + i);
                    break;
                case Direction.Up:
                case Direction.Down:
                    cellIndex = new Vector2Int(gridIndex.x + i, gridIndex.y);
                    break;
            }

            if (!IsValidGridIndex(cellIndex)) continue;
            affectedCells.Add(GetGridCellByIndex(cellIndex));
        }
        return affectedCells;
    }

    public void TakeTurn()
    {
        // Handle UI animations when taking turn
    }

    public void GetAttacked(DraggableItem item)
    {
        Debug.Log("Get attacked " + gameObject.name);
        if (status.ContainsKey(item))
        {
            status[item]--;
            Debug.Log(item + " " + status[item]);
            if (status[item] <= 0)
            {
                Debug.Log(item + " is destroyed");
                item.gameObject.SetActive(true);
                item.isInteractable = false;
                item.Image.color = item.InvalidColor;
                status.Remove(item);
            }

            if (status.Count == 0)
            {
                Debug.Log("Game Over! Player " + GameManager.Instance.CurrentGrid + " Win!!");
            }
        }
    }

    private void Awake()
    {
        flexibleGridLayout = GetComponent<FlexibleGridLayout>();
        // Initialize grid's data
        gridColumns = flexibleGridLayout.columns;
        gridRows = flexibleGridLayout.rows;
        cellSize = flexibleGridLayout.cellSize.x;
        leftPadding = flexibleGridLayout.padding.left;
        rightPadding = flexibleGridLayout.padding.right;
        topPadding = flexibleGridLayout.padding.top;
        bottomPadding = flexibleGridLayout.padding.bottom;
        spacing = flexibleGridLayout.spacing;
    }

    private void Start()
    {
        

        Debug.Log(flexibleGridLayout.cellSize.x);


        int index = 0;
        cells = new GridCell[gridRows, gridColumns];
        for (int r = 0; r < gridRows; r++)
        {
            for (int c = 0; c < gridColumns; c++)
            {
                cells[r, c] = new GridCell(images[index], this);
                ++index;
                //Debug.Log(r + " " + c);
            }
        }
    }
}
