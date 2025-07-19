using UnityEngine;

public class Ingredient : MonoBehaviour
{
    public string ingredientName;

    void OnMouseDown()
    {
        FindObjectOfType<DishManager>().AddIngredient(ingredientName);
        Destroy(gameObject); // One-time use
    }
}
