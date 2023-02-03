using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// Enemy will follow the player if it sees the player with a line of view.
/// 
/// Author: Alben Trang
/// </summary>
public class EnemyFollowSight : EnemyFollowBase
{
    [Header("Enemy Follow Sight Variables")]
    public Transform lookPoint;
    public float lookDistance = 10.0f;
    public float tooCloseRadius = 3.0f;

    private Damageable playerDamageable;
    private NavMeshAgent agent;
    private Vector3 lastSeenPlayerPos;
    private bool foundSomething;
    private bool firstSpottedPlayer;
    private RaycastHit hit;

    private void Start()
    {
        playerTarget = GameObject.FindWithTag("Player").transform;
        playerDamageable = playerTarget.GetComponent<Damageable>();
        agent = GetComponent<NavMeshAgent>();
        lastSeenPlayerPos = Vector3.zero;
        canAttack = true;
        firstSpottedPlayer = false;
    }

    private void Update()
    {
        float distanceFromTarget = Vector3.Distance(playerTarget.position, transform.position);
        foundSomething = Physics.SphereCast(lookPoint.position, lookRadius, lookPoint.forward, out hit, lookDistance, Constants.LayerMask.Protagonist);

        if (distanceFromTarget <= agent.stoppingDistance)
        {
            // Rotate to always face the player when they are close.
            FaceTarget();

            // Attack the player continuously when they are close.
            if (canAttack)
            {
                playerDamageable.ReceiveDamage(damage);
                canAttack = false;
                StartCoroutine(AttackCooldown());
            }
        }
        else if ((foundSomething && hit.collider.CompareTag("Player")) || (distanceFromTarget <= tooCloseRadius))
        {
            // Rotate to always face the player when they are close.
            FaceTarget();

            firstSpottedPlayer = true;
            lastSeenPlayerPos = playerTarget.position;
            agent.SetDestination(lastSeenPlayerPos);
        }
        else if (firstSpottedPlayer)
        {
            agent.SetDestination(lastSeenPlayerPos);
        }
    }
}
