using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class InspectorCamera : MonoBehaviour
{
	InputsManager inputsManager;
	CinemachineBrain camBrain;

	private void Awake()
	{
		inputsManager = NetworkManager.Singleton.GetComponent<InputsManager>();
		camBrain = Camera.main.GetComponent<CinemachineBrain>();
	}

	private void OnEnable()
	{
		inputsManager.enabled = false;
		camBrain.enabled = false;
	}

	void Update()
    {
		if (Input.GetMouseButtonDown(0) ||Input.GetButton("Use"))
		{
			inputsManager.enabled = true;
			camBrain.enabled = true;
			gameObject.SetActive(false);
		}
    }
}
