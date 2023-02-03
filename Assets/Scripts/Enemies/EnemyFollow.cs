using System.Collections;
using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// Enemy will follow the player if it gets close enough to it.
/// 
/// Author: Alben Trang
/// </summary>
public class EnemyFollow : EnemyFollowBase
{
    // Some code by Brackeys from here: "ENEMY AI - Making an RPG in Unity (E10)"
    // https://www.youtube.com/watch?v=xppompv1DBg&list=PLPV2KyIb3jR4KLGCCAciWQ5qHudKtYeP7&index=11

    private Damageable playerDamageable;
    private NavMeshAgent agent;

    private void Start()
    {
        playerTarget = GameObject.FindWithTag("Player").transform;
        playerDamageable = playerTarget.GetComponent<Damageable>();
        agent = GetComponent<NavMeshAgent>();
        canAttack = true;
    }

    private void Update()
    {
        float distanceFromTarget = Vector3.Distance(playerTarget.position, transform.position);

        if (distanceFromTarget <= lookRadius)
        {
            agent.SetDestination(playerTarget.position);

            if (distanceFromTarget <= agent.stoppingDistance)
            {
                // Attack the player continuously when they are close.
                if (canAttack)
                {
                    playerDamageable.ReceiveDamage(damage);
                    canAttack = false;
                    StartCoroutine(AttackCooldown());
                }

                // Rotate to always face the player when they are close.
                FaceTarget();
            }
        }
    }
}
