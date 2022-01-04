using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class KeyLock : Useable
{
	[SerializeField]
	Inventory inventory;
	[SerializeField]
	string requiredKey;
	[SerializeField]
	UnityEvent OnUnlock;

	public override void Use()
	{
		if (inventory.items.Contains(requiredKey))
		{
			OnUnlock?.Invoke();
			//PlaySound("Unlock");
			Destroy(gameObject);
		}
	}
}
