using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DishManager : MonoBehaviour
{
    public List<string> requiredIngredients = new();
    public List<string> currentIngredients = new();

    public void AddIngredient(string name)
    {
        currentIngredients.Add(name);
        // Optional: update plate visuals
    }

    public void SubmitDish()
    {
        bool success = requiredIngredients.OrderBy(i => i).SequenceEqual(currentIngredients.OrderBy(i => i));
        GameManager.Instance.EndRound(success);
        ResetDish();
    }

    public void SetNewDish(List<string> ingredients)
    {
        requiredIngredients = ingredients;
        currentIngredients.Clear();
        // Update UI to show required dish
    }

    public void ResetDish()
    {
        currentIngredients.Clear();
        // Clear plate visuals
    }
}
