using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieStatManager : MonoBehaviour
{
    ZombieManager zombie;

    [Header("Damage Modifiers")]
    public float headShotDamageMultiplier = 1.5f;

    [Header("Overall Health")]
    public int overallHealth = 100;       // the Health each 0, zombie die

    [Header("Head Health")]
    public int headHealth = 100;          

    [Header("UpperBody Health")]
    public int torsoHealth = 100;
    public int leftArmHealth = 100;
    public int rightArmHealth = 100;

    [Header("LowerBody Health")]
    public int leftLegHealth = 100;
    public int rightLegHealth = 100;

    private void Awake()
    {
        zombie = GetComponent<ZombieManager>();    
    }

    public void DealHeadShotDamage(int Damage)
    {
        headHealth = headHealth - Mathf.RoundToInt(Damage * headShotDamageMultiplier);
        overallHealth = overallHealth - Mathf.RoundToInt(Damage * headShotDamageMultiplier);
        CheckForDeath();
    }

    public void DealTorsoDamage(int Damage)
    {
        torsoHealth = torsoHealth - Damage;
        overallHealth = overallHealth - Damage;
        CheckForDeath();
    }

    public void DealArmDamage(bool leftArmDamage,int Damage)
    {
        if (leftArmDamage)
        {
            leftArmHealth = leftArmHealth - Damage;
        }
        else
        {
            rightArmHealth = rightArmHealth - Damage;
        }
        CheckForDeath();
    }

    public void DealLegDamage(bool leftLegDamage, int Damage)
    {
        if (leftLegDamage)
        {
            leftLegHealth = leftLegHealth - Damage;
        }
        else
        {
            rightLegHealth = rightLegHealth - Damage;
        }
        CheckForDeath();
    }

    private void CheckForDeath()
    {
        if (overallHealth <= 0)
        {
            overallHealth = 0;
            zombie.isDead = true;
            zombie.zombieAnimator.PlayTargetActionAnimation("Death");
            
        }
    }
}
