using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public enum UsableState
{
	NotUsed,
	Used,
	Using
}

[RequireComponent(typeof(Collider))]
public abstract class Useable : MonoBehaviour, IUseable
{
	[SerializeField]
	protected UnityEvent OnUsed;
	[SerializeField]
	protected UnityEvent OnUnused;

	protected UsableState state = UsableState.NotUsed;
	protected Transform t;

	protected virtual void Awake()
	{
		t = GetComponent<Transform>();
	}

	public abstract void Use();

	public virtual void LongUse()
	{
		Use();
	}
}
