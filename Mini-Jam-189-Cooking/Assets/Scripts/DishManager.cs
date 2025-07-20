using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class DishManager : MonoBehaviour
{
    public List<string> requiredIngredients = new();
    public List<string> currentIngredients = new();

    // Track buttons for each ingredient name
    private Dictionary<string, GameObject> ingredientButtons = new();

    // Assign these in the inspector
    public GameObject ingredientButtonPrefab;
    public Transform contentParent; // The Content object of the ScrollRect
    public IngredientManager ingredientManager; // Assign in inspector or find at runtime
    public Button submitButton; // Assign the Submit button in the inspector

    public void AddIngredient(string name)
    {
        currentIngredients.Add(name);
        // Toggle the collected indicator on the corresponding button
        if (ingredientButtons.TryGetValue(name, out GameObject btn) && btn.transform.childCount > 0)
        {
            Transform indicator = btn.transform.GetChild(0);
            Image img = indicator.GetComponent<Image>();
            if (img != null)
                img.enabled = true;
        }
        // Enable submit button only if all required ingredients are collected
        if (submitButton != null)
            submitButton.interactable = currentIngredients.Count == requiredIngredients.Count;
    }

    public void SubmitDish()
    {
        bool success = requiredIngredients.OrderBy(i => i).SequenceEqual(currentIngredients.OrderBy(i => i));
        Debug.Log(success ? "Dish prepared successfully!" : "Dish is incorrect!");
        GameManager.Instance.EndRound(success);
        ResetDish();
    }

    public void SetNewDish(List<string> ingredients)
    {
        requiredIngredients = ingredients;
        currentIngredients.Clear();

        // Clear previous buttons
        foreach (Transform child in contentParent)
        {
            Destroy(child.gameObject);
        }
        ingredientButtons.Clear();

        // Disable submit button at start of new dish
        if (submitButton != null)
            submitButton.interactable = false;

        // For each required ingredient, create a button and set its image
        foreach (string ingredientName in requiredIngredients)
        {
            GameObject btn = Instantiate(ingredientButtonPrefab, contentParent);
            // Find the prefab for this ingredient by name
            GameObject prefab = ingredientManager.incredientPrefabs.Find(p => p.name == ingredientName);
            if (prefab != null)
            {
                Ingredient ingredient = prefab.GetComponent<Ingredient>();
                Sprite sprite = null;
                if (ingredient != null && ingredient.ingredientSprite != null)
                {
                    sprite = ingredient.ingredientSprite;
                }
                else
                {
                    // fallback: try SpriteRenderer
                    SpriteRenderer sr = prefab.GetComponent<SpriteRenderer>();
                    if (sr != null)
                        sprite = sr.sprite;
                }
                // Set the image on the button
                Image img = btn.GetComponentInChildren<Image>();
                if (img != null && sprite != null)
                {
                    img.sprite = sprite;
                }
            }
            btn.name = ingredientName + "Button";
            // Ensure the indicator is off initially
            if (btn.transform.childCount > 0)
            {
                Image indicator = btn.transform.GetChild(0).GetComponent<Image>();
                if (indicator != null)
                    indicator.enabled = false;
            }
            ingredientButtons[ingredientName] = btn;
        }
    }

    public void ResetDish()
    {
        currentIngredients.Clear();
        // Clear plate visuals
        // Reset all indicators
        foreach (var btn in ingredientButtons.Values)
        {
            if (btn != null && btn.transform.childCount > 0)
            {
                Image indicator = btn.transform.GetChild(0).GetComponent<Image>();
                if (indicator != null)
                    indicator.enabled = false;
            }
        }
        // Disable submit button on reset
        if (submitButton != null)
            submitButton.interactable = false;
    }
}
