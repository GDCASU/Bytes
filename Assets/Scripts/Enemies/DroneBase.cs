using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

/// <summary>
/// Drone base class for movement and performing actions.
/// 
/// Author: Alben Trang
/// </summary>
public abstract class DroneBase : MonoBehaviour
{
    [Header("Drone Base Variables")]
    [Tooltip("Speed of the drone's movement")]
    [SerializeField] protected float droneSpeed = 2.0f;
    [Tooltip("Use this activate an initial action when this drone is close to the target")]
    [SerializeField] protected float detectTargetRadius = 10.0f;
    [Tooltip("Use this to make the drone stop and/or perform an action near the target")]
    [SerializeField] protected float closeToTargetRadius = 5.0f;
    [Tooltip("Delay the secondary action by this many seconds")]
    [SerializeField] protected float chargeTime = 2.0f;
    [SerializeField] protected string targetTag = string.Empty;

    protected GameObject target;

    protected virtual void Start()
    {
        target = GameObject.FindGameObjectWithTag(targetTag);
    }

    protected virtual void FixedUpdate()
    {
        if (Vector3.Distance(target.transform.position, this.transform.position) <= closeToTargetRadius)
        {
            SecondaryAction();
        }
        else if (Vector3.Distance(target.transform.position, this.transform.position) <= detectTargetRadius)
        {
            InitialAction();
        }
        else
        {
            RoamingAction();
        }
    }

    protected virtual void LateUpdate()
    {
        // Update UI code here
    }

    protected abstract void RoamingAction();

    protected abstract void InitialAction();

    protected abstract void SecondaryAction();

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, detectTargetRadius);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, closeToTargetRadius);
    }
}