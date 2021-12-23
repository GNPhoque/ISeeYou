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
		code = "";
	}

	public void SetEvents(UnityEvent success, UnityEvent fail)
	{
		OnSuccess = success;
		OnFail = fail;
	}

	public void SetSecretCode(string secret)
	{
		SetReferences();

		code = "";
		secretCode = secret;
		success = false;
		isActive = true;

		camBrain.enabled = false;
		inputsManager.enabled = false;
		crosshair.SetActive(false);
		gameObject.SetActive(true);
	}

	private void SetReferences()
	{
		if (!camBrain) camBrain = Camera.main.GetComponent<CinemachineBrain>();
		if (!inputsManager) inputsManager = NetworkManager.Singleton.gameObject.GetComponent<InputsManager>();
		if (!crosshair) crosshair = GameObject.FindGameObjectWithTag("Crosshair");
	}

	public void OnButtonPress(int button)
	{
		if (isActive)
		{
			if (button == -1)//GREEN
			{
				PlaySound();
				Invoke("PlayGreenSound", .25f);                
			}
			else if (button == -2)//RED
			{
				PlaySound();
				code = "";
			}
			else
			{
				PlaySound();
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
				//source.PlayOneShot(correct);
				StartCoroutine("WaitEndSound");
				break;
			case "Incorrect":
				source.clip = incorrect;
				//source.PlayOneShot(incorrect);
				StartCoroutine("WaitEndSound");
				break;
			default:
				source.clip = bip;
				//source.PlayOneShot(bip);
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
		gameObject.SetActive(false);
	}
}
