using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class IntVariable : ScriptableObject
{
	[SerializeField]
	int _value;

	public event Action<int> onValueChanged;
	public int value { get { return _value; } set { _value = value; onValueChanged?.Invoke(value); } }
}
