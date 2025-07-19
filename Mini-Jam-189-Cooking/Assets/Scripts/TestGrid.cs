using UnityEngine;
using CodeMonkey.Utils;

public class TestGrid : MonoBehaviour
{
    private Grid grid;
    public int width;
    public int height;
    public float cellSize;
    public Vector3 originPosition;
    void Start()
    {
        grid = new Grid(width, height, cellSize, originPosition);
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            grid.SetValue(UtilsClass.GetMouseWorldPosition(), 76);
        }   
        if (Input.GetMouseButtonDown(1))
        {
            Debug.Log(grid.GetValue(UtilsClass.GetMouseWorldPosition()));
        }
    }
}
