using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName ="A.I/Actions/Zombie Attack Action")]

public class ZombieAttackAction : ScriptableObject
{
    [Header("Attack Animation")]
    public string attackAnimation;

    [Header("Attack Cooldown")]
    public float attackCooldown = 0.5f;   //cooldown time ATTACK of zombie

    [Header("Attack Angles & Distances")]
    public float maxinumAttackAngle = 20f;          //the max angle of sight of zombie to attack
    public float mininumAttackAngle = -20f;         //the min angle of sight of zombie to attack
    public float mininumAttackDistance = 1f;        //the min distance from the current target
    public float maxinumAttackDistance = 1.5f;      //the max distance from the current target
}
