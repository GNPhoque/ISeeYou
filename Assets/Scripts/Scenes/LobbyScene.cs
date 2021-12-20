using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Netcode;
using Unity.Netcode.Transports.UNET;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LobbyScene : MonoBehaviour
{
	#region Fields
	[Header("UI")]
	[SerializeField]
	LobbyUI lobbyUI;
	[SerializeField]
	TMP_Text logText;
	[SerializeField]
	TMP_InputField ipInputField;
	[SerializeField]
	TMP_InputField portInputField;
	[SerializeField]
	Transform playerListContainer;

	[Header("ScriptableObjects")]
	[SerializeField]
	IntVariable playersConnected;

	//Private fields
	bool connected;
	float logClearTime;

	//Properties
	public Transform PlayerListContainer { get => playerListContainer; }
	string logMessage
	{
		set { logText.text += $"{value}\n"; StartCoroutine(ClearLogMessage()); }
	}

	#endregion

	////Singleton
	//private static LobbyScene _singleton;
	//public static LobbyScene singleton { get { return _singleton; } }
	//void Awake()
	//{
	//	if (_singleton != null && _singleton != this)
	//		Destroy(gameObject);

	//	_singleton = this;
	//}

	private void Start()
	{
		logMessage = "Log messages will be displayed here...";
		playersConnected.value = 0;
		NetworkManager.Singleton.OnClientConnectedCallback += Singleton_OnClientConnectedCallback;
		NetworkManager.Singleton.OnClientDisconnectCallback += Singleton_OnClientDisconnectCallback;
		NetworkManager.Singleton.OnServerStarted += Singleton_OnServerStarted;
	}

	#region EVENTS
	private void Singleton_OnServerStarted()
	{
		logMessage = "Server started";
	}

	private void Singleton_OnClientConnectedCallback(ulong obj)
	{
		if (NetworkManager.Singleton.LocalClientId == obj)
		{
			connected = true;
			lobbyUI.MoveToLobby();
			logMessage = "Connected to host!";
		}
		if (NetworkManager.Singleton.IsHost)
		{
			playersConnected.value++;
		}
		logMessage = $"{obj} has been connected";
	}

	private void Singleton_OnClientDisconnectCallback(ulong obj)
	{
		logMessage = $"{obj} disconnected";
		if (NetworkManager.Singleton.LocalClientId == obj)
		{
			connected = false;
		}
		if (NetworkManager.Singleton.ServerClientId == obj)
		{
			logMessage = "Host disconnected, closing connection...";
			LobbyBackButton();
		}
		if (NetworkManager.Singleton.IsHost)
		{
			playersConnected.value--;
		}
	}
	#endregion

	#region Buttons
	public void MainHostButton()
	{
		NetworkManager.Singleton.GetComponent<UNetTransport>().ServerListenPort = int.Parse(string.IsNullOrEmpty(portInputField.text) ? "7777" : portInputField.text);
		if (!NetworkManager.Singleton.StartHost())
		{
			Debug.Log("Could not start host...");
			NetworkManager.Singleton.Shutdown();
		}
	}

	public void MainJoinButton()
	{
		NetworkManager.Singleton.GetComponent<UNetTransport>().ConnectAddress = string.IsNullOrEmpty(ipInputField.text) ? "127.0.0.1" : ipInputField.text;
		NetworkManager.Singleton.GetComponent<UNetTransport>().ConnectPort = int.Parse(string.IsNullOrEmpty(portInputField.text) ? "7777" : portInputField.text);
		if (NetworkManager.Singleton.StartClient())
		{
			logMessage = "Connecting to Host...";
			StartCoroutine(ConnectionTimeout());
		}
		else
		{
			logMessage = "Could not start client...";
			NetworkManager.Singleton.Shutdown();
		}
	}


	public void LobbyBackButton()
	{
		lobbyUI.MoveToLobby();
		playersConnected.value = 0;
		ShutdownServerRpc();
	}

	public void LobbyStartButton()
	{
		StopAllCoroutines();
		NetworkManager.Singleton.SceneManager.LoadScene("Game", LoadSceneMode.Single);
	}
	#endregion

	#region RPC
	[ServerRpc]
	void ShutdownServerRpc()
	{
		if (NetworkManager.Singleton.IsHost)
		{
			logMessage = "Host disconnected";
		}
		logMessage = "Shutting down connection.";
		NetworkManager.Singleton.Shutdown();
	}
	#endregion

	#region Coroutines
	IEnumerator ConnectionTimeout()
	{
		yield return new WaitForSeconds(5f);
		if (!connected)
		{
			logMessage = "Could not connect to host check the IP and port fields, and check host has forwarded the port.";
			NetworkManager.Singleton.Shutdown();
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
