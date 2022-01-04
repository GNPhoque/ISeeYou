using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GridPuzzleActivator : Useable
{
	[SerializeField]
	UnityEvent onSuccess;
	[SerializeField]
	UnityEvent onFail;

	GameObject pinpad;

	public override void Use()
	{
		OnUsed?.Invoke();
	}

	public void ClearAllEvents()
	{
		onSuccess = null;
		onFail = null;
		OnUnused = null;
		OnUsed = null;
	}

	public void ClearOnSuccessEvents()
	{
		onSuccess = null;
	}
}
