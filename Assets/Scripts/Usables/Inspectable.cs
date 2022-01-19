using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class Inspectable : Useable
{
	[SerializeField]
	Camera inspectorCamera;
	[SerializeField]
	Vector3 cameraPosition;
	[SerializeField]
	Vector3 cameraRotation;

	public override void Use()
	{
		inspectorCamera.transform.localPosition = cameraPosition;
		inspectorCamera.transform.localEulerAngles= cameraRotation;
		inspectorCamera.gameObject.SetActive(true);
	}
}
