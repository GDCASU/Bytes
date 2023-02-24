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

    [Header("Reverse Roaming Variables")]
    [Tooltip("Number of seconds to rotate the drone 180 degrees")]
    [SerializeField] [Range(0.01f, 1.0f)] private float reverseRotateRate = 0.01f;
    [Tooltip("Number of seconds to move the drone forward after the 180 degree rotation")]
    [SerializeField] [Range(0.01f, 1.0f)] private float reverseMovementRate = 0.01f;
    [Tooltip("Distance the drone will move after reversing direction")]
    [SerializeField][Range(0.01f, 1.0f)] private float reverseMovementDistance = 0.01f;
    [Tooltip("Distance the drone checks in front of it")]
    [SerializeField] private float distanceToObstacle = 5.0f;

    protected GameObject target;
    protected bool isActing;

    protected virtual void Start()
    {
        target = GameObject.FindGameObjectWithTag(targetTag);
        isActing = false;
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

    /// <summary>
    /// Reverse the drone's direction and move back if the drone is directly in front of an obstacle.
    /// Recommended for FixedUpdate()
    /// </summary>
    protected void ReverseDirection()
    {
        float slerp = 0;
        float lerp = 0;
        Quaternion startRotation = this.transform.rotation;
        Quaternion endRotation = new Quaternion(this.transform.rotation.eulerAngles.x, this.transform.rotation.eulerAngles.y + 180, this.transform.rotation.eulerAngles.z, 1);
        Vector3 startPos = this.transform.position;
        Vector3 endPos = new Vector3(this.transform.position.x, this.transform.position.y + reverseMovementDistance, this.transform.position.z);

        while (slerp <= 1)
        {
            this.transform.rotation = Quaternion.Slerp(startRotation, endRotation, slerp);
            slerp += reverseRotateRate;
        }

        while (lerp <= 1)
        {
            this.transform.position = Vector3.Lerp(startPos, endPos, lerp);
            lerp += reverseMovementRate;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, detectTargetRadius);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, closeToTargetRadius);
        Gizmos.color = Color.green;
        Gizmos.DrawLine(firePoint.position, new Vector3(-(Mathf.Sin(obstacleCheckRayAngle) * obstacleCheckRayLength), 0.0f, Mathf.Cos(obstacleCheckRayAngle) * obstacleCheckRayLength));
        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(firePoint.position, new Vector3(Mathf.Sin(obstacleCheckRayAngle) * obstacleCheckRayLength, 0.0f, Mathf.Cos(obstacleCheckRayAngle) * obstacleCheckRayLength));
    }
}