using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class CameraController : MonoBehaviour
{
    [SerializeField] private float mouseSensitivity = 5f;
    [SerializeField] private float distanceFromTarget = 60f;
    [SerializeField] private float pitchMin = 20f;
    [SerializeField] private float pitchMax = 50f;
    [SerializeField] private float rotationSmoothTime = 0.5f;
    [SerializeField] private Vector3 rotationSmoothVelocity;
    
    private float yaw;
    private float pitch;
    private Transform target;
    private Vector3 currentRotation;
    private PlayerInput playerInput;
    private WorldGenerator worldGenerator;
 

    private void Start()
    {
        worldGenerator = FindObjectOfType<WorldGenerator>();
        playerInput = FindObjectOfType<PlayerInput>();
        target = new GameObject("Pivot").transform;
        SetTargetPosition();
    }

    private void Update()
    {
        if (playerInput.Generate)
        {
            SetTargetPosition();
        }

        if (playerInput.GenerateTexture)
        {
            SetTargetPosition();
        }

        if (playerInput.GenerateMFDOOM)
        {
            SetTargetPosition();
        }
    }

    private void SetTargetPosition()
    {
        var targetPos = new Vector3(worldGenerator.Width, 0f, worldGenerator.Height);
        target.SetPositionAndRotation(targetPos,Quaternion.identity );
    }

    private void LateUpdate()
    {
        if(!target) return;
        
        yaw = playerInput.CameraInput.x * mouseSensitivity;
        pitch = playerInput.CameraInput.y * mouseSensitivity;
        pitch = Mathf.Clamp(pitch, pitchMin, pitchMax);
        Vector3 targetRotation = new Vector3(pitch, yaw);
        currentRotation = Vector3.SmoothDamp(currentRotation, targetRotation, ref rotationSmoothVelocity, rotationSmoothTime);
        transform.eulerAngles = currentRotation;
        transform.position = target.position - transform.forward * distanceFromTarget;

    }
    
}
