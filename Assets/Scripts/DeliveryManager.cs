using UnityEngine;
using System.Collections.Generic;

public class DeliveryManager : MonoBehaviour
{

    public static DeliveryManager Instance { get; private set; }

    [SerializeField] private RecipeListSO recipeListSO;

    private List<RecipeSO> waitingRecipeSOList;
    private float spawnRecipeTimer;
    private float spawnRecipeTimerMax = 4f;
    private int waitingRecipeMax = 4;

    private void Awake() {
        Instance = this;
        waitingRecipeSOList = new List<RecipeSO>();
    }

    private void Update() {
        spawnRecipeTimer -= Time.deltaTime;
        if (spawnRecipeTimer <= 0f) {
            spawnRecipeTimer = spawnRecipeTimerMax;

            if (waitingRecipeSOList.Count < waitingRecipeMax) {
                RecipeSO waitingRecipeSO = recipeListSO.recipeSOList[Random.Range(0, recipeListSO.recipeSOList.Count)];
                Debug.Log(waitingRecipeSO.recipeName);
                waitingRecipeSOList.Add(waitingRecipeSO);
            }
        }
    }

    public void DeliverRecipe(PlateKitchenObject plateKitchenObject) {
        for (int i = 0; i < waitingRecipeSOList.Count; i++) {
            RecipeSO waitingRecipeSO = waitingRecipeSOList[i];

            if (waitingRecipeSO.kitchenObjectSOList.Count == plateKitchenObject.GetKitchenObjectSOList().Count) {
                // Has the same amount of ingredients
                bool plateContentsMatchesRecipe = true;
                foreach (KitchenObjectSO kitchenObjectSO in waitingRecipeSO.kitchenObjectSOList) {
                    // Cycling through the recipe ingredients
                    bool ingredientFound = false;
                    foreach (KitchenObjectSO plateIngredient in plateKitchenObject.GetKitchenObjectSOList()) {
                        // Cycling through the plate ingredients
                        if (plateIngredient == kitchenObjectSO) {
                            // Found the ingredient in the plate
                            ingredientFound = true;
                            break;
                        }
                    }
                    if (!ingredientFound) {
                        // This recipe ingredient was not found in the plate
                        plateContentsMatchesRecipe = false;
                    }
                }

                if (plateContentsMatchesRecipe) {
                    // The plate matches the recipe
                    Debug.Log("Delivered correct recipe!");
                    waitingRecipeSOList.RemoveAt(i);
                    return;
                }
            }
        }

        // No matching recipe found
        Debug.Log("Delivered incorrect recipe!");
    }
}
