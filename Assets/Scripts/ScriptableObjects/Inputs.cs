using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class Inputs : ScriptableObject
{
    public Vector2 movement;
    public Vector2 rotation;
    public bool use, useDown, useUp;
    public bool jump, jumpDown, jumpUp;
}
