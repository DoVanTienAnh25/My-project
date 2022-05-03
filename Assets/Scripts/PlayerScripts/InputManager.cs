using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    PlayerControls playerControls;
    PlayerManager playerManager;
    AnimatorManager animatorManager;
    PlayerLocomotion playerLocomotion;
    Animator animator;
    PlayerUIManager playerUIManager;

    [Header("Player Movement")]
    public float verticalMovementInput;
    public float horizontalMovementInput;
    private Vector2 movementInput;

    [Header("Camera Rotation")]
    public float verticalCameraInput;
    public float horizontalCameraInput;
    private Vector2 cameraInput;

    [Header("Button Input")]
    public bool aimingInput;
    public bool runInput;
    public bool jumpInput;
    public bool shootInput;
    public bool reloadInput;

    private void Awake()
    {
        animatorManager = GetComponent<AnimatorManager>();
        animator = GetComponent<Animator>();
        playerLocomotion = GetComponent<PlayerLocomotion>();
        playerUIManager = FindObjectOfType<PlayerUIManager>(); 
        playerManager = GetComponent<PlayerManager>();
    }

    private void OnEnable()
    {
        if (playerControls == null)
        {
            playerControls = new PlayerControls();

            playerControls.PlayerMovement.Movement.performed += i => movementInput = i.ReadValue<Vector2>();
            playerControls.PlayerMovement.Camera.performed += i => cameraInput = i.ReadValue<Vector2>();

            playerControls.PlayerAction.Jump.performed += i => jumpInput = true;

            playerControls.PlayerMovement.Run.performed += i => runInput = true;
            playerControls.PlayerMovement.Run.canceled += i => runInput = false;

            playerControls.PlayerAction.Aim.performed += i => aimingInput = true;
            playerControls.PlayerAction.Aim.canceled += i => aimingInput = false;

            playerControls.PlayerAction.Shoot.performed += i => shootInput = true;
            playerControls.PlayerAction.Shoot.canceled += i => shootInput = false;

            playerControls.PlayerAction.Reload.performed += i => reloadInput = true;
        }

        playerControls.Enable();
    }

    private void OnDisable()
    {
        playerControls.Disable();
    }
    
    public void HandleAllInput()
    {
        HandleMovementInput();
        HandleCameraInput();
        HandleJumpingInput();
        HandleAimingInput();
        HandleShootingInput();
        HandleReloadInput();
    }

    private void HandleMovementInput()
    {
        horizontalMovementInput = movementInput.x;
        verticalMovementInput = movementInput.y;
        animatorManager.HandleAnimatorValues(horizontalMovementInput, verticalMovementInput, runInput);

        if (verticalMovementInput != 0 || horizontalMovementInput != 0)
        {
            animatorManager.rightHandIK.weight = 0;
            animatorManager.leftHandIK.weight = 0;
        }
        else
        {
            animatorManager.rightHandIK.weight = 1;
            animatorManager.leftHandIK.weight = 1;
        }
    }

    private void HandleCameraInput()
    {
        horizontalCameraInput = cameraInput.x;
        verticalCameraInput = cameraInput.y;
    }
    
    private void HandleJumpingInput()
    {
        if(jumpInput)
        {
            jumpInput = false;
            playerLocomotion.HandleJumping();
        }
    }

    private void HandleAimingInput()
    {
        if (verticalMovementInput != 0 || horizontalMovementInput != 0)
        {
            aimingInput = false;
            animator.SetBool("isAiming", false);
            playerUIManager.crossHair.SetActive(false);
            return;
        }

        if (aimingInput)
        {
            animator.SetBool("isAiming", true);
            playerUIManager.crossHair.SetActive(true);
        }
        else
        {
            animator.SetBool("isAiming", false);
            playerUIManager.crossHair.SetActive(false);
        }

        animatorManager.UpdateAimConstraints();
    }

    private void HandleShootingInput()
    {
        if (shootInput && aimingInput)
        {
            shootInput = false;
            Debug.Log("Bang");
            playerManager.UseCurrentWeapon();
        }
    }

    private void HandleReloadInput()
    {
        if (playerManager.isPerformingAction)
        {
            return;
        }

        if (reloadInput)
        {
            reloadInput = false;

            if (playerManager.playerEquipmentManager.weapon.remainingAmmo == playerManager.playerEquipmentManager.weapon.maxAmmo)
            { 
                return;
            }
                
            
            playerManager.animatorManager.ClearHandIK();
            playerManager.animatorManager.PlayAnimation("Reload_Pistol", true);

            playerManager.playerEquipmentManager.weapon.remainingAmmo = 12;
            playerManager.playerUIManager.currentAmmoCountText.text = playerManager.playerEquipmentManager.weapon.remainingAmmo.ToString();
        }
    }
}
