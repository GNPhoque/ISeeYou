using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lootable : Useable
{
	[SerializeField]
	Inventory inventory;

	public override void Use()
	{
		inventory.AddItem(gameObject.name);
		OnUsed?.Invoke();
		Destroy(gameObject);
	}
}