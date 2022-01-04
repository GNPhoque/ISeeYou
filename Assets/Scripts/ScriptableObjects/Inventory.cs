using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class Inventory : ScriptableObject
{
	public List<string> items = new List<string>();

	public void AddItem(string item)
	{
		items.Add(item);
	}
}
