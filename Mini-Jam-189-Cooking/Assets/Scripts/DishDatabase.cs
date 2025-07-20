using System.Collections.Generic;
using UnityEngine;

public static class DishDatabase
{
    public static List<string> GetRandomDish()
    {
        List<List<string>> dishes = new()
        {
            new() { "apple", "banana", "cherries", "chocolate", "cheese" },                // Fruit Fondue
            new() { "carrot", "onion", "mushroom", "cabbage", "corn" },                    // Veggie Stew
            new() { "turkey", "cheese", "onion", "mushroom", "celery-stick" },             // Turkey Casserole
            new() { "egg", "cheese", "onion", "mushroom", "corn" },                        // Breakfast Omelette
            new() { "cabbage", "carrot", "onion", "celery-stick", "corn", "cheese" },      // Garden Soup
            new() { "banana", "apple", "cherries", "chocolate", "egg" },                   // Fruit Pancake
            new() { "turkey", "corn", "cheese", "mushroom", "onion", "egg" },              // Turkey Skillet
            new() { "cheese", "chocolate", "banana", "apple", "egg" },                     // Dessert Melt
            new() { "onion", "mushroom", "carrot", "cabbage", "celery-stick", "corn" },    // Stir Fry
            new() { "egg", "cheese", "mushroom", "corn", "apple", "turkey" },              // Brunch Mix
            new() { "cheese", "onion", "egg", "chocolate", "banana" },                     // Breakfast Surprise
            new() { "cherries", "banana", "apple", "chocolate", "cheese", "egg" }          // Sweet Treat Bowl
        };

        return dishes[Random.Range(0, dishes.Count)];
    }

}
