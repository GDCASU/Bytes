using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// Base enemy script that sets up some variables for following the player.
/// 
/// Author: Alben Trang
/// </summary>
public abstract class EnemyFollowBase : Enemy
{
    [Header("Base Enemy Follow Variables")]
    public float lookRadius = 10.0f;
    public float rotationSpeed = 5.0f;
    public float attackCooldown = 1.0f;
    public int damage = 1;

    protected Transform playerTarget;
    protected bool canAttack;

    protected virtual void FaceTarget()
    {
        Vector3 dirToTarget = (playerTarget.position - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(dirToTarget.x, 0, dirToTarget.z));
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * rotationSpeed);
    }

    protected virtual IEnumerator AttackCooldown()
    {
        yield return new WaitForSeconds(attackCooldown);
        canAttack = true;
    }

    protected virtual void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, lookRadius);
    }
}
