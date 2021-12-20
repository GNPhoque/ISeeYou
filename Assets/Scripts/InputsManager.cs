using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputsManager : MonoBehaviour
{
    [SerializeField]
    Inputs inputs;
    void Update()
    {
        inputs.movement = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        inputs.rotation = new Vector2(Input.GetAxisRaw("CameraHorizontal"), Input.GetAxisRaw("CameraVertical"));

        inputs.use = Input.GetButton("Use");
        inputs.useDown = Input.GetButtonDown("Use");
        inputs.useUp = Input.GetButtonUp("Use");

        inputs.jump = Input.GetButton("Jump");
        inputs.jumpDown = Input.GetButtonDown("Jump");
        inputs.jumpUp = Input.GetButtonUp("Jump");
    }
}
