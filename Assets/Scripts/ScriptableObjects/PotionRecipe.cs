using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class PotionRecipe : ScriptableObject
{
    [SerializeField]
    public Ingredient[] ingredients;
    [SerializeField]
    public GameObject reward;

	public override string ToString()
	{
        return string.Join(",", ingredients);
	}
}

[Serializable]
public struct Ingredient
{
    public string item;
    public int time;

    public Ingredient(string item, int time)
    {
        this.item = item;
        this.time = time;
    }

	public override string ToString()
	{
        return $"{item}";
        //return $"{item},{(time < 5 ? 0 : time)}";
	}
}
