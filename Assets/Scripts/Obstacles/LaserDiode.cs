using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserDiode : MonoBehaviour
{
    [Header("Laser Variables")]
    public Vector3 laserStartBuffer = Vector3.zero;
    [Range(-1, 1)] public float laserDirX;
    [Range(-1, 1)] public float laserDirY;
    [Range(-1, 1)] public float laserDirZ;
    public float laserMaxDistance = 100.0f;
    public float laserRadius = 1.5f;
    public float damage = 1.0f;

    [Header("Flicker Variables")]
    public bool isFlickering = false;
    public float flickerStartDelay = 0.5f;
    public float flickerTime = 1.0f;

    private Transform laserPoint;
    private LineRenderer lineRenderer;
    private Vector3 laserDir;
    private bool isOn;

    private void Awake()
    {
        laserPoint = this.transform.GetChild(0);
        lineRenderer = laserPoint.GetComponent<LineRenderer>();
        laserDir = new Vector3(laserDirX, laserDirY, laserDirZ);
        if (isFlickering) InvokeRepeating("FlickerLaser", flickerStartDelay, flickerTime);
        isOn = true;
    }

    private void FixedUpdate()
    {
        laserDir = new Vector3(laserDirX, laserDirY, laserDirZ); // Keep this line if the laser direction can change
        if (isOn)
        {
            Vector3 laserVector = Vector3.zero;
            if (Physics.Raycast(laserPoint.position, laserDir, out RaycastHit hit, laserMaxDistance))
            {
                GameObject player = hit.collider.gameObject;
                if (player.CompareTag("Player") &&
                    player.GetComponent<Player>())
                {
                    // Cause damage to the player
                    player.GetComponent<Player>().TakeDamage(damage);
                    lineRenderer.SetPosition(0, laserStartBuffer);
                    lineRenderer.SetPosition(1, new Vector3(0, (hit.distance) * 2, 0) - hit.transform.position);
                }
                else
                {
                    // Laser extends to the object it hits
                    laserVector = laserDir * ((hit.distance));
                    lineRenderer.SetPosition(0, laserStartBuffer);
                    lineRenderer.SetPosition(1, laserVector);
                    Debug.DrawRay(laserPoint.position, transform.TransformDirection(laserVector), Color.yellow);
                }
            }
            else
            {
                // If nothing is hit, laser extends to its maximum distance that was set
                laserVector = laserDir * laserMaxDistance;
                lineRenderer.SetPosition(0, laserStartBuffer);
                lineRenderer.SetPosition(1, laserVector);
                Debug.DrawRay(laserPoint.position, transform.TransformDirection(laserDir) * laserMaxDistance, Color.red);
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
