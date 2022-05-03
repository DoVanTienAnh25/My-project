using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PursueTargetState : State
{
    AttackState attackState;

    private void Awake()
    {
        attackState = GetComponent<AttackState>();    
    }

    public override State Tick(ZombieManager zombieManager)
    {
        //if zombie get dame, or in some action, pause the state
        if (zombieManager.isPerformingAction)
        {
            zombieManager.animator.SetFloat("Vertical", 0, 0.2f, Time.deltaTime);
            return this;
        }
        Debug.Log("zombie is in the pursue state");
        MoveTowardsCurrentTarget(zombieManager);
        RotateTowardsTarget(zombieManager);
        
        if(zombieManager.distanceFromCurrentTarget <= zombieManager.mininumAttackDistance)
        {
            zombieManager.zombieNavMeshAgent.enabled = true;
            return attackState;
        }
        else
        {
            return this;
        }
    }

    private void MoveTowardsCurrentTarget(ZombieManager zombieManager)
    {
        zombieManager.animator.SetFloat("Vertical", 1, 0.2f, Time.deltaTime);
    }

    private void RotateTowardsTarget(ZombieManager zombieManager)
    {
        zombieManager.zombieNavMeshAgent.enabled = true;
        zombieManager.zombieNavMeshAgent.SetDestination(zombieManager.currentTarget.transform.position);
        zombieManager.transform.rotation = Quaternion.Slerp(zombieManager.transform.rotation, 
            zombieManager.zombieNavMeshAgent.transform.rotation, 
            zombieManager.rotationSpeed / Time.deltaTime);
    }
}
