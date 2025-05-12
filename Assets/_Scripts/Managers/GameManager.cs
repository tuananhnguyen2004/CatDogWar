using Utils;
using UnityEngine;
using System.Collections.Generic;
using System;
using SOEventSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Threading.Tasks;

public enum PlayerTurn
{
    FirstPlayer,
    SecondPlayer
}

public class GameManager : Singleton<GameManager>
{

    [Header("List")]
    public List<DraggableItem> invalidItems;
    private HashSet<DraggableItem> placedItems;
    public HashSet<DraggableItem> PlacedItems => placedItems;
    [SerializeField] private List<Grid> grids;

    [Header("Visuals")]
    public Sprite hitImage;
    public Color hitColor;
    public Color missedColor;

    [Header("Event Publisher")]
    [SerializeField] private BoolPublisher onGridValid;
    [SerializeField] private VoidPublisher onGameplayEnter;
    [SerializeField] private IntPublisher onPlayerWin;
    [SerializeField] private VoidPublisher onGameOver;
    [SerializeField] private PlayerTurnPublisher onTurnSwitch;
    [SerializeField] private Vector2Publisher onCellAttack;

    [Header("Others")]
    private PlayerTurn currentTurn = PlayerTurn.FirstPlayer;
    public PlayerTurn CurrentTurn => currentTurn;
    public Grid CurrentGrid => grids[(int) currentTurn];
    private bool isInGameplay = false;
    private bool isAttacking = false;
    public bool IsAttacking { get => isAttacking; set => isAttacking = value; }

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
        isInGameplay = true;
        onGameplayEnter.RaiseEvent();
        SwitchTurn(PlayerTurn.SecondPlayer);
    }

    public async Task GameOver()
    {
        int wonPlayer = (int)currentTurn == 0? 1: 0;
        isInGameplay = false;

        await Task.Delay(1000);
        onPlayerWin.RaiseEvent(wonPlayer + 1);
        onGameOver.RaiseEvent();
    }

    public void SwitchTurn()
    {
        AssignItemsToGrid();

        currentTurn = (currentTurn == PlayerTurn.FirstPlayer) ? PlayerTurn.SecondPlayer : PlayerTurn.FirstPlayer;
        CurrentGrid.TakeTurn();

        if (!isInGameplay) return;
        onTurnSwitch.RaiseEvent(currentTurn);
    }

    public void SwitchTurn(PlayerTurn playerTurn)
    {
        AssignItemsToGrid();

        currentTurn = playerTurn;
        CurrentGrid.TakeTurn();

        if (!isInGameplay) return;

        onTurnSwitch.RaiseEvent(currentTurn);
    }

    private void AssignItemsToGrid()
    {
        if (isInGameplay) return;
        
        foreach (DraggableItem item in placedItems)
        {
            CurrentGrid.status.Add(item, item.Width);
            item.gameObject.SetActive(false);
            Debug.Log(item + " " + item.Width);
        }
        placedItems.Clear();
    }

    private void Update()
    {
        if (!isInGameplay) return;

        if (Input.GetKeyDown(KeyCode.Mouse0) && !isAttacking)
        {
            RectTransformUtility.ScreenPointToLocalPointInRectangle(CurrentGrid.GetComponent<RectTransform>(),
                Input.mousePosition, null, out Vector2 localPoint);
            if (CurrentGrid.IsWithinGrid(localPoint))
            {
                onCellAttack.RaiseEvent(localPoint);
            }
            else
            {
                Debug.Log("Out of grid");
            }
        }
    }
}
