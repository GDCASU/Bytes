using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public abstract class DroneBase : Enemy
{
    public float closeToPlayerRadius = 3.0f;
    public float detectPlayerRadius = 10.0f;

    protected GameObject target;

    protected virtual void Start()
    {
        target = GameObject.FindGameObjectWithTag("Player");
    }

    protected virtual void FixedUpdate()
    {
        if (Physics.CheckSphere(transform.position, closeToPlayerRadius))
        {

        }
        else if (Physics.CheckSphere(transform.position, detectPlayerRadius))
        {

        }
    }
}
