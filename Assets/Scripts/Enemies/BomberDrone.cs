using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

// Vertical Slice: https://docs.google.com/document/d/1CcRi0IGXlqaR201UI9EBUWjuiFyrSbZiKEtRt3kqeTI/edit#

/// <summary>
/// A type of drone that will seek the player, then charge at the player, and then
/// blow itself up to deal massive damage to the player.
/// 
/// Author: Alben Trang
/// </summary>
public class BomberDrone : DroneBase
{
    [Header("Bomber Drone Variables")]
    [Tooltip("The amount of damage to hurt the target player")]
    [SerializeField] private float damage = 80;
    [Tooltip("The amount of force the explosion pushes objects")]
    [SerializeField] private float knockback = 7;
    [Tooltip("The distance it travels after its charge attack before it returns to roaming state")]
    [SerializeField] private float maxLaunchDistance = 15;

    protected override void Start()
    {
        base.Start();
        
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();
    }

    protected override void LateUpdate()
    {
        base.LateUpdate();
    }

    protected override void RoamingAction()
    {
        if (Vector3.Distance(target.transform.position, this.transform.position) <= closeToTargetRadius)
        {

        }
        else
        {
            
        }
    }

    protected override void InitialAction()
    {
        transform.LookAt(target.transform);

        // Chase player
        transform.position = Vector3.MoveTowards(transform.position, target.transform.position, droneSpeed * Time.deltaTime);

        // Obstacle detection starts here
        //transform.Translate(0.1f, 0.0f, 0.0f);
    }

    protected override void SecondaryAction()
    {
        
    }
}
