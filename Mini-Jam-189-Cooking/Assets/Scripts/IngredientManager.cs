using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Unity.VisualScripting;

public class IngredientManager : MonoBehaviour
{
    public List<GameObject> incredientPrefabs;
    public PathFinding pathFinding;
    public ObstacleGridEditor obstacleGridEditor;
    public int spawnX;
    public int spawnY;
    public Transform spawnPosition;
    public string ingredientName;
    public float moveAnimationSpeed = 0.7f;
    // For Testing
    public DishManager dishManager;
    public List<string> requiredIngredients;


    // Track occupied ingredient cells (not obstacles)
    private List<Vector2Int> occupiedCells = new List<Vector2Int>();
    private int maxSpawnTries = 100;

    void Start()
    {
        // Do not assign obstacleCells to occupiedCells; keep them separate
        InvokeRepeating(nameof(ScheduledSpawn), 5f, 5f);
        requiredIngredients = dishManager.requiredIngredients;
    }

    public void SetRequiredIngredients(List<string> newIngredients)
    {
        requiredIngredients = newIngredients;
    }

    GameObject SelectRandomIngredient()
    {
        int randomPlausibleIngrediantName = Random.Range(0, requiredIngredients.Count);
        GameObject selectedPrefab = null;
        foreach (var prefav in incredientPrefabs)
        {
            if (prefav.name == requiredIngredients[randomPlausibleIngrediantName])
            {
                selectedPrefab = prefav;
                break;
            }
        }
        return selectedPrefab;
    }

    void GetRandomSpawnPosition(out int x, out int y)
    {
        x = Random.Range(0, pathFinding.width);
        y = Random.Range(0, pathFinding.height);
    }


    void ScheduledSpawn()
    {
        int tries = 0;
        bool found = false;
        Vector2Int cell = Vector2Int.zero;
        while (tries < maxSpawnTries)
        {
            GetRandomSpawnPosition(out spawnX, out spawnY);
            cell = new Vector2Int(spawnX, spawnY);
            // Don't spawn on obstacles or on other ingredients
            bool isObstacle = obstacleGridEditor != null && obstacleGridEditor.obstacleCells.Contains(cell);
            if (!isObstacle && !occupiedCells.Contains(cell))
            {
                found = true;
                break;
            }
            tries++;
        }
        if (!found)
        {
            Debug.LogWarning("No free cell found for ingredient spawn after max tries.");
            return;
        }
        occupiedCells.Add(cell);
        GameObject ingredient = SelectRandomIngredient();
        Vector3 targetPosition = pathFinding.gridToWorld(cell);
        GameObject obj = Instantiate(ingredient, spawnPosition.position, Quaternion.identity);
        Ingredient ingredientScript = obj.GetComponent<Ingredient>();
        if (ingredientScript != null)
        {
            ingredientScript.gridCell = cell;
        }
        obj.transform.DOMove(targetPosition + new Vector3(0, -1, 0), moveAnimationSpeed).SetEase(Ease.OutQuad);
        Debug.Log("Total Occupied Cells : " + occupiedCells.Count);
    }

    // Call this when an ingredient is removed from the grid (e.g., picked up or used)
    public void FreeCell(Vector2Int cell)
    {
        occupiedCells.Remove(cell);
    }

    // Check if a cell is occupied (for obstacles or ingredients)
    public bool IsOccupied(Vector2Int cell)
    {
        return occupiedCells.Contains(cell);
    }

    // Add a cell to the occupied list (for obstacles)
    public void AddOccupiedCell(Vector2Int cell)
    {
        if (!occupiedCells.Contains(cell))
            occupiedCells.Add(cell);
    }
}
