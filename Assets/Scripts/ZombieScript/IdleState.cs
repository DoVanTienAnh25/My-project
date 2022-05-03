using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleState : State
{
    PursueTargetState pursueTargetState;

    [Header("Detection Radius")]
    //how far we detect the target
    [SerializeField] float detectionRadius = 10;

    [Header("Line of Sight Detection")]
    // the setting determins where the linecast start on the y Axis (use for line of sight)
    [SerializeField]float characterEyesLevel = 1.8f;
    [SerializeField]LayerMask ignoreForTheLineOfSightDetection;

    [Header("Detection Layer")]
    //the layer of the target we attack
    [SerializeField] LayerMask detectionLayer;

    [Header("Detection Angle Radius")]
    [SerializeField] float mininumDetectionRadiusAngle = -50f;
    [SerializeField] float maxinumDetectionRadiusAngle = 50f;
    //we make our character idle until they find a portental target
    //if a target is found we proceed to the "pursue target" state
    //if non target is found we remain in idle position

    private void Awake()
    {
        pursueTargetState = GetComponent<PursueTargetState>();
    }
    public override State Tick(ZombieManager zombieManager)
    {
        if (zombieManager.currentTarget != null)
        {
            return pursueTargetState;
        }
        else
        {
            FindATargetViaLineOfSight(zombieManager);
            return this;
        }

    }

    private void FindATargetViaLineOfSight(ZombieManager zombieManager)
    {
        //we search all colliders on the layer of the PLAYER within certain radius
        Collider[] colliders = Physics.OverlapSphere(transform.position, detectionRadius, detectionLayer);

        Debug.Log("we are checking for collider");

        //for every collider that we find, that is on the same layer of the player, we try and search for a PlayerManager script
        for (int i = 0; i < colliders.Length; i++)
        {
            PlayerManager player = colliders[i].transform.GetComponent<PlayerManager>();

            //if PlayerManager is detected, we then check for line of sight
            if (player != null)
            {
                Debug.Log("we have found the player collider");

                Vector3 targetDirection = transform.position - player.transform.position;
                float viewableAngle = Vector3.Angle(targetDirection, transform.forward);
                
                Debug.Log("viewable angle = "+ viewableAngle);

                if (viewableAngle > mininumDetectionRadiusAngle && viewableAngle < maxinumDetectionRadiusAngle)
                {

                    Debug.Log("We have passed the field of view check");

                    RaycastHit hit;
                    Vector3 playerStartPoint = new Vector3(player.transform.position.x, characterEyesLevel, player.transform.position.z);
                    Vector3 zombieStartPoint = new Vector3(transform.position.x, characterEyesLevel, transform.position.z);
                    
                    Debug.DrawLine(playerStartPoint, zombieStartPoint, Color.yellow);

                    //check for object blocking sight
                    if (Physics.Linecast(playerStartPoint, zombieStartPoint, out hit, ignoreForTheLineOfSightDetection))
                    {
                        Debug.Log("There is something in the way");
                    }
                    else
                    {
                        Debug.Log("we have a target, watching states");
                        zombieManager.currentTarget = player;
                    }

                }
                
            }

        }
    }
}
