using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Keypad : MonoBehaviour
{
	[SerializeField]
	UnityEvent OnSuccess;
	[SerializeField]
	UnityEvent OnFail;
	[SerializeField]
	AudioSource source;
	[SerializeField]
	AudioClip bip;
	[SerializeField]
	AudioClip correct;
	[SerializeField]
	AudioClip incorrect;

    string secretCode;
    string code;
	bool isActive;

	private void Start()
	{
		code = "";
		SetSecretCode("0000");
	}

	public void SetSecretCode(string secret)
	{
		secretCode = secret;
		isActive = true;
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
			PlaySound("Correct");
			OnSuccess?.Invoke();
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
				source.PlayOneShot(correct);
				break;
			case "Incorrect":
				source.PlayOneShot(incorrect);
				break;
			default:
				source.PlayOneShot(bip);
				break;
		}
	}
}
