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
    [Tooltip("Use this to make the drone stop and/or perform an action near the target")]
    public float closeToTargetRadius = 3.0f;
    [Tooltip("Use this activate an initial action when this drone is close to the target")]
    public float detectTargetRadius = 10.0f;
    public string targetTag = string.Empty;

    protected GameObject target;

    protected virtual void Start()
    {
        target = GameObject.FindGameObjectWithTag(targetTag);
    }

    protected virtual void FixedUpdate()
    {
        if (Physics.CheckSphere(transform.position, closeToTargetRadius))
        {
            SecondaryAction();
        }
        else if (Physics.CheckSphere(transform.position, detectTargetRadius))
        {
            InitialAction();
        }
    }

    protected abstract void InitialAction();

    protected abstract void SecondaryAction();
}