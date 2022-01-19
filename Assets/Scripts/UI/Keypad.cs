using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.Events;
using Cinemachine;

public class Keypad : MonoBehaviour
{
	[Header("Events")]
	[SerializeField]
	UnityEvent OnSuccess;
	[SerializeField]
	UnityEvent OnFail;

	[Header("Audio")]
	[SerializeField]
	AudioSource source;
	[SerializeField]
	AudioClip bip;
	[SerializeField]
	AudioClip correct;
	[SerializeField]
	AudioClip incorrect;

	//Private Fields
	CinemachineBrain camBrain;
	InputsManager inputsManager;
	GameObject crosshair;

    string secretCode;
    string code;
	bool isActive;
	bool success;

	private void Start()
	{
		SetReferences();
		gameObject.SetActive(false);
	}

	public void SetEvents(UnityEvent success, UnityEvent fail)
	{
		OnSuccess = success;
		OnFail = fail;
	}

	public void SetSecretCode(string secret)
	{
		code = "";
		secretCode = secret;
		success = false;
		isActive = true;

		camBrain.enabled = false;
		inputsManager.enabled = false;
		crosshair.SetActive(false);
		Cursor.visible = true;
		Cursor.lockState = CursorLockMode.Confined;
		gameObject.SetActive(true);
	}

	private void SetReferences()
	{
		if (!camBrain) camBrain = Camera.main.GetComponent<CinemachineBrain>();
		if (!crosshair) crosshair = GameObject.FindGameObjectWithTag("Crosshair");
		if (!inputsManager) inputsManager = NetworkManager.Singleton.gameObject.GetComponent<InputsManager>();
	}

	public void OnButtonPress(int button)
	{
		if (isActive)
		{
			PlaySound();
			if (button == -1)//GREEN
			{
				Invoke("PlayGreenSound", .25f);                
			}
			else if (button == -2)//RED
			{
				code = "";
			}
			else//NUMBER
			{
				code += button;
			} 
		}
	}

	private void PlayGreenSound()
	{
		if (code == secretCode)
		{
			success = true;
			PlaySound("Correct");
		}
		else
		{
			PlaySound("Incorrect");
			code = "";
			OnFail?.Invoke();
		}
	}

	private void PlaySound(string sound = "")
	{
		switch (sound)
		{
			case "Correct":
				source.clip = correct;
				StartCoroutine("WaitEndSound");
				break;
			case "Incorrect":
				source.clip = incorrect;
				StartCoroutine("WaitEndSound");
				break;
			default:
				source.clip = bip;
				StartCoroutine("WaitEndSound");
				break;
		}
	}

	IEnumerator WaitEndSound()
	{
		source.Play();
		yield return new WaitForSeconds(source.clip.length);
		if (success)
		{
			OnSuccess?.Invoke();
			Exit();
		}
	}

	public void Exit()
	{
		isActive = false;

		camBrain.enabled = true;
		inputsManager.enabled = true;
		crosshair.SetActive(true);
		Cursor.visible = false;
		Cursor.lockState = CursorLockMode.Locked;
		gameObject.SetActive(false);
	}
}
