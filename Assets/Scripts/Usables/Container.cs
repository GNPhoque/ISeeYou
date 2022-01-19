using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.Events;

public class Container : Useable
{
	[SerializeField]
	Inventory inventory;
	[SerializeField]
	PotionRecipe[] recipes;
	[SerializeField]
	UnityEvent OnSuccess;

	List<Ingredient> ingredients;

	private void Start()
	{
		ingredients = new List<Ingredient>();
	}

	public override void Use()
	{
		if (inventory.carryItem)
		{
			if (ingredients.Any())
			{
				Ingredient ingredient = ingredients.Last();
				ingredient.time = Mathf.RoundToInt(Time.time - ingredient.time);
				ingredients[ingredients.Count - 1] = ingredient;
			}
			AddIngredient();
			if (ingredients.Count == 4)
			{
				ingredients.RemoveAt(0);
			}
			if (CheckRecipe())
			{
				OnSuccess?.Invoke();
			}
		}
		else
		{
			ResetObject();
		}
	}

	void AddIngredient()
	{
		Ingredient ingredient = new Ingredient(inventory.carryItem.name, Mathf.RoundToInt(Time.time));
		inventory.RemoveCarryItem();
		ingredients.Add(ingredient);
	}

	bool CheckRecipe()
	{
		foreach (PotionRecipe recipe in recipes)
		{
			if (ingredients.Count == recipe.ingredients.Length)
			{
				Debug.Log(recipe.ToString());
				Debug.Log(string.Join(",", ingredients));
				if (recipe.ToString() == string.Join(",", ingredients))
				{
					Debug.Log("Recipe Success!");
					GameObject reward = Instantiate(recipe.reward);
					reward.GetComponent<Carryable>().Use();
					return true;
				}
			} 
		}
		return false;
	}

	private void ResetObject()
	{
		ingredients = new List<Ingredient>();
	}
}
