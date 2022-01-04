using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Netcode;
using Unity.Netcode.Transports.UNET;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LobbyScene : NetworkBehaviour
{
	#region Fields
	[Header("UI")]
	[SerializeField]
	LobbyUI lobbyUI;
	[SerializeField]
	TMP_Text logText;
	[SerializeField]
	TMP_InputField joinCodeInputField;
	[SerializeField]
	Transform playerListContainer;
	[SerializeField]
	RelayManager relayManager;

	[Header("ScriptableObjects")]
	[SerializeField]
	IntVariable playersConnected;

	//Private fields

	bool connected;
	bool waitingClientDisconnect;
	float logClearTime;

	//Properties
	public Transform PlayerListContainer { get => playerListContainer; }
	string logMessage
	{
		set { logText.text += $"{value}\n"; StartCoroutine(ClearLogMessage()); }
	}

	#endregion

	private void Start()
	{
		logMessage = "Log messages will be displayed here...";
		playersConnected.value = 0;
		NetworkManager.OnClientConnectedCallback += Singleton_OnClientConnectedCallback;
		NetworkManager.OnClientDisconnectCallback += Singleton_OnClientDisconnectCallback;
		NetworkManager.OnServerStarted += Singleton_OnServerStarted;
	}

	#region EVENTS
	private void Singleton_OnServerStarted()
	{
		logMessage = "Server started";
	}

	private void Singleton_OnClientConnectedCallback(ulong obj)
	{
		//CONNECTION SUCCESS
		if (NetworkManager.LocalClientId == obj)
		{
			connected = true;
			lobbyUI.MoveToLobby();
			logMessage = "Connected to host!";
		}
		if (NetworkManager.IsHost)
		{
			playersConnected.value++;
		}
		logMessage = $"{obj} has been connected";
	}

	private void Singleton_OnClientDisconnectCallback(ulong obj)
	{
		logMessage = $"{obj} disconnected";
		if (NetworkManager.LocalClientId == obj)
		{
			connected = false;
		}
		if (IsHost)
		{
			playersConnected.value--;
			if (waitingClientDisconnect)
			{
				lobbyUI.MoveToLobby();
				NetworkManager.Shutdown();
			}
		}
	}
	#endregion

	#region Buttons
	public async void MainHostButton()
	{
		waitingClientDisconnect = false;

		if (relayManager.IsRelayEnabled)
		{
			await relayManager.SetupRelay();
		}

		if (!NetworkManager.StartHost())
		{
			Debug.Log("Could not start host...");
			NetworkManager.Shutdown();
		}
	}

	public async void MainJoinButton()
	{

		if (relayManager.IsRelayEnabled)
		{
			await relayManager.JoinRelay(joinCodeInputField.text);
		}

		if (NetworkManager.StartClient())
		{
			logMessage = "Connecting to Host...";
			StartCoroutine(ConnectionTimeout());
		}
		else
		{
			logMessage = "Could not start client...";
			NetworkManager.Shutdown();
		}
	}

	public void LobbyBackButton()
	{
		playersConnected.value = 0;
		if (IsHost)
		{
			DisconnectClientRpc();
		}
		else
		{
			NetworkManager.Shutdown();
			lobbyUI.MoveToLobby();
		}
	}

	public void LobbyStartButton()
	{
		StopAllCoroutines();
		NetworkManager.SceneManager.LoadScene("Game", LoadSceneMode.Single);
	}
	#endregion

	#region RPC
	[ClientRpc]
	private void DisconnectClientRpc()
	{
		if (IsHost)
		{
			waitingClientDisconnect = true;
			return;
		}
		logMessage = "Host disconnected";
		lobbyUI.MoveToLobby();
		NetworkManager.Shutdown();
	}
	#endregion

	#region Coroutines
	IEnumerator ConnectionTimeout()
	{
		yield return new WaitForSeconds(5f);
		if (!connected)
		{
			logMessage = "Could not connect to host check the IP and port fields, and check host has forwarded the port.";
			NetworkManager.Shutdown();
		}
	}

	IEnumerator ClearLogMessage()
	{
		logClearTime = Time.time + 10f;
		yield return new WaitForSeconds(10f);
		if (logClearTime < Time.time)
		{
			logText.text = "";
		}
	}
	#endregion
}