using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


public class GridPuzzlePiece : MonoBehaviour
{
	RectTransform rt;

	private void Awake()
	{
		rt = GetComponent<RectTransform>();
	}

	public void Move(int direction)
	{
		Vector2 dir= Vector2.zero;
		switch ((GridPuzzleDirection)direction)
		{
			case GridPuzzleDirection.Left:
				dir = Vector2.left;
				break;
			case GridPuzzleDirection.Up:
				dir = Vector2.up;
				break;
			case GridPuzzleDirection.Right:
				dir = Vector2.right;
				break;
			case GridPuzzleDirection.Down:
				dir = Vector2.down;
				break;
			default:
				break;
		}
		Debug.Log(direction);
		Debug.Log(dir);
		//EventSystem.current.RaycastAll()
		//Debug.Log(Physics2D.Raycast(rt.anchoredPosition + 102 * dir,));
		if (Physics2D.OverlapCircle(rt.anchoredPosition + 102 * dir, .02f))
		{
			rt.DOAnchorPos(rt.anchoredPosition + 102 * dir, .2f);
		}
	}
}
