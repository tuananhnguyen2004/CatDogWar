using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public enum Direction
{
    Right,
    Down,
    Left,
    Up
}
public class DraggableItem : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerClickHandler
{
    [SerializeField] private int width;
    public int Width => width;
    [SerializeField] private bool isWithinGrid;

    private Vector2 originalPos;
    [SerializeField] private Vector2 minGridPos;
    [SerializeField] private Vector2 maxGridPos;

    [SerializeField] private Direction currentDirection;
    public Direction CurrentDirection => currentDirection;

    [Header("Color Settings")]
    private Color defaultColor;
    public Color DefaultColor => defaultColor;
    [SerializeField] private Color invalidColor;
    public Color InvalidColor => invalidColor;

    private bool isDragging;
    private Image image;
    public Image Image => image;
    private RectTransform rectTransform;
    public bool isInteractable;

    public int FirstPartLength
    {
        get 
        {
            int length = 0;
            switch(currentDirection)
            {
                case Direction.Right:
                case Direction.Up:
                    length = Mathf.CeilToInt(width / 2f) - 1;
                    break;
                case Direction.Down:
                case Direction.Left:
                    length = width - Mathf.CeilToInt(width / 2f);
                    break;
            }
            return length;
        }
    }

    public int SecondPartLength => width - FirstPartLength;

    private void Awake()
    {
        image = GetComponent<Image>();
        rectTransform = GetComponent<RectTransform>();
    }

    private void Start()
    {
        currentDirection = Direction.Right;
        originalPos = transform.position;
        defaultColor = image.color;
        isInteractable = true;

        float flexibleWidth = GameManager.Instance.CurrentGrid.CellSize * width;
        float flexibleHeight = GameManager.Instance.CurrentGrid.CellSize * 0.9f;
        rectTransform.sizeDelta = new Vector2(flexibleWidth, flexibleHeight);
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (!isInteractable) return;

        AudioManager.Instance.PlaySoundFX("Drag");

        Debug.Log("Begin Drag");
        isDragging = true;

        GameManager.Instance.CurrentGrid.CalculateGridBounds(this, width, out Vector2 minPos, out Vector2 maxPos);
        GameManager.Instance.CurrentGrid.ClearItem(this);

        minGridPos = minPos;
        maxGridPos = maxPos;
        transform.SetAsLastSibling();
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (!isInteractable) return;

        // Constraint the draggable item within the grid
        if (!isWithinGrid)
        {
            transform.position = eventData.position;

            if (GameManager.Instance.CurrentGrid.IsWithinGrid(Input.mousePosition))
            {
                isWithinGrid = true;
            }
        }
        else
        {
            float clampedX = Mathf.Clamp(Input.mousePosition.x, minGridPos.x, maxGridPos.x);
            float clampedY = Mathf.Clamp(Input.mousePosition.y, minGridPos.y, maxGridPos.y);

            transform.position = new Vector2(clampedX, clampedY);
        }

        // Highlight grid cell
        Vector2Int tempGridIndex = GetItemCellIndex();
        GameManager.Instance.HighlightGridVisual(this, tempGridIndex);

        CheckValidPosition();
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (!isInteractable) return;

        if (!isWithinGrid)
        {
            transform.position = originalPos;
        }
        else
        {
            GameManager.Instance.CurrentGrid.ClearHighlight();
            GameManager.Instance.PlacedItems.Add(this);

            AudioManager.Instance.PlaySoundFX("Drop");

            // Snap the draggable item to the grid cell
            GridCell gridCell = GetItemGridCell();
            transform.position = gridCell.Position;

            CheckValidPosition(
                () =>
                {
                    if (GameManager.Instance.invalidItems.Contains(this)) return;
                    GameManager.Instance.invalidItems.Add(this);
                    Debug.Log("Can't assign invalid item to grid");
                },
                () =>
                {
                    Vector2Int gridIndex = GetItemCellIndex();
                    GameManager.Instance.AssignItemToGrid(this, gridIndex);
                }
            );
            GameManager.Instance.ValidateGrid();
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (!isInteractable) return;

        if (isDragging)
        {
            isDragging = false;
            return;
        }
        if (!isWithinGrid) return;
        
        AudioManager.Instance.PlaySoundFX("Rotate");

        // Handle Rotation Logic
        currentDirection = (Direction)(((int)currentDirection + 1) % Enum.GetNames(typeof(Direction)).Length);
        transform.rotation = Quaternion.AngleAxis((transform.eulerAngles.z - 90) % 360, Vector3.forward);
        OffsetItem();

        // Revalidate the grid (need refactoring)
        GameManager.Instance.CurrentGrid.ClearItem(this);
        CheckValidPosition(
            () => {
                if (GameManager.Instance.invalidItems.Contains(this)) return;
                GameManager.Instance.invalidItems.Add(this);
            },
            () =>
            {
                Vector2Int gridIndex = GetItemCellIndex();
                GameManager.Instance.AssignItemToGrid(this, gridIndex);
                GameManager.Instance.invalidItems.Remove(this);
            }
        );
        GameManager.Instance.ValidateGrid();
    }

    private void OffsetItem()
    {
        Vector2Int gridIndex = GetItemCellIndex();
        int minIndex;
        int maxIndex;
        GridCell gridCell = GetItemGridCell();

        switch (currentDirection)
        {
            case Direction.Left:
            case Direction.Right:
                minIndex = gridIndex.y - FirstPartLength;
                if (minIndex < 0)
                {
                    gridCell = GameManager.Instance.CurrentGrid.GetGridCellByIndex(new Vector2Int(gridIndex.x, gridIndex.y - minIndex));
                    break;
                }

                maxIndex = gridIndex.y + SecondPartLength;
                if (maxIndex >= GameManager.Instance.CurrentGrid.GridColumns)
                {
                    gridCell = GameManager.Instance.CurrentGrid.GetGridCellByIndex(new Vector2Int(gridIndex.x, gridIndex.y - (maxIndex - GameManager.Instance.CurrentGrid.GridColumns)));
                }
                break;
            case Direction.Up:
            case Direction.Down:
                minIndex = gridIndex.x - FirstPartLength;
                if (minIndex < 0)
                {
                    gridCell = GameManager.Instance.CurrentGrid.GetGridCellByIndex(new Vector2Int(gridIndex.x - minIndex, gridIndex.y));
                    break;
                }
                maxIndex = gridIndex.x + SecondPartLength;
                if (maxIndex >= GameManager.Instance.CurrentGrid.GridRows)
                {
                    gridCell = GameManager.Instance.CurrentGrid.GetGridCellByIndex(new Vector2Int(gridIndex.x - (maxIndex - GameManager.Instance.CurrentGrid.GridRows), gridIndex.y));
                    break;
                }
                break;
        }
        transform.position = gridCell.Position;
    }

    public bool CheckValidPosition(Action onInvalid = null, Action onValid = null)
    {
        List<GridCell> affectedGridCells = GameManager.Instance.CurrentGrid.GetAffectedGridCells(this);
        foreach(GridCell cell in affectedGridCells)
        {
            if (cell.assignedItem != null)
            {
                onInvalid?.Invoke();
                image.color = invalidColor;
                return false;
            }
        }
        image.color = defaultColor;
        onValid?.Invoke();
        return true;
    }

    public Vector2Int GetItemCellIndex()
    {
        RectTransformUtility.ScreenPointToLocalPointInRectangle(GameManager.Instance.CurrentGrid.GetComponent<RectTransform>(),
                transform.position, null, out Vector2 localPoint);
        return GameManager.Instance.CurrentGrid.GetGridCellIndex(localPoint);
    }

    public GridCell GetItemGridCell()
    {
        return GameManager.Instance.CurrentGrid.GetGridCellByIndex(GetItemCellIndex());
    }
}
