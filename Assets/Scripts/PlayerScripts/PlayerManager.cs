using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    CameraManager cameraManager;
    InputManager inputManager;
    Animator animator;
    
    PlayerLocomotion playerLocomotion;
    public AnimatorManager animatorManager;
    public PlayerUIManager playerUIManager;

    public PlayerEquipmentManager playerEquipmentManager;
    
     [Header("Player Flag")]
    public bool isAiming;
    public bool isFalling;
    public bool isPerformingAction;

    private void Awake()
    {
        cameraManager = FindObjectOfType<CameraManager>();
        inputManager = GetComponent<InputManager>();
        playerLocomotion = GetComponent<PlayerLocomotion>();
        animator = GetComponent<Animator>();
        playerEquipmentManager = GetComponent<PlayerEquipmentManager>();
        animatorManager = GetComponent<AnimatorManager>();
        playerUIManager = FindObjectOfType<PlayerUIManager>();
    }

    private void Update()
    {
        inputManager.HandleAllInput();

        isPerformingAction = animator.GetBool("isPerformingAction");
        isAiming = animator.GetBool("isAiming");
    }

    private void FixedUpdate()
    {
        playerLocomotion.HandleAllLocomtion();
    }

    private void LateUpdate()
    {
        cameraManager.HandleAllCameraMovement();
        isFalling = animator.GetBool("isFalling");

        playerLocomotion.isJumping = animator.GetBool("isJumping");
        animator.SetBool("isGrounded", playerLocomotion.isGrounded);
    }

    public void UseCurrentWeapon()
    {
        if (isPerformingAction)
            return;

        if (playerEquipmentManager.weapon.remainingAmmo > 0)
        {
            playerEquipmentManager.weapon.remainingAmmo = playerEquipmentManager.weapon.remainingAmmo - 1;
            playerUIManager.currentAmmoCountText.text = playerEquipmentManager.weapon.remainingAmmo.ToString();
            animatorManager.PlayAnimation("Shot_Pistol", true);
            playerEquipmentManager.weaponAnimator.ShootWeapon(cameraManager);
        }
        else
        {
            Debug.Log("out of ammo");
        }
        
    }
}
