using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackState : State
{
    PursueTargetState pursueTargetState;

    [Header("Zombie Attack")]
    public ZombieAttackAction[] zombieAttackActions;

    [Header("Potential Attacks Performable Right Now")]
    public List<ZombieAttackAction> potentialAttacks;

    [Header("Current Attack Being performed")]
    public ZombieAttackAction currentAttack;

    [Header("State Flags")]
    public bool hasPerformedAttack;

    private void Awake()
    {
        pursueTargetState = GetComponent<PursueTargetState>();
    }

    public override State Tick(ZombieManager zombieManager)
    {
        zombieManager.animator.SetFloat("Vertical", 0, 0.2f, Time.deltaTime);
        
        if (zombieManager.isPerformingAction)
        {
            zombieManager.animator.SetFloat("Vertical", 0, 0.2f, Time.deltaTime);
            return this;
        }

        if (!hasPerformedAttack && zombieManager.attackCoolDownTimer <= 0)
        {
            if (currentAttack == null)
            {
                GetNewAttack(zombieManager);
            }
            else
            {
                AttackTarget(zombieManager);
            }
        }

        if (hasPerformedAttack)
        {
            ResetStateFlag();
            return pursueTargetState;
        }
        else
        {
            return this;
        }
    }

    private void GetNewAttack (ZombieManager zombieManager)
    {
        for (int i = 0; i < zombieManager.attackCoolDownTimer; i++)
        {
            ZombieAttackAction zombieAttacks = zombieAttackActions[i];

            if (zombieManager.distanceFromCurrentTarget <= zombieAttacks.maxinumAttackDistance &&
                zombieManager.distanceFromCurrentTarget >= zombieAttacks.mininumAttackDistance)
            {
                if (zombieManager.viewableAngleFromCurrentTarget <= zombieAttacks.maxinumAttackAngle &&
                    zombieManager.viewableAngleFromCurrentTarget >= zombieAttacks.mininumAttackAngle)
                {
                    potentialAttacks.Add(zombieAttacks);
                }
            }
        }

        int randomValue = Random.Range(0, potentialAttacks.Count);

        if (potentialAttacks.Count > 0)
        {
            currentAttack = potentialAttacks[randomValue];
            potentialAttacks.Clear();
        }
    }

    private void AttackTarget(ZombieManager zombieManager)
    {
        if (currentAttack != null)
        {
            hasPerformedAttack = true;
            zombieManager.attackCoolDownTimer = currentAttack.attackCooldown;
            zombieManager.zombieAnimator.PlayTargetAttackAnimation(currentAttack.attackAnimation);
        }
        else
        {
            Debug.LogWarning("Zombie attack, but there is no animation");
        }
    }

    private void ResetStateFlag()
    {
        hasPerformedAttack = false;
    }
}
