using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotatable : Useable
{
	[SerializeField]
	Vector3 rotation;
	[SerializeField]
	float tweenDuration;
	[SerializeField]
	private bool isLocked;

	private bool collided;

	public override void Use()
	{
		if (!isLocked && state != UsableState.Using)
		{
			if (state == UsableState.NotUsed)
			{
				state = UsableState.Using;
				t.DORotate(rotation, tweenDuration, RotateMode.LocalAxisAdd).onComplete = () =>
				{
					if (collided)
					{
						collided = false;
						Use();
					}
					else
					{
						state = UsableState.Used;
						OnUsed?.Invoke();
					}
				};
			}
			else
			{
				state = UsableState.Using;
				t.DORotate(-rotation, tweenDuration, RotateMode.LocalAxisAdd).onComplete = () =>
				{
					//Debug.Log($"Rotation ended, collided = {collided}");
					if (collided)
					{
						collided = false;
						Use();
					}
					else
					{
						state = UsableState.NotUsed;
						OnUnused?.Invoke();
					}
				};
			}
		}
	}

	public void Unlock()
	{
		isLocked = false;
	}

	//private void OnCollisionEnter(Collision collision)
	//{
	//	if (state == UsableState.Using && collision.transform.CompareTag("Player"))
	//	{
	//		collided = true;
	//	}
	//}
}
