using System.Collections.Generic;
using UnityEngine;

public static class DishDatabase
{
    public static List<string> GetRandomDish()
    {
        // You can improve this by using ScriptableObjects later
        List<List<string>> dishes = new()
        {
            new() { "Chicken", "Potato" },
            new() { "Bread", "Tomato" },
            new() { "Egg", "Salt" }
        };

        return dishes[Random.Range(0, dishes.Count)];
    }
}
