using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NavMeshEnemy : MonoBehaviour
{
    [Header("References")]
    [SerializeField] GameObject target;
    [SerializeField] NavMeshAgent navAgent;

    [Header("Enemy Values")]
    [SerializeField] float damage;

    /*  INTERNALS   */
    private float timer = .2f;
    private float stopwatch = 0f;

    // Update is called once per frame
    void Update()
    {
        stopwatch += Time.deltaTime;

        if (stopwatch >= timer)
        {
            navAgent.SetDestination(target.transform.position);
            stopwatch -= timer;
        }
    }

    public void SetTarget(GameObject newTarget)
    {
        target = newTarget;
    }

    private void OnCollisionEnter(Collision collision)
    {
        PlayerStats player;
        if (player = collision.gameObject.GetComponentInParent<PlayerStats>())
        {
            player.recieveDamage(damage);
        }
    }
}
