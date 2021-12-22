using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CancelUseable : Useable
{
	Useable _tmpUseable;

	public Useable tmpUseable { get => _tmpUseable; set => _tmpUseable = value; }

	public override void Use()
	{
		tmpUseable.Use();
		tmpUseable = null;
	}

	public UsableState Getstate()
	{
		return state;
	}
}
