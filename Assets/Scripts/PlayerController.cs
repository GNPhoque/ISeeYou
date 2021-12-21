using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class PlayerController : NetworkBehaviour
{
	[SerializeField]
	GameObject playerListItemPrefab;
	
	[Header("Scriptable Objects")]
	[SerializeField]
	Inputs inputs;

	[Header("Fields")]
	[SerializeField]
	float spawnXRange;
	[SerializeField]
	float spawnZRange;

	//Private Fields
	NetworkBehaviour sceneManager;
	Transform playerListContainer;
	GameObject playerListItem;

	private void Start()
	{
		NetworkManager.SceneManager.OnLoadComplete += SceneManager_OnLoadComplete;
		transform.position = new Vector3(Random.Range(-spawnXRange, spawnXRange), 0f, Random.Range(-spawnZRange, spawnZRange));
		playerListContainer = GameObject.FindGameObjectWithTag("PlayerListContainer").transform;
		playerListItem = Instantiate(playerListItemPrefab, playerListContainer);
	}

	private void SceneManager_OnLoadComplete(ulong clientId, string sceneName, UnityEngine.SceneManagement.LoadSceneMode loadSceneMode)
	{
		if (sceneName == "Game")
		{
			sceneManager = GameObject.FindGameObjectWithTag("SceneManager").GetComponent<NetworkBehaviour>();
			transform.position = ((GameScene)sceneManager).GetSpawnPoint().position;
		}
	}

	private void Update()
	{
		if (IsClient)
		{
			if (inputs.movement != Vector2.zero)
			{

			}
		}
		if (IsServer)
		{

		}
	}

	private void OnDestroy()
	{
		Destroy(playerListItem);
	}
}
