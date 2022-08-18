using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider), typeof(LineRenderer))]
public class LaserDiode : MonoBehaviour
{
    public float laserStartPoint = 0.0f;
    public float laserMaxDistance = 100.0f;
    public float damage = 1.0f;
    public bool isFlickering = false;
    public float flickerStartDelay = 0.5f;
    public float flickerTime = 1.0f;

    private BoxCollider boxCollider;
    private LineRenderer lineRenderer;
    private bool isOn;

    private void Awake()
    {
        boxCollider = this.GetComponent<BoxCollider>();
        lineRenderer = this.GetComponent<LineRenderer>();
    }

    private void Start()
    {
        boxCollider.center = new Vector3(0, (laserMaxDistance / 2.0f) + laserStartPoint, 0);
        boxCollider.size = new Vector3(0, laserMaxDistance, 0);
        lineRenderer.SetPosition(0, new Vector3(0, laserStartPoint, 0));
        lineRenderer.SetPosition(1, new Vector3(0, laserMaxDistance - laserStartPoint, 0));
        if (isFlickering) InvokeRepeating("FlickerLaser", flickerStartDelay, flickerTime);
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Player") &&
            other.gameObject.GetComponent<Player>())
        {
            other.gameObject.GetComponent<Player>().TakeDamage(damage);
        }
    }

    private void FlickerLaser()
    {
        if (isOn)
        {
            boxCollider.enabled = false;
            lineRenderer.enabled = false;
            isOn = false;
        }
        else
        {
            boxCollider.enabled = true;
            lineRenderer.enabled = true;
            isOn = true;
        }
    }
}
