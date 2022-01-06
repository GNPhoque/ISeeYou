using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LobbyUI : MonoBehaviour
{
	[SerializeField]
	RectTransform lobbyPanel;
	[SerializeField]
	RectTransform mainPanel;
	[SerializeField]
	GameObject settingsPanel;
	[SerializeField]
	Button startButton;
	[SerializeField]
	IntVariable playersConnected;
	[SerializeField]
	float panelTransitionDuration;

	bool isInLobby;

	private void Start()
	{
		//startButton.enabled = false;
		//playersConnected.onValueChanged += (int value) => { startButton.enabled = value == 2; };
	}

	public void MoveToLobby()
	{
		if (isInLobby)
		{
			lobbyPanel.DOAnchorPos(new Vector2(3000f, 0f), panelTransitionDuration);
			mainPanel.DOAnchorPos(new Vector2(0f, 0f), panelTransitionDuration);
		}
		else
		{
			lobbyPanel.DOAnchorPos(new Vector2(0f, 0f), panelTransitionDuration);
			mainPanel.DOAnchorPos(new Vector2(-3000f, 0f), panelTransitionDuration);
		}
		isInLobby = !isInLobby;
	}

	public void SettingsButton()
	{
		settingsPanel.SetActive(!settingsPanel.activeSelf);
	}

	public void ExitButton()
	{
		Application.Quit();
	}
}