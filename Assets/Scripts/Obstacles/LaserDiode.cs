using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserDiode : MonoBehaviour
{
    [Header("Laser Variables")]
    [Range(-1, 1)] public float laserDirX;
    [Range(-1, 1)] public float laserDirY;
    [Range(-1, 1)] public float laserDirZ;
    public float laserMaxDistance = 100.0f;
    public float laserRadius = 1.5f;
    public int damage = 1;

    [Header("Flicker Variables")]
    public bool isFlickering = false;
    public float flickerStartDelay = 0.5f;
    public float flickerTime = 1.0f;

    private Transform laser;
    private LineRenderer lineRenderer;
    private Vector3 localLaserDirection;
    private Vector3 worldLaserDirection;
    private bool isOn;

    private void Awake()
    {
        laser = this.transform.GetChild(0);
        lineRenderer = laser.GetComponent<LineRenderer>();
        if (isFlickering) InvokeRepeating("FlickerLaser", flickerStartDelay, flickerTime);
        isOn = true;
    }

    private void FixedUpdate()
    {
        /*
         * The laser directions relative to the world and relative to 'Laser Diode' should both be tracked since `Physics.Raycast()` deals with
         * world directions whilst `lineRenderer.SetPosition()` deals with local directions. Plus, directions should be vectors with a magnitude of 1.
         * - Cristion Domingez
         */
        // NOTE: Keep these lines if the laser direction can change.
        localLaserDirection = new Vector3(laserDirX, laserDirY, laserDirZ).normalized; 
        worldLaserDirection = transform.TransformDirection(localLaserDirection);

        if (isOn)
        {
            UseLaser();
        }
    }

    private void UseLaser()
    {
        // NOTE: Check where the laser hits from the beginning to the end of it. Adjust the GameObject's model and colliders
        // to fit with the laser's hit limits.
        if (Physics.SphereCast(laser.position, laserRadius, worldLaserDirection, out RaycastHit hit, laserMaxDistance, Constants.LayerMask.Protagonist | Constants.LayerMask.Environment))
        {
            if (hit.transform.gameObject.layer == Constants.Layer.Protagonist)
                hit.transform.GetComponent<Hurtbox>().Owner.ReceiveDamage(damage);
                

            // Laser extends to the object it hits
            lineRenderer.SetPosition(0, Vector3.zero);
            lineRenderer.SetPosition(1, localLaserDirection * hit.distance);
        }
        else
        {
            // If nothing is hit, laser extends to its maximum distance that was set
            lineRenderer.SetPosition(0, Vector3.zero);
            lineRenderer.SetPosition(1, localLaserDirection * laserMaxDistance);
        }
    }

    /// <summary>
    /// The laser can turn itself on and off at set intervals when isFlickering is true
    /// </summary>
    private void FlickerLaser()
    {
        if (isOn)
        {
            lineRenderer.enabled = false;
            isOn = false;
        }
        else
        {
            UseLaser(); // Check if SphereCast hit object before showing line renderer.
            lineRenderer.enabled = true;
            isOn = true;
        }
    }
}
