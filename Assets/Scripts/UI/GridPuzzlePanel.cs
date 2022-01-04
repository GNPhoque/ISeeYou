using Cinemachine;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public enum GridPuzzleDirection
{
	Left = 0,
	Up = 1,
	Right = 2,
	Down = 3
}

public class GridPuzzlePanel : MonoBehaviour
{
	[Header("Prefabs")]
	[SerializeField]
	GameObject gridPuzzleCell;
	[SerializeField]
	GameObject gridPuzzleTarget;
	[SerializeField]
	GameObject gridPuzzlePlayer;

	[Header("Grid Layouts")]
	[SerializeField]
	GridPuzzleLayout[] gridPuzzleLayouts;

	[Header("Scene References")]
	[SerializeField]
	Transform t;
	[SerializeField]
	Transform canvas;

	[Header("Audio")]
	[SerializeField]
	AudioSource source;
	[SerializeField]
	AudioClip bip;
	[SerializeField]
	AudioClip correct;
	[SerializeField]
	AudioClip incorrect;

	//Private Fields
	CinemachineBrain camBrain;
	InputsManager inputsManager;
	GameObject crosshair;

	GameObject player;
	GameObject target;
	RectTransform playerRectTransform;

	(int x, GameObject piece, Vector2 position)[] grid;
	int playerGridPositionIndex;
	int playerTargetGridPositionIndex;
	int selectedGridLayout;
	bool success;

	void Start()
	{
		SetReferences();
		SetGridCells();
		selectedGridLayout = Random.Range(0, gridPuzzleLayouts.Length);
		canvas.gameObject.SetActive(false);
	}

	void SetReferences()
	{
		if (!camBrain) camBrain = Camera.main.GetComponent<CinemachineBrain>();
		if (!crosshair) crosshair = GameObject.FindGameObjectWithTag("Crosshair");
		if (!inputsManager) inputsManager = NetworkManager.Singleton.gameObject.GetComponent<InputsManager>();
	}

	public void Display()
	{
		canvas.gameObject.SetActive(true);
		StartCoroutine("WaitDisplay");
	}

	IEnumerator WaitDisplay()
	{
		yield return new WaitForSeconds(.1f);
		SetPieces();
		SetWalls();

		success = false;
		camBrain.enabled = false;
		inputsManager.enabled = false;
		crosshair.SetActive(false);
	}

	void SetGridCells()
	{
		grid = new (int x, GameObject piece, Vector2 position)[49];
		for (int i = 0; i < grid.Length; i++)
		{
			GameObject go = Instantiate(gridPuzzleCell, t);
			grid[i] = (i, go, go.transform.position);
		}
	}

	public void SetPieces()
	{
		for (int i = 0; i < grid.Length; i++)
		{
			grid[i].position = grid[i].piece.GetComponent<RectTransform>().anchoredPosition;
		}
		GetComponent<GridLayoutGroup>().enabled = false;

		player = Instantiate(gridPuzzlePlayer, t);
		playerRectTransform = player.GetComponent<RectTransform>();
		playerGridPositionIndex = gridPuzzleLayouts[selectedGridLayout].playerStart;
		Debug.Log(grid[playerGridPositionIndex].piece.GetComponent<RectTransform>().anchoredPosition);
		playerRectTransform.anchoredPosition = grid[playerGridPositionIndex].piece.GetComponent<RectTransform>().anchoredPosition + new Vector2(2.5f, -2.5f);

		target = Instantiate(gridPuzzleTarget, t);
		Debug.Log(grid[gridPuzzleLayouts[selectedGridLayout].target].piece.GetComponent<RectTransform>().anchoredPosition);
		target.GetComponent<RectTransform>().anchoredPosition = grid[gridPuzzleLayouts[selectedGridLayout].target].piece.GetComponent<RectTransform>().anchoredPosition + new Vector2(2.5f, -2.5f);
	}

	void SetWalls()
	{
		for (int i = 0; i < grid.Length; i++)
		{
			grid[i].piece.tag = gridPuzzleLayouts[0].walkables.Contains(i) ? "GridCell" : "Untagged";
		}
	}

	public void Move(int direction)
	{
		PlaySound();
		//Get direction
		Vector2 dir = Vector2.zero;
		switch ((GridPuzzleDirection)direction)
		{
			case GridPuzzleDirection.Left:
				if (playerGridPositionIndex % 7 != 0)
				{
					dir = Vector2.left;
					playerTargetGridPositionIndex = playerGridPositionIndex - 1;
				}
				break;
			case GridPuzzleDirection.Up:
				if (playerGridPositionIndex > 6)
				{
					dir = Vector2.up;
					playerTargetGridPositionIndex = playerGridPositionIndex - 7;
				}
				break;
			case GridPuzzleDirection.Right:
				if (playerGridPositionIndex % 7 != 6)
				{
					dir = Vector2.right;
					playerTargetGridPositionIndex = playerGridPositionIndex + 1;
				}
				break;
			case GridPuzzleDirection.Down:
				if (playerGridPositionIndex < 42)
				{
					dir = Vector2.down;
					playerTargetGridPositionIndex = playerGridPositionIndex + 7;
				}
				break;
			default:
				break;
		}

		//Move
		if (dir != Vector2.zero)
		{
			if (grid[playerTargetGridPositionIndex].piece.CompareTag("GridCell"))
			{
				playerRectTransform.DOAnchorPos(grid[playerTargetGridPositionIndex].position + new Vector2(2.5f,-2.5f), .2f);
				playerGridPositionIndex = playerTargetGridPositionIndex;
				if (playerGridPositionIndex == gridPuzzleLayouts[selectedGridLayout].target)
				{
					success = true;
					PlaySound("Correct");
				}
				return;
			}
		}
		PlaySound("Incorrect");
	}

	private void PlaySound(string sound = "")
	{
		switch (sound)
		{
			case "Correct":
				source.clip = correct;
				StartCoroutine("WaitEndSound");
				break;
			case "Incorrect":
				source.clip = incorrect;
				StartCoroutine("WaitEndSound");
				break;
			default:
				source.clip = bip;
				StartCoroutine("WaitEndSound");
				break;
		}
	}

	IEnumerator WaitEndSound()
	{
		source.Play();
		yield return new WaitForSeconds(source.clip.length);
		if (success)
		{
			Exit(); 
		}
	}

	public void Exit()
	{
		Destroy(player);
		Destroy(target);
		camBrain.enabled = true;
		inputsManager.enabled = true;
		crosshair.SetActive(true);
		canvas.gameObject.SetActive(false);
	}
}
