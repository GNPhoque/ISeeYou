using UnityEditor;
using UnityEngine;

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class PlayerInteraction : MonoBehaviour
{
	[SerializeField]
	Inputs inputs;
	[SerializeField]
	float maxDistance;
	[SerializeField]
	float longUseDelay;
	[SerializeField]
	Image crosshair;
	[SerializeField]
	LayerMask IUsableMask;
	[SerializeField]
	CancelUseable cancelable;

	Useable _target;
	Useable tmpTarget;
	float useDownTime;
	bool usingCancel;

	Useable target { get => _target; set { _target = value; ChangeCrosshairState(); } }

	private void Update()
	{
		FindTarget();
		UseTarget();
		if (crosshair)
		{
			ChangeCrosshairState(); 
		}
	}

	bool FindTarget()
	{
		//if not already using
		if (useDownTime == -1f)
		{
			RaycastHit hit;
			if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit, maxDistance, IUsableMask))
			{
				target = hit.collider.gameObject.GetComponent<Useable>() ?? null;
			}
			else target = null; 
		}
		return target;
	}

	void UseTarget()
	{
		//Checking for long press
		if (useDownTime >= 0f)
		{
			if (usingCancel)
			{
				OnUseCancelable();
			}
			else if (tmpTarget != null)
			{
				OnUseTarget();
			}
			else
			{
				useDownTime = -1f;
				tmpTarget = null;
			}
		}
		//Checking for btn use down
		else if (inputs.useDown)
		{
			OnUseDown();
		}
	}

	void OnUseCancelable()
	{
		//Use btn up : quick use
		if (inputs.useUp)
		{
			useDownTime = -1f;
			cancelable = null;
			usingCancel = false;
			cancelable.Use();
			return;
		}
		//Holding longer than delay : LongUse
		else if (useDownTime > longUseDelay)
		{
			useDownTime = -1f;
			cancelable = null;
			usingCancel = false;
			cancelable.LongUse();
			return;
		}
	}

	void OnUseTarget()
	{
		useDownTime += Time.deltaTime;
		//Use btn up : quick use
		if (inputs.useUp)
		{
			useDownTime = -1f;
			if (tmpTarget is CancelUseable)
			{
				if (((CancelUseable)tmpTarget).Getstate() != UsableState.NotUsed)
					cancelable = null;
				else cancelable = (CancelUseable)tmpTarget;
			}
			tmpTarget.Use();
		}
		//Holding longer than delay : LongUse
		else if (useDownTime > longUseDelay)
		{
			useDownTime = -1f;
			if (tmpTarget is CancelUseable)
			{
				if (((CancelUseable)tmpTarget).Getstate() == UsableState.Used)
					cancelable = null;
				else cancelable = (CancelUseable)tmpTarget;
			}
			tmpTarget.LongUse();
		}
	}

	void OnUseDown()
	{
		//use down & target present
		if (target != null)
		{
			useDownTime = 0f;
			tmpTarget = target;
		}
		//use down & target not present : use cancelable
		else if (cancelable)
		{
			if (cancelable.Getstate() != UsableState.Used)
			{
				cancelable = null;
				return;
			}
			useDownTime = 0f;
			usingCancel = true;
		}
	}

	void ChangeCrosshairState()
	{
		if (target != null)
		{
			crosshair.material.color = Color.red;
		}
		else
		{
			crosshair.material.color = Color.white;
		}
	}
}