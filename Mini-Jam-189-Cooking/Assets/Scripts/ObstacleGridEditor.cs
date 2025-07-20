using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

[ExecuteInEditMode]
public class ObstacleGridEditor : MonoBehaviour
{
    public PathFinding pathFinding;
    public IngredientManager ingredientManager;
    public List<Vector2Int> obstacleCells = new List<Vector2Int>();
    public Color obstacleColor = Color.red;

    private void OnDrawGizmos()
    {
        if (pathFinding == null) return;
        Gizmos.color = obstacleColor;
        foreach (var cell in obstacleCells)
        {
            Vector3 pos = pathFinding.gridToWorld(cell);
            Gizmos.DrawCube(pos, Vector3.one * 0.8f);
        }
    }

#if UNITY_EDITOR
    private void OnEnable()
    {
        SceneView.duringSceneGui += OnSceneGUI;
    }
    private void OnDisable()
    {
        SceneView.duringSceneGui -= OnSceneGUI;
    }
    private void OnSceneGUI(SceneView sceneView)
    {
        if (pathFinding == null) return;
        Event e = Event.current;
        // Paint obstacles with LMB hold/drag, erase with RMB hold/drag
        if ((e.type == EventType.MouseDown || e.type == EventType.MouseDrag) && (e.button == 0 || e.button == 1) && !e.alt)
        {
            Ray ray = HandleUtility.GUIPointToWorldRay(e.mousePosition);
            float t = 0f;
            if (ray.direction.z != 0f)
                t = -ray.origin.z / ray.direction.z;
            Vector3 worldPos = ray.origin + ray.direction * t;
            if (pathFinding.grid == null)
            {
                Debug.LogWarning("PathFinding.grid is null. Make sure PathFinding is initialized in edit mode.");
                return;
            }
            pathFinding.grid.GetXY(worldPos, out int x, out int y);
            // Bounds check
            if (x < 0 || x >= pathFinding.width || y < 0 || y >= pathFinding.height)
                return;
            Vector2Int cell = new Vector2Int(x, y);
            if (e.button == 0)
            {
                // Paint obstacle
                if (!obstacleCells.Contains(cell))
                {
                    obstacleCells.Add(cell);
                    if (ingredientManager != null && !ingredientManager.IsOccupied(cell))
                    {
                        ingredientManager.AddOccupiedCell(cell);
                    }
                    e.Use();
                    SceneView.RepaintAll();
                }
            }
            else if (e.button == 1)
            {
                // Erase obstacle
                if (obstacleCells.Contains(cell))
                {
                    obstacleCells.Remove(cell);
                    if (ingredientManager != null && ingredientManager.IsOccupied(cell))
                    {
                        ingredientManager.FreeCell(cell);
                    }
                    e.Use();
                    SceneView.RepaintAll();
                }
            }
        }
    }
#endif
}
