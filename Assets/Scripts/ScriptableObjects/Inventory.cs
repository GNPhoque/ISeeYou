using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class Inventory : ScriptableObject
{
	public List<string> items = new List<string>();
	public Carryable carryItem { get; private set; }

	public void AddItem(string item)
	{
		items.Add(item);
	}

	public void CarryNewItem(Carryable newItem)
	{
		RemoveCarryItem();
		carryItem = newItem;
	}

	public void RemoveCarryItem()
	{
		if (carryItem)
		{
			carryItem.ResetObject();
			carryItem = null;
		}
	}
}
