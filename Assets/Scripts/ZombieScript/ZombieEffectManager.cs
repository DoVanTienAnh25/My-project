using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieEffectManager : MonoBehaviour
{
    ZombieManager zombieManager;

    private void Awake()
    {
        zombieManager = GetComponent<ZombieManager>();
    }

    public void DamageZombieHead(int damage)
    {
        zombieManager.isPerformingAction = true;
        zombieManager.animator.CrossFade("DamageHeavy", 0.2f);
        zombieManager.zombieStatManager.DealHeadShotDamage(damage);
    }

    public void DamageZombieTorso(int damage)
    {
        zombieManager.isPerformingAction = true;
        zombieManager.animator.CrossFade("DamageLight", 0.2f);
        zombieManager.zombieStatManager.DealTorsoDamage(damage);
    }

    public void DamageZombieRightArm(int damage)
    {
        zombieManager.isPerformingAction = true;
        zombieManager.animator.CrossFade("DamageLight", 0.2f);
        zombieManager.zombieStatManager.DealArmDamage(false, damage);
    }

    public void DamageZombieLeftArm(int damage)
    {
        zombieManager.isPerformingAction = true;
        zombieManager.animator.CrossFade("DamageLight", 0.2f);
        zombieManager.zombieStatManager.DealArmDamage(true, damage);
    }

    public void DamageZombieRightLeg(int damage)
    {
        zombieManager.isPerformingAction = true;
        zombieManager.animator.CrossFade("DamageLight", 0.2f);
        zombieManager.zombieStatManager.DealLegDamage(false, damage);
    }

    public void DamageZombieLeftLeg(int damage)
    {
        zombieManager.isPerformingAction = true;
        zombieManager.animator.CrossFade("DamageLight", 0.2f);
        zombieManager.zombieStatManager.DealLegDamage(true, damage);
    }
}
