using System.Collections;
using System.Collections.Generic;
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

    }

    protected override void SecondaryAction()
    {
        
    }
}
