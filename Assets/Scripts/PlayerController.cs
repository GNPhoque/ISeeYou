using Cinemachine;
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
	float moveSpeed;
	[SerializeField]
	float rotationSpeed;

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
			if (IsOwner) Camera.main.GetComponent<CinemachineVirtualCamera>().Follow = t;
		}
	}

	private void FixedUpdate()
	{
		if (IsOwner) //move
		{
			if (canMove && inputs.movement != Vector2.zero)
			{
				Move();

				Vector3 cameraForward = Camera.main.transform.forward;
				cameraForward = new Vector3(cameraForward.x, 0f, cameraForward.z);
				rb.MoveRotation(Quaternion.RotateTowards(rb.rotation, Quaternion.LookRotation(cameraForward), rotationSpeed));
			}
		}
	}

	private void Move()
	{
		Vector3 moveX = t.right * inputs.movement.x * moveSpeed;
		Vector3 moveZ = t.forward * inputs.movement.y * moveSpeed;
		Vector3 move = moveX + moveZ;
		rb.velocity = new Vector3(move.x, rb.velocity.y, move.z);
		//rb.velocity = new Vector3(inputs.movement.x * moveSpeed, rb.velocity.y, inputs.movement.y * moveSpeed); //OLD VERSION WITHOUT FORWARD
	}

	private void OnDestroy()
	{
		Destroy(playerListItem);
	}
}
