using UnityEditor;
using UnityEngine;

using System.Collections;
using System.Collections.Generic;
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
	LayerMask IUsableMask;
	[SerializeField]
	CancelUseable cancelable;

	Image crosshair;
	Useable _target;
	Useable tmpTarget;
	float useDownTime;
	bool canceling;
	bool isCrosshairSet;

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
			if (canceling)
			{
				Cancel();
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

	void Cancel()
	{
		//Use btn up : quick use
		if (inputs.useUp)
		{
			cancelable.Use(gameObject);
			useDownTime = -1f;
			cancelable = null;
			canceling = false;
			return;
		}
		//Holding longer than delay : LongUse
		else if (useDownTime > longUseDelay)
		{
			cancelable.LongUse(gameObject);
			useDownTime = -1f;
			cancelable = null;
			canceling = false;
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
				cancelable = ((CancelUseable)tmpTarget).Getstate() == UsableState.NotUsed ? (CancelUseable)tmpTarget : null;
				if (tmpTarget is UseablePositionChange)
				{
					((CancelUseable)tmpTarget).Use(gameObject);
					return;
				}
			}
			tmpTarget.Use();
		}
		//Holding longer than delay : LongUse
		else if (useDownTime > longUseDelay)
		{
			useDownTime = -1f;
			if (tmpTarget is CancelUseable)
			{
				cancelable = ((CancelUseable)tmpTarget).Getstate() == UsableState.NotUsed ? (CancelUseable)tmpTarget : null;
				if (tmpTarget is UseablePositionChange)
				{
					((CancelUseable)tmpTarget).LongUse(gameObject);
					return;
				}
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
			canceling = true;
		}
	}

	void ChangeCrosshairState()
	{
		if (crosshair && crosshair.IsActive())
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

	public void SetCrosshair()
	{
		crosshair = GameObject.FindGameObjectWithTag("Crosshair").GetComponent<Image>();
		isCrosshairSet = true;
	}
}