using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.SocialPlatforms;

/// <summary>
/// Drone base class for movement and performing actions.
/// 
/// Author: Alben Trang
/// </summary>
public abstract class DroneBase : MonoBehaviour
{
    [Header("Drone Base Variables")]
    [Tooltip("The empty game object's transform that's used for firing objects in front of the drone")]
    [SerializeField] protected Transform firePoint;
    [Tooltip("The tag of the object that the drone will seek")]
    [SerializeField] protected string targetTag = string.Empty;
    [Tooltip("Speed of the drone's movement")]
    [SerializeField] protected float droneSpeed = 2.0f;
    [Tooltip("Use this activate an initial action when this drone is close to the target")]
    [SerializeField] protected float detectTargetRadius = 10.0f;
    [Tooltip("Use this to make the drone stop and/or perform an action near the target")]
    [SerializeField] protected float closeToTargetRadius = 5.0f;
    [Tooltip("Delay the secondary action by this many seconds")]
    [SerializeField] protected float chargeTime = 2.0f;
    [Tooltip("The length of the raycasts that will check for obstacles")]
    [SerializeField] protected float obstacleCheckRayLength = 4.0f;
    [Tooltip("Separates the two raycasts that checks for obstacles with this angle in radians")]
    [SerializeField] [Range(0.0f, 1.57f)] protected float obstacleCheckRayAngle = 1.0f;
    [Tooltip("Speed that the drone will avoid obstacles during its initial action")]
    [SerializeField] protected float obstacleAvoidanceSpeed = 0.01f;

    [Header("Return to Original Position Variables")]
    [Tooltip("Rate to rotate the drone 180 degrees")]
    [SerializeField] [Range(0.001f, 1.0f)] protected float returnRotateRate = 0.01f;
    [Tooltip("Rate to move the drone forward after the 180 degree rotation")]
    [SerializeField] [Range(0.001f, 1.0f)] protected float returnMovementRate = 0.01f;

    [Header("Reverse Roaming Variables")]
    [Tooltip("Rate to rotate the drone 180 degrees")]
    [SerializeField] [Range(0.001f, 1.0f)] protected float reverseRotateRate = 0.01f;
    [Tooltip("Distance the drone checks in front of it")]
    [SerializeField] protected float distanceToObstacle = 5.0f;

    protected GameObject target;
    protected Vector3 originalPosition;
    protected bool isActing;
    protected float originalHeight;
    protected bool foundTarget;

    protected virtual void Start()
    {
        target = GameObject.FindGameObjectWithTag(targetTag);
        originalPosition = transform.position;
        isActing = false;
        originalHeight = transform.position.y;
        foundTarget = false;
    }

    protected virtual void FixedUpdate()
    {
        if (!isActing)
        {
            if (Vector3.Distance(target.transform.position, this.transform.position) <= closeToTargetRadius)
            {
                foundTarget = true;
                SecondaryAction();
            }
            else if (Vector3.Distance(target.transform.position, this.transform.position) <= detectTargetRadius)
            {
                foundTarget = true;
                InitialAction();
            }
            else
            {
                if (foundTarget)
                {
                    foundTarget = false;
                    ReturnToOriginalHeight();
                }
                else
                {
                    RoamingAction();
                }
            }
        }
    }

    protected virtual void LateUpdate()
    {
        // Update UI code here
    }

    protected abstract void RoamingAction();

    protected abstract void InitialAction();

    protected abstract void SecondaryAction();

    /// <summary>
    /// The drone returns to its original position and rotation except the y axis
    /// </summary>
    protected void ReturnToOriginalHeight()
    {
        isActing = true;

        float slerp = 0;
        float lerp = 0;
        Quaternion startRotation = this.transform.rotation;
        Quaternion endRotation = new Quaternion(0, this.transform.rotation.eulerAngles.y, 0, 1);
        Vector3 startPos = this.transform.position;
        Vector3 endPos = new Vector3(this.transform.position.x, originalHeight, this.transform.position.z);

        while (slerp <= 1)
        {
            this.transform.rotation = Quaternion.Slerp(startRotation, endRotation, slerp);
            slerp += reverseRotateRate;
        }

        while (lerp <= 1)
        {
            this.transform.position = Vector3.Lerp(startPos, endPos, lerp);
            lerp += returnMovementRate;
        }

        isActing = false;
    }

    /// <summary>
    /// Reverse the drone's direction and move back if the drone is directly in front of an obstacle.
    /// </summary>
    protected void ReverseDirection()
    {
        isActing = true;

        float slerp = 0;
        Quaternion startRotation = this.transform.rotation;
        Quaternion endRotation = new Quaternion();
        endRotation.eulerAngles = new Vector3(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y + 180, transform.rotation.eulerAngles.z);

        while (slerp <= 1)
        {
            this.transform.rotation = Quaternion.Slerp(startRotation, endRotation, slerp);
            slerp += returnRotateRate;
        }

        isActing = false;
    }
}