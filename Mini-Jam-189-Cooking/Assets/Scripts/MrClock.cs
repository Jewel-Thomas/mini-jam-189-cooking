using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MrClock : MonoBehaviour
{
    public GameObject[] ingredientPrefabs;
    public Transform throwPoint;
    public Transform chefTarget; // Assign the chef's Transform in the Inspector
    public float throwForce = 7f;

    public void ThrowIngredients(List<string> ingredients)
    {
        foreach (string ing in ingredients)
        {
            GameObject prefab = ingredientPrefabs.FirstOrDefault(p => p.name == ing);
            if (prefab != null && chefTarget != null)
            {
                GameObject obj = Instantiate(prefab, throwPoint.position, Quaternion.identity);
                Rigidbody2D rb = obj.GetComponent<Rigidbody2D>();
                if (rb != null)
                {
                    Vector2 dir = (chefTarget.position - throwPoint.position).normalized;
                    rb.linearVelocity = dir * throwForce;
                }
            }
        }
    }

    public void ReactToFailure() {
        // Screen shake, grumble, etc.
    }
}
