using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CancelUseable : Useable
{
	public UsableState Getstate()
	{
		return state;
	}

	public abstract void Use(GameObject source);

	public virtual void LongUse(GameObject source)
	{
		Use(source);
	}
}
