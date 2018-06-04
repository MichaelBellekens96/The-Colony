using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodProcessor : MonoBehaviour {

	public void AddMeal(GameObject rawFoodBox)
    {
        ResourceManager.Instance.AddMealToList();
        ResourceManager.Instance.RemoveResourceBox(rawFoodBox.transform);
    }

    public void PrepareMeal()
    {
        // Instantiate meal
        Debug.Log("Received a meal");
        ResourceManager.Instance.RemoveMealFromList();
    }
}
