using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using System.Linq;

public class Carryable : Useable
{
	[SerializeField]
	Inventory inventory;

	GameObject localPlayer;
	Transform originalParent;
	Transform playerHand;
	Collider col;

	Vector3 originalPos;
	Quaternion originalRot;
	Vector3 originalScale;

	private void Awake()
	{
		col = GetComponent<Collider>();
		originalParent = transform.parent;
		originalPos = transform.position;
		originalRot = transform.rotation;
		originalScale = transform.localScale;
	}

	public override void Use()
	{
		playerHand = Camera.main.GetComponentsInChildren<Transform>().First(x => x.gameObject.name == "Hand");
		originalPos = transform.position;
		originalRot = transform.rotation;
		originalScale= transform.localScale;

		col.enabled = false;
		transform.SetParent(playerHand, true);
		transform.localPosition = Vector3.zero;
		transform.localRotation = Quaternion.identity;

		inventory.CarryNewItem(this);
	}

	public void ResetObject()
	{
		transform.SetParent(originalParent, true);
		col.enabled = true;
		transform.position = originalPos;
		transform.rotation = originalRot;
		transform.localScale = originalScale;
	}
}
