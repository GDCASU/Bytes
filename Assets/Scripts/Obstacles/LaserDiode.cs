using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class LaserDiode : MonoBehaviour
{
    [Header("Laser Variables")]
    public Vector3 laserDir = Vector3.zero;
    public float laserStartBuffer = 0.0f;
    public float laserMaxDistance = 100.0f;
    public float damage = 1.0f;

    [Header("Flicker Variables")]
    public bool isFlickering = false;
    public float flickerStartDelay = 0.5f;
    public float flickerTime = 1.0f;

    private LineRenderer lineRenderer;
    private bool isOn;

    private void Awake()
    {
        lineRenderer = this.GetComponent<LineRenderer>();
        lineRenderer.SetPosition(0, new Vector3(0, laserStartBuffer, 0));
        lineRenderer.SetPosition(1, new Vector3(0, laserMaxDistance - laserStartBuffer, 0));
        if (isFlickering) InvokeRepeating("FlickerLaser", flickerStartDelay, flickerTime);
        isOn = true;
    }

    private void FixedUpdate()
    {
        if (isOn)
        {
            if (Physics.Raycast(transform.position, laserDir, out RaycastHit hit, laserMaxDistance))
            {
                GameObject player = hit.collider.gameObject;
                if (player.CompareTag("Player") &&
                    player.GetComponent<Player>())
                {
                    // Cause damage to the player
                    player.GetComponent<Player>().TakeDamage(damage);
                }
                else
                {
                    // Laser extends to the object it hits
                    lineRenderer.SetPosition(0, new Vector3(0, laserStartBuffer, 0));
                    lineRenderer.SetPosition(1, new Vector3(0, (hit.distance - laserStartBuffer) * 2, 0));
                }
            }
            else
            {
                // If nothing is hit, laser extends to its maximum distance that was set
                lineRenderer.SetPosition(0, new Vector3(0, laserStartBuffer, 0));
                lineRenderer.SetPosition(1, new Vector3(0, (laserMaxDistance - laserStartBuffer), 0));
            }
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
            lineRenderer.enabled = true;
            isOn = true;
        }
    }
}
