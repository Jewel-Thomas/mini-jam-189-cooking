using UnityEngine;

[ExecuteAlways]
public class GridGizmoDrawer : MonoBehaviour
{
    public int width = 10;
    public int height = 10;
    public float cellSize = 1f;
    public Vector3 originPosition = Vector3.zero;
    public Color gridColor = Color.cyan;

    void OnDrawGizmos()
    {
        Gizmos.color = gridColor;
        for (int x = 0; x <= width; x++)
        {
            Vector3 from = originPosition + new Vector3(x * cellSize, 0, 0);
            Vector3 to = originPosition + new Vector3(x * cellSize, height * cellSize, 0);
            Gizmos.DrawLine(from, to);
        }
        for (int y = 0; y <= height; y++)
        {
            Vector3 from = originPosition + new Vector3(0, y * cellSize, 0);
            Vector3 to = originPosition + new Vector3(width * cellSize, y * cellSize, 0);
            Gizmos.DrawLine(from, to);
        }
    }
}
