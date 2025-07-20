using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class ChefMover : MonoBehaviour
{
    public float moveDurationPerCell = 0.2f;

    // Call this to move the chef along a path
    public IngredientManager ingredientManager;

    public void MoveAlongPath(List<Vector2Int> path, System.Func<Vector2Int, Vector3> gridToWorld, System.Action onComplete = null)
    {
        if (path == null || path.Count < 2) { onComplete?.Invoke(); return; }
        Sequence seq = DOTween.Sequence();
        for (int i = 1; i < path.Count; i++)
        {
            Vector3 targetPos = gridToWorld(path[i]);
            seq.Append(transform.DOMove(targetPos, moveDurationPerCell).SetEase(Ease.Linear));
        }
        // When the sequence completes, check for ingredient at the target cell and collect it
        Vector2Int targetCell = path[path.Count - 1];
        seq.OnComplete(() =>
        {
            if (ingredientManager != null)
            {
                Ingredient[] allIngredients = FindObjectsByType<Ingredient>(FindObjectsSortMode.None);
                foreach (var ingredient in allIngredients)
                {
                    if (ingredient.gridCell == targetCell)
                    {
                        ingredient.Collect();
                        break;
                    }
                }
            }
            onComplete?.Invoke();
        });
        seq.Play();
    }
}
