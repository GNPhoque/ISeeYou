using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class GameScene : NetworkBehaviour
{
	[SerializeField]
	Transform spawnPointHost;
	[SerializeField]
	Transform spawnPointClient;

	public Transform GetSpawnPoint()
	{
		return IsHost ? spawnPointHost : spawnPointClient;
	}
}
