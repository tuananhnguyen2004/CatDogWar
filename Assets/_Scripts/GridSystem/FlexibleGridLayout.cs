using UnityEngine;
using UnityEngine.UI;

public class FlexibleGridLayout : LayoutGroup
{
    public enum FitType
    {
        Width,
        Height,
        FixedRows,
        FixedColumns,
        Uniform
    }

    public enum Corner
    {
        TopLeft,
        TopRight,
        BottomLeft,
        BottomRight
    }

    public FitType fitType;
    public Corner startCorner = Corner.TopLeft;
    public int rows;
    public int columns;
    public Vector2 cellSize;
    public Vector2 spacing;
    public bool fitX;
    public bool fitY;

    public override void CalculateLayoutInputVertical()
    {
        if (fitType == FitType.Width || fitType == FitType.Height || fitType == FitType.Uniform)
        {
            fitX = true;
            fitY = true;
            float sqrt = Mathf.Sqrt(transform.childCount);
            columns = Mathf.CeilToInt(sqrt);
            rows = Mathf.CeilToInt(sqrt);
        }

        if (fitType == FitType.Width || fitType == FitType.FixedColumns)
        {
            rows = Mathf.CeilToInt(transform.childCount / (float)columns);
        }
        else if (fitType == FitType.Height || fitType == FitType.FixedRows)
        {
            columns = Mathf.CeilToInt(transform.childCount / (float)rows);
        }

        float cellWidth = (rectTransform.rect.width / (float)columns) - ((spacing.x / (float)columns) * (columns - 1))
            - (padding.left / (float) columns) - (padding.right / (float) columns);
        float cellHeight = (rectTransform.rect.height / (float)rows) - ((spacing.y / (float)rows) * (rows - 1))
            - (padding.top / (float)rows) - (padding.bottom / (float)rows); ;

        float aspectRatio = cellSize.x / cellSize.y;
        if (fitX && fitY)
        {
            if (cellWidth / cellHeight > aspectRatio)
            {
                cellHeight = cellWidth / aspectRatio;
            }
            else
            {
                cellWidth = cellHeight * aspectRatio;
            }
        }

        cellSize.x = fitX? cellWidth : cellSize.x;
        cellSize.y = fitY? cellHeight : cellSize.y;

        int columnCount = 0;
        int rowCount = 0;
        for (int i = 0; i < rectChildren.Count; i++)
        {
            switch (startCorner)
            {
                case Corner.TopLeft:
                    rowCount = i / columns;
                    columnCount = i % columns;
                    break;
                case Corner.TopRight:
                    rowCount = i / columns;
                    columnCount = columns - 1 - (i % columns);
                    break;
                case Corner.BottomLeft:
                    rowCount = rows - 1 - (i / columns);
                    columnCount = i % columns;
                    break;
                case Corner.BottomRight:
                    rowCount = rows - 1 - (i / columns);
                    columnCount = columns - 1 - (i % columns);
                    break;
            }
            var item = rectChildren[i];
            var x = columnCount * cellSize.x + (spacing.x * columnCount) + padding.left;
            var y = rowCount * cellSize.y + (spacing.y * rowCount) + padding.top;

            SetChildAlongAxis(item, 0, x, cellSize.x);
            SetChildAlongAxis(item, 1, y, cellSize.y);
        }
    }

    public override void SetLayoutHorizontal()
    {
    }

    public override void SetLayoutVertical()
    {
        
    }
}
