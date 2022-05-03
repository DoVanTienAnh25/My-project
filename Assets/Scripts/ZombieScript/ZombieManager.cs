using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ZombieManager : MonoBehaviour
{
    public ZombieAnimatorManager zombieAnimator;
    public ZombieStatManager zombieStatManager;

    //the state zombie begin
    public IdleState startingState;

    [Header("flag")]
    public bool isPerformingAction;
    public bool isDead;

    [Header("Current State")]
    //the state zombie currently on
    [SerializeField] private State currentState;

    [Header("Current target")]
    public PlayerManager currentTarget;
    public Vector3 targetDirection;
    public float distanceFromCurrentTarget;
    public float viewableAngleFromCurrentTarget;

    [Header("Nav Mesh Agent")]
    public NavMeshAgent zombieNavMeshAgent;

    [Header("Animator")]
    public Animator animator;

    [Header("Rigidbody")]
    public Rigidbody zombieRigidbody;

    [Header("Locomotion")]
    public float rotationSpeed = 5;

    [Header("Attack")]
    public float mininumAttackDistance = 1f;  //shortest range attack
    public float maxinumAttackDistance = 1.5f;    //longest range attack
    public float attackCoolDownTimer;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        currentState = startingState;
        zombieNavMeshAgent = GetComponentInChildren<NavMeshAgent>();
        zombieRigidbody = GetComponent<Rigidbody>();
        zombieAnimator = GetComponent<ZombieAnimatorManager>();
        zombieStatManager = GetComponent<ZombieStatManager>();
    }
    public void HandleStateMachine()
    {
        State nextState;

        if (currentState != null)
        {
            nextState = currentState.Tick(this);

            if (nextState != null)
            {
                currentState = nextState;
            }
        }
    }

    private void Update()
    {
        zombieNavMeshAgent.transform.localPosition = Vector3.zero;

        if (attackCoolDownTimer > 0)
        {
            attackCoolDownTimer = attackCoolDownTimer - Time.deltaTime;
        }

        if (currentTarget != null)
        {
            targetDirection = currentTarget.transform.position - transform.position;
            viewableAngleFromCurrentTarget = Vector3.SignedAngle(targetDirection, transform.forward, Vector3.up);
            distanceFromCurrentTarget = Vector3.Distance(currentTarget.transform.position, transform.position);
        }
    }

    private void FixedUpdate()
    {
        if (!isDead)
        {
            HandleStateMachine();
        }
       
    }
}
