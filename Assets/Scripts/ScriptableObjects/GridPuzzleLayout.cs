using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class GridPuzzleLayout : ScriptableObject
{
    public List<int> walkables;
    public int playerStart;
    public int target;
}
