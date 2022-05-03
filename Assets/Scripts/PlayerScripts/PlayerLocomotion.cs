using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLocomotion : MonoBehaviour
{
    InputManager inputManager;
    PlayerManager playerManager;
    AnimatorManager animatorManager;
    Rigidbody playerRigibody;
    CameraManager cameraManager;

    [Header("Camera Transform")]
    public Transform cameraHolderTransform;

    [Header("Movement Speed")]
    public float rotaionSpeed =  3.5f;
    public float walkingSpeed = 1.5f;
    public float runningSpeed = 5;

    [Header("Rotation Variables")]
    Quaternion targetRotaion;
    Quaternion playerRotation;
    Vector3 moveDirection;

    [Header("Falling")]
    public float inAirTimer;
    public float leapingVelocity;
    public float fallingVelocity;
    public float rayCastHeightOffSet = 0.5f;
    public LayerMask groundLayer;

    [Header("Movement Flag")]
    public bool isGrounded;
    public bool isJumping;

    [Header("Jump Speeds")]
    public float gravityIntensity = -15;
    public float jumpHeight = 3;

    private void Awake()
    {
        playerManager = GetComponent<PlayerManager>();
        inputManager = GetComponent<InputManager>();
        playerRigibody = GetComponent<Rigidbody>();
        animatorManager = GetComponent<AnimatorManager>();
        cameraManager = FindObjectOfType<CameraManager>();
    }

    public void HandleAllLocomtion()
    {
        HandleFallingAndLanding();

        if (isJumping)
            return;

        if (playerManager.isFalling)
            return;

        HandleRotation();
        HandleMovement();
    }

    private void HandleMovement()
    {
        moveDirection = cameraManager.cameraPlayer.transform.forward * inputManager.verticalMovementInput;
        moveDirection = moveDirection + cameraManager.cameraPlayer.transform.right * inputManager.horizontalMovementInput;
        moveDirection.Normalize();
        moveDirection.y = 0;

        if (playerManager.isPerformingAction)
        {
            moveDirection = moveDirection * 0;
        }
        else
        {
            if (inputManager.runInput)
            {
                moveDirection = moveDirection * runningSpeed;
            }

            else
            {
                moveDirection = moveDirection * walkingSpeed;
            }
        }
        

        Vector3 movementVelocity = moveDirection;
        playerRigibody.velocity = movementVelocity;
    }

    private void HandleRotation()
    {
        if (playerManager.isAiming)
        {
            targetRotaion = Quaternion.Euler(0, cameraHolderTransform.eulerAngles.y, 0);
            playerRotation = Quaternion.Slerp(transform.rotation, targetRotaion, rotaionSpeed * Time.deltaTime);
            transform.rotation = playerRotation;
        }
        else
        {
            targetRotaion = Quaternion.Euler(0, cameraHolderTransform.eulerAngles.y, 0);
            playerRotation = Quaternion.Slerp(transform.rotation, targetRotaion, rotaionSpeed * Time.deltaTime);

            if (inputManager.horizontalMovementInput != 0 || inputManager.verticalMovementInput != 0)
            {
                transform.rotation = playerRotation;
            }
        }
        
    }

    private void HandleFallingAndLanding()
    {
        RaycastHit hit;
        Vector3 rayCastOrigin = transform.position;
        Vector3 targetPosition;
        rayCastOrigin.y = rayCastOrigin.y + rayCastHeightOffSet;
        targetPosition = transform.position;

        if (!isGrounded && !isJumping)
        {
            if (!playerManager.isFalling)
            {
                animatorManager.PlayTargetAnimation("Falling", true);
            }

            inAirTimer = inAirTimer + Time.deltaTime;
            playerRigibody.AddForce(transform.forward * leapingVelocity);
            playerRigibody.AddForce(Vector3.down * fallingVelocity * inAirTimer);
        }

        if (Physics.SphereCast(rayCastOrigin, 0.2f, Vector3.down, out hit, groundLayer))
        {
            if (!isGrounded && playerManager.isFalling)
            {
                animatorManager.PlayTargetAnimation("Land", true);
            }

            Vector3 rayCastHitPoint = hit.point;
            targetPosition.y = rayCastHitPoint.y;
            inAirTimer = 0;
            isGrounded = true;
        }
        else
        {
            isGrounded = false;
        }

        if (isGrounded && !isJumping)
        {
            if (playerManager.isFalling || inputManager.verticalMovementInput > 0)
            {
                transform.position = Vector3.Lerp(transform.position, targetPosition, inputManager.verticalMovementInput);
            }
            else
            {
                transform.position = targetPosition;
            }
        }

    }

    public void HandleJumping()
    {
        if (isGrounded)
        {
            animatorManager.animator.SetBool("isJumping", true);
            animatorManager.PlayTargetAnimation("Jump", false);

            float jumpingVelocity = Mathf.Sqrt(-2 * (gravityIntensity) * jumpHeight);
            Vector3 playerVelocity = moveDirection;
            playerVelocity.y = jumpingVelocity;
            playerRigibody.velocity = playerVelocity;
        }
    }
}
