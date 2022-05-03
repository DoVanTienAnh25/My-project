using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    InputManager inputManager;
    PlayerManager playerManager;

    public Transform cameraPivot;
    public Camera cameraPlayer;

    [Header("Camera Follow")]
    public GameObject player;
    public Transform aimingPivot;

    Vector3 cameraFollowVelocity = Vector3.zero;
    Vector3 targetPosition;
    Vector3 cameraRotation;
    Quaternion targetRotation;

    [Header("Camera Speed")]
    public float cameraSmoothTime = 0.2f;
    public float aimCameraSmoothTime = 3f;

    float lookAmountHorizontal;
    float lookAmountVertical;
    float maxinumPivotAngle = 15;
    float mininumPivotAngle = -15;

    private void Awake()
    {
        playerManager = player.GetComponent<PlayerManager>();
        inputManager = player.GetComponent<InputManager>();
    }

    public void HandleAllCameraMovement()
    {
        FollowPlayer();
        RotateCamera();
    }

    private void FollowPlayer()
    {
        if(playerManager.isAiming)
        {
            targetPosition = Vector3.SmoothDamp(transform.position, aimingPivot.transform.position, ref cameraFollowVelocity, cameraSmoothTime * Time.deltaTime);
            transform.position = targetPosition;
        }
        else
        {
            targetPosition = Vector3.SmoothDamp(transform.position, player.transform.position, ref cameraFollowVelocity, cameraSmoothTime * Time.deltaTime);
            transform.position = targetPosition;
        }
        
    }

    private void RotateCamera()
    {
        if (playerManager.isAiming)
        {
            cameraPivot.localRotation = Quaternion.Euler(0, 0, 0);

            lookAmountVertical = lookAmountVertical + (inputManager.horizontalCameraInput);
            lookAmountHorizontal = lookAmountHorizontal - (inputManager.verticalCameraInput);
            lookAmountHorizontal = Mathf.Clamp(lookAmountHorizontal, mininumPivotAngle, maxinumPivotAngle);

            cameraRotation = Vector3.zero;
            cameraRotation.y = lookAmountVertical;
            targetRotation = Quaternion.Euler(cameraRotation);
            targetRotation = Quaternion.Slerp(transform.rotation, targetRotation, aimCameraSmoothTime);
            transform.rotation = targetRotation;

            cameraRotation = Vector3.zero;
            cameraRotation.x = lookAmountHorizontal;
            targetRotation = Quaternion.Euler(cameraRotation);
            targetRotation = Quaternion.Slerp(cameraPivot.localRotation, targetRotation, aimCameraSmoothTime);
            cameraPlayer.transform.localRotation = targetRotation;
        }
        else
        {
            cameraPlayer.transform.localRotation = Quaternion.Euler(0, 0, 0);

            lookAmountVertical = lookAmountVertical + (inputManager.horizontalCameraInput);
            lookAmountHorizontal = lookAmountHorizontal - (inputManager.verticalCameraInput);
            lookAmountHorizontal = Mathf.Clamp(lookAmountHorizontal, mininumPivotAngle, maxinumPivotAngle);

            cameraRotation = Vector3.zero;
            cameraRotation.y = lookAmountVertical;
            targetRotation = Quaternion.Euler(cameraRotation);
            targetRotation = Quaternion.Slerp(transform.rotation, targetRotation, cameraSmoothTime);
            transform.rotation = targetRotation;

            cameraRotation = Vector3.zero;
            cameraRotation.x = lookAmountHorizontal;
            targetRotation = Quaternion.Euler(cameraRotation);
            targetRotation = Quaternion.Slerp(cameraPivot.localRotation, targetRotation, cameraSmoothTime);
            cameraPivot.localRotation = targetRotation;
        }
        
    }
}
