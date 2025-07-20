using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Ingredient : MonoBehaviour
{
    public int decayTime = 10;
    public IngredientManager ingredientManager;
    void Start()
    {
        ingredientManager = GameObject.FindGameObjectWithTag("Prop").GetComponent<IngredientManager>();
        Invoke(nameof(DestroyAndRemove), decayTime);
    }
    
    void DestroyAndRemove()
    {
        ingredientManager.FreeCell(new Vector2Int(ingredientManager.spawnX, ingredientManager.spawnY));
        Destroy(gameObject);
    }

}
