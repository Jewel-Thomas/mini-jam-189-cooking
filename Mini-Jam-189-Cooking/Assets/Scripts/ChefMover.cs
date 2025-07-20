using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class ChefMover : MonoBehaviour
{
    public float moveDurationPerCell = 0.2f;

    // Call this to move the chef along a path
    public void MoveAlongPath(List<Vector2Int> path, System.Func<Vector2Int, Vector3> gridToWorld)
    {
        if (path == null || path.Count < 2) return;
        Sequence seq = DOTween.Sequence();
        for (int i = 1; i < path.Count; i++)
        {
            Vector3 targetPos = gridToWorld(path[i]);
            seq.Append(transform.DOMove(targetPos, moveDurationPerCell).SetEase(Ease.Linear));
        }
        seq.Play();
    }
}
