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

	[Header("Components")]
	[SerializeField]
	Transform t;
	[SerializeField]
	Rigidbody rb;

	[Header("Fields")]
	[SerializeField]
	float spawnXRange;
	[SerializeField]
	float spawnZRange;
	[SerializeField]
	float speed;

	//Private Fields
	GameObject playerListItem;
	NetworkBehaviour sceneManager;
	Transform playerListContainer;
	bool canMove = true;

	private void Start()
	{
		NetworkManager.SceneManager.OnLoadComplete += SceneManager_OnLoadComplete;
		t.position = new Vector3(Random.Range(-spawnXRange, spawnXRange), 4f, Random.Range(-spawnZRange, spawnZRange));
		playerListContainer = GameObject.FindGameObjectWithTag("PlayerListContainer").transform;
		playerListItem = Instantiate(playerListItemPrefab, playerListContainer);
	}

	private void SceneManager_OnLoadComplete(ulong clientId, string sceneName, UnityEngine.SceneManagement.LoadSceneMode loadSceneMode)
	{
		if (sceneName == "Game")
		{
			sceneManager = GameObject.FindGameObjectWithTag("SceneManager").GetComponent<NetworkBehaviour>();
			t.position = ((GameScene)sceneManager).GetSpawnPoint().position;
		}
	}

	private void Update()
	{

	}

	private void FixedUpdate()
	{
		if (IsClient) //move
		{
			if (canMove && inputs.movement != Vector2.zero)
			{
				rb.velocity = new Vector3(inputs.movement.x * speed, rb.velocity.y, inputs.movement.y * speed);
			}
		}
	}

	private void OnDestroy()
	{
		Destroy(playerListItem);
	}
}
