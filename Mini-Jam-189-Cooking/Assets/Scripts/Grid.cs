using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CodeMonkey.Utils;

public class Grid
{
    public int width;
    public int height;
    public float cellSize;
    public Vector3 originPosition;
    private int[,] gridArray;
    private TextMesh[,] debugTextArray;
    public Grid(int width, int height, float cellSize, Vector3 originPosition)
    {
        this.width = width;
        this.height = height;
        this.cellSize = cellSize;
        this.originPosition = originPosition;

        gridArray = new int[width, height];
        debugTextArray = new TextMesh[width, height];
        for (int x = 0; x < gridArray.GetLength(0); x++)
        {
            for (int y = 0; y < gridArray.GetLength(1); y++)
            {
                // debugTextArray[x, y] = UtilsClass.CreateWorldText(gridArray[x, y].ToString(), null, GetWorldPosition(x, y) + new Vector3(cellSize, cellSize) * 0.5f, 20, Color.white, TextAnchor.MiddleCenter);
                Debug.DrawLine(GetWorldPosition(x, y), GetWorldPosition(x, y + 1), Color.white, 100f);
                Debug.DrawLine(GetWorldPosition(x, y), GetWorldPosition(x + 1, y), Color.white, 100f);
            }
            Debug.DrawLine(GetWorldPosition(0, height), GetWorldPosition(width, height), Color.white, 100f);
            Debug.DrawLine(GetWorldPosition(width, 0), GetWorldPosition(width, height), Color.white, 100f);
        }

    }

    private bool CheckBounds(int x, int y)
    {
        return x >= 0 && x < width && y >= 0 && y < height;
    }

    public Vector3 GetWorldPosition(int x, int y)
    {
        return new Vector3(x, y) * cellSize + originPosition;
    }

    public void GetXY(Vector3 worldPosition, out int x, out int y)
    {
        x = Mathf.FloorToInt((worldPosition-originPosition).x / cellSize);
        y = Mathf.FloorToInt((worldPosition-originPosition).y / cellSize);
    }

    public void SetValue(int x, int y, int value)
    {
        if (!CheckBounds(x, y)) return;
        gridArray[x, y] = value;
        debugTextArray[x, y].text = gridArray[x, y].ToString();
    }

    public void SetValue(Vector3 worldPosition, int value)
    {
        GetXY(worldPosition, out int x, out int y);
        SetValue(x, y, value);
    }

    public int GetValue(int x, int y)
    {
        if (!CheckBounds(x, y)) return 0;
        return gridArray[x, y];
    }
    
    public int GetValue(Vector3 worldPosition)
    {
        GetXY(worldPosition, out int x, out int y);
        return GetValue(x, y);
    }
}
