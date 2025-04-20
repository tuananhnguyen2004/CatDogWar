using Utils;
using UnityEngine;
using System.Collections.Generic;
using System;
using SOEventSystem;

public class GameManager : Singleton<GameManager>
{
    private enum PlayerTurn
    {
        FirstPlayer,
        SecondPlayer
    }

    [Header("List")]
    public List<DraggableItem> invalidItems;
    private HashSet<DraggableItem> placedItems;
    public HashSet<DraggableItem> PlacedItems => placedItems;
    [SerializeField] private List<Grid> grids;

    [Header("Visuals")]
    public Color hitColor;
    public Color missedColor;

    [Header("Event Publisher")]
    [SerializeField] private BoolPublisher onGridValid;
    [SerializeField] private VoidPublisher onGameplayEnter;
    
    [Header("Others")]
    private PlayerTurn currentTurn = PlayerTurn.FirstPlayer;
    public Grid CurrentGrid => grids[(int) currentTurn];
    private bool isInGameplay = false;

    private void Awake()
    {
        invalidItems = new List<DraggableItem>();
        placedItems = new HashSet<DraggableItem>();
    }

    public void HighlightGridVisual(DraggableItem item, Vector2Int index)
    {
        CurrentGrid.ClearHighlight();

        List<GridCell> affectedGridCells = CurrentGrid.GetAffectedGridCells(item);
        foreach (GridCell cell in affectedGridCells)
        {
            CurrentGrid.HighlightCell(cell);
        }
    }

    public void AssignItemToGrid(DraggableItem item, Vector2Int index)
    {
        List<GridCell> affectedGridCells = CurrentGrid.GetAffectedGridCells(item);
        foreach (GridCell cell in affectedGridCells)
        {
            cell.assignedItem = item;
            //Debug.Log(cell.assignedItem);
        }
    }

    public void ValidateGrid()
    {
        for (int i = 0; i < invalidItems.Count; ++i)
        {
            DraggableItem item = invalidItems[i];
            CurrentGrid.ClearItem(item);
            item.CheckValidPosition(
                onValid: () =>
                {
                    Vector2Int gridIndex = item.GetItemCellIndex();
                    AssignItemToGrid(item, gridIndex);
                    invalidItems.Remove(item);
                }
            );
        }
        if (invalidItems.Count == 0 && placedItems.Count == 5)
        {
            onGridValid.RaiseEvent(true);
        }
        else
        {
            onGridValid.RaiseEvent(false);
        }
    }

    public void EnterGameplay()
    {
        AssignItemsToGrid();

        // Second player will be attacked first
        currentTurn = PlayerTurn.SecondPlayer;
        isInGameplay = true;
        onGameplayEnter.RaiseEvent();
    }

    public void SwitchTurn()
    {
        AssignItemsToGrid();
        currentTurn = currentTurn == PlayerTurn.FirstPlayer ? PlayerTurn.SecondPlayer : PlayerTurn.FirstPlayer;
        CurrentGrid.TakeTurn();
    }

    private void AssignItemsToGrid()
    {
        if (!isInGameplay)
        {
            foreach (DraggableItem item in placedItems)
            {
                CurrentGrid.status.Add(item, item.Width);
                item.gameObject.SetActive(false);
                Debug.Log(item + " " + item.Width);
            }
            placedItems.Clear();
        }
    }

    private void Update()
    {
        if (!isInGameplay) return;

        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            RectTransformUtility.ScreenPointToLocalPointInRectangle(CurrentGrid.GetComponent<RectTransform>(),
                Input.mousePosition, null, out Vector2 localPoint);
            if (CurrentGrid.IsWithinGrid(localPoint))
            {
                Vector2Int gridIndex = CurrentGrid.GetGridCellIndex(localPoint);
                GridCell gridCell = CurrentGrid.GetGridCellByIndex(gridIndex);

                Debug.Log("Cell index: " + gridIndex + " get attacked on " + CurrentGrid.gameObject.name);
                gridCell.GetAttacked();
            }
            else
            {
                Debug.Log("Out of grid");
            }
        }
    }
}
