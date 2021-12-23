using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PinpadActivator : Useable
{
	[SerializeField]
	UnityEvent onSuccess;
	[SerializeField]
	UnityEvent onFail;

	GameObject pinpad;

	public override void Use()
	{
		if (!pinpad)
		{
			pinpad = ((MonoBehaviour)OnUsed.GetPersistentTarget(0)).gameObject;
			pinpad.GetComponent<Keypad>().SetEvents(onSuccess, onFail);
		}
		OnUsed?.Invoke();
	}
}
