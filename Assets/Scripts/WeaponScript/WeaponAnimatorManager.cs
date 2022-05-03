using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponAnimatorManager : MonoBehaviour
{
    Animator weaponAnimator;
    PlayerManager player;

    [Header("Weapon FX")]
    public GameObject weaponMuzzelFlashFX;  //the muzzle flash FX that is instantiated when weapon fired
    public GameObject weaponBulletCaseFX;   //the bullet case FX is ejected from the weapon, when weapon is fired

    [Header("weapon FX transform")]
    public Transform weaponMuzzleFlashTransform;    // the location of muzzle flash
    public Transform weaponBulletCaseTransform;     // the location of bullet case

    [Header("Weapon Bullet Range")]
    public float bulletRange = 100f;

    [Header("Shootable Layers")]
    public LayerMask shootableLayers;

    private void Awake()
    {
        weaponAnimator = GetComponentInChildren<Animator>();
        player = GetComponentInParent<PlayerManager>();
    }

    public void ShootWeapon(CameraManager cameraManager)
    {
        //Animate the weapon
        weaponAnimator.Play("Fire");

        //Instantiate muzzle flash fx
        GameObject muzzleFlash = Instantiate(weaponMuzzelFlashFX, weaponMuzzleFlashTransform);
        muzzleFlash.transform.parent = null;
        
        //Instantiate Empty bullet fx
        GameObject weaponBullet = Instantiate(weaponBulletCaseFX, weaponBulletCaseTransform);
        weaponBullet.transform.parent = null;

        //Shoot Something
        RaycastHit hit;
        if (Physics.Raycast(cameraManager.cameraPlayer.transform.position, cameraManager.cameraPlayer.transform.forward, out hit, bulletRange, shootableLayers))
        {
            Debug.Log(hit.collider.gameObject.layer);
            ZombieEffectManager zombie = hit.collider.gameObject.GetComponentInParent<ZombieEffectManager>();

            if (zombie != null)
            {
                if (hit.collider.gameObject.layer == 8)
                {
                    zombie.DamageZombieHead(player.playerEquipmentManager.weapon.damage);
                }
                else if (hit.collider.gameObject.layer == 9)
                {
                    zombie.DamageZombieTorso(player.playerEquipmentManager.weapon.damage);
                }
                else if (hit.collider.gameObject.layer == 10)
                {
                    zombie.DamageZombieLeftArm(player.playerEquipmentManager.weapon.damage);
                }
                else if (hit.collider.gameObject.layer == 11)
                {
                    zombie.DamageZombieRightArm(player.playerEquipmentManager.weapon.damage);
                }
                else if (hit.collider.gameObject.layer == 12)
                {
                    zombie.DamageZombieLeftLeg(player.playerEquipmentManager.weapon.damage);
                }
                else if (hit.collider.gameObject.layer == 13)
                {
                    zombie.DamageZombieRightLeg(player.playerEquipmentManager.weapon.damage);
                }
            }
        }
    }

}
