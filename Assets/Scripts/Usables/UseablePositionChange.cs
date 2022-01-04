using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UseablePositionChange : CancelUseable
{
	[SerializeField]
	Vector3 offset;
	[SerializeField]
	new Collider collider;

	GameObject target;
	Vector3 positionBeforeChange;

	public override void Use(GameObject source)
	{
		if (state == UsableState.Used)
		{
			collider.isTrigger = false;
			target.transform.position = positionBeforeChange;
			target = null;
			state = UsableState.NotUsed;
		}
		else
		{
			collider.isTrigger = true;
			target = source;
			positionBeforeChange = target.transform.position + offset;
			target.transform.position = t.position;
			state = UsableState.Used;
		}
	}

	public override void Use()
	{
		throw new System.NotImplementedException("CancelUseable.Use() should not be used, send the caller gameObject in parameters");
	}
}
