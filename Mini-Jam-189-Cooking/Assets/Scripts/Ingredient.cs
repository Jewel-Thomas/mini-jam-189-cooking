using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Ingredient : MonoBehaviour
{
    public int decayTime = 10;
    public IngredientManager ingredientManager;
    public DishManager dishManager;
    public Vector2Int gridCell;
    public Sprite ingredientSprite;

    void Start()
    {
        ingredientManager = GameObject.FindGameObjectWithTag("Prop").GetComponent<IngredientManager>();
        dishManager = GameObject.FindFirstObjectByType<DishManager>();
        Invoke(nameof(DestroyAndRemove), decayTime);
    }

    // Called by ChefMover when chef reaches this cell
    public void Collect()
    {
        if (dishManager != null)
        {
            dishManager.AddIngredient(gameObject.name.Replace("(Clone)", ""));
        }
        ingredientManager.FreeCell(gridCell);
        Destroy(gameObject);
    }

    void DestroyAndRemove()
    {
        ingredientManager.FreeCell(gridCell);
        Destroy(gameObject);
    }

}
