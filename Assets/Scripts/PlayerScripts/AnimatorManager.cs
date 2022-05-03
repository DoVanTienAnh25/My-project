using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class AnimatorManager : MonoBehaviour
{
    public Animator animator;
    PlayerManager playerManager;
    RigBuilder rigBuilder;

    [Header("Hand IK Constraints")]
    public TwoBoneIKConstraint rightHandIK;
    public TwoBoneIKConstraint leftHandIK;

    [Header("Aiming Constranints")]
    public MultiAimConstraint spine01;
    public MultiAimConstraint spine02;
    public MultiAimConstraint head;

    float snappedHorizontal;
    float snappedVertical;

    private void Awake()
    {
        animator = GetComponent<Animator>();   
        playerManager = GetComponent<PlayerManager>();
        rigBuilder = GetComponent<RigBuilder>();
    }

    public void PlayTargetAnimation(string targetAnimation, bool isFalling)
    {
        animator.SetBool("isFalling", isFalling);
        animator.CrossFade(targetAnimation, 0.2f);
    }

    public void PlayAnimation(string targetAnimation, bool isPerformingAction)
    {
        animator.SetBool("isPerformingAction", isPerformingAction);
        animator.CrossFade(targetAnimation, 0.2f);
    }

    public void HandleAnimatorValues(float horizontalMovement, float verticalMovement, bool isRunning)
    {
        if (horizontalMovement > 0)
        {
            snappedHorizontal = 1;
        }
        else if (horizontalMovement < 0)
        {
            snappedHorizontal = -1;
        }
        else
        {
            snappedHorizontal = 0;
        }



        if (verticalMovement > 0)
        {
            snappedVertical = 1;
        }
        else if (verticalMovement < 0)
        {
            snappedVertical = -1;
        }
        else
        {
            snappedVertical = 0;
        }

        
        if (isRunning && snappedVertical > 0)
        {
            snappedVertical = 2;
        }
        
        if (isRunning && snappedVertical < 0)
        {
            snappedVertical = -2;
        }

        if(isRunning && snappedVertical == 0)
        {
            if ( snappedHorizontal < 0)
            {
                snappedHorizontal = -2;
            }
            else if (snappedHorizontal > 0)
            {
                snappedHorizontal = 2;
            }
            else
            {
                snappedHorizontal = 0;
            }
        }

        animator.SetFloat("Horizontal", snappedHorizontal, 0.1f,Time.deltaTime);
        animator.SetFloat("Vertical", snappedVertical, 0.1f, Time.deltaTime);
    }
    
    public void AssignHandIK(RightHandIKTarget rightTarget, LeftHandIKTarget leftTarget)
    {
        rightHandIK.data.target = rightTarget.transform;
        leftHandIK.data.target = leftTarget.transform;
        rigBuilder.Build();
    }

    public void ClearHandIK()
    {
        rightHandIK.data.targetPositionWeight = 0;
        rightHandIK.data.targetRotationWeight = 0;

        leftHandIK.data.targetPositionWeight = 0;
        leftHandIK.data.targetRotationWeight = 0;
    }

    public void RefreshHandIK()
    {
        rightHandIK.data.targetPositionWeight = 1;
        rightHandIK.data.targetRotationWeight = 1;

        leftHandIK.data.targetPositionWeight = 1;
        leftHandIK.data.targetRotationWeight = 1;
    }
    public void UpdateAimConstraints()
    {
        if(playerManager.isAiming)
        {
            spine01.weight = 0.3f;
            spine02.weight = 0.3f;
            head.weight = 0.7f;
        }
        else
        {
            spine01.weight = 0;
            spine02.weight = 0;
            head.weight = 0;
        }
    }
}
