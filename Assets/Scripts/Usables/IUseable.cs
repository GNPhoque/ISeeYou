using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IUseable
{
	void Use(GameObject source = null);
	void LongUse(GameObject source = null);
	void Use();
	void LongUse();
}
