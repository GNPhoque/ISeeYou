using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UseablePositionChange : CancelUseable
{
	[SerializeField]
	Useable tmpUseable;

	GameObject target;
	Vector3 positionBeforeChange;

	public override void Use(GameObject source)
	{
		if (state== UsableState.Used)
		{
			target = source;
			tmpUseable = this;
			target.transform.position = positionBeforeChange;
		}
		else
		{
			tmpUseable = null;
			positionBeforeChange = target.transform.position;
		}
	}

	public override void Use()
	{
		tmpUseable = null;
		target.transform.position = positionBeforeChange;
	}
}
