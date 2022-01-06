using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using Unity.Netcode.Samples;
using UnityEngine;

[RequireComponent(typeof(NetworkObject))]
[RequireComponent(typeof(ClientNetworkTransform))]
public class PlayerController : NetworkBehaviour
{
	[SerializeField]
	GameObject playerListItemPrefab;
	
	[Header("Scriptable Objects")]
	[SerializeField]
	Inputs inputs;

	[Header("Components")]
	[SerializeField]
	PlayerInteraction playerInteraction;
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
	[SerializeField]
	Vector3 lobbyStartPosition;

	//Private Fields
	GameObject playerListItem;
	NetworkBehaviour sceneManager;
	Transform playerListContainer;
	Vector3 spawnPosition;
	bool canMove = true;

	private void Start()
	{
		NetworkManager.SceneManager.OnLoadComplete += SceneManager_OnLoadComplete;
		if (!IsHost)
		{
			lobbyStartPosition += new Vector3(4f, 0f, -3f);
		}
		spawnPosition = lobbyStartPosition;
		t.position = spawnPosition;
		playerListContainer = GameObject.FindGameObjectWithTag("PlayerListContainer").transform;
		playerListItem = Instantiate(playerListItemPrefab, playerListContainer);
	}

	private void SceneManager_OnLoadComplete(ulong clientId, string sceneName, UnityEngine.SceneManagement.LoadSceneMode loadSceneMode)
	{
		if (sceneName == "Game")
		{
			sceneManager = GameObject.FindGameObjectWithTag("SceneManager").GetComponent<NetworkBehaviour>();
			spawnPosition = ((GameScene)sceneManager).GetSpawnPoint().position;
			t.position = spawnPosition;
			if (IsOwner) Camera.main.GetComponent<CinemachineVirtualCamera>().Follow = t;
			playerInteraction.SetCrosshair();
		}
	}

	private void FixedUpdate()
	{
		if (IsOwner)
		{
			if (canMove && inputs.movement != Vector2.zero)
			{
				Move();
				RotateTowardsCameraForward();
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

	private void RotateTowardsCameraForward()
	{
		Vector3 cameraForward = Camera.main.transform.forward;
		cameraForward = new Vector3(cameraForward.x, 0f, cameraForward.z);
		rb.MoveRotation(Quaternion.RotateTowards(rb.rotation, Quaternion.LookRotation(cameraForward), rotationSpeed));
	}

	public override void OnDestroy()
	{
		Destroy(playerListItem);
	}

	private void OnTriggerEnter(Collider other)
	{
		if (other.CompareTag("SafetyNet"))
		{
			t.position = spawnPosition;
		}
	}
}
