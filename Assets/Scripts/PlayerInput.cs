using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    
    public Vector2 CameraInput;
    public bool Generate;
    public bool GenerateTexture;
    public bool GenerateMFDOOM;
    private void Update()
    {
        CameraInput.x += Input.GetAxis("Mouse X");
        CameraInput.y += Input.GetAxis("Mouse Y");
        Generate = Input.GetButtonDown("Fire1");
        GenerateTexture = Input.GetButtonDown("Fire2");
        GenerateMFDOOM = Input.GetButtonDown("Fire3");


    }

}
