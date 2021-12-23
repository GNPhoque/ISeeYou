using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class GameScene : NetworkBehaviour
{
	[SerializeField]
	GameObject crosshairCanvas;
	[SerializeField]
	Transform spawnPointHost;
	[SerializeField]
	Transform spawnPointClient;

	private void Awake()
	{
		Instantiate(crosshairCanvas);
	}

	public Transform GetSpawnPoint()
	{
		return IsHost ? spawnPointHost : spawnPointClient;
	}
}
