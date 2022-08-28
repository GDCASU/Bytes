/*
 * Author: Cristion Dominguez
 * Date: 19 Aug. 2022
 */

using UnityEngine;

public static class Spread
{
    /// <summary>
    /// Returns a direction that deviates from the forward direcion at a specified max degree.
    /// </summary>
    /// <param name="origin"> Transform with a forward direction to deviate from. </param>
    /// <param name="maxDegree"> Max degree the deviated direction can be from the forward direction. </param>
    public static Vector3 DeviateFromForwardDirection(Transform origin, float maxDegree)
    {
        Vector2 unitCircleDirection = Random.insideUnitCircle.normalized;
        Vector3 offsetDirection = new Vector3(origin.right.x * unitCircleDirection.x, origin.up.y * unitCircleDirection.y, 0f);
        float offsetMagnitude = Mathf.Tan(Random.Range(0f, maxDegree * Mathf.Deg2Rad));
        Vector3 finalDirection = origin.forward + (offsetDirection * offsetMagnitude);
        return finalDirection.normalized;
    }

    /// <summary>
    /// Returns a direction that deviates from the forward direcion at a specified max degree.
    /// </summary>
    /// <param name="forwardDirection"> Forward direction to deviate from. </param>
    /// <param name="rightDirection"> Right direction associated with the forward direction. </param>
    /// <param name="upDirection"> Up direction associated with the forward direction. </param>
    /// <param name="maxDegree"> Max degree the deviated direction can be from the forward direction. </param>
    public static Vector3 DeviateFromForwardDirection(Vector3 forwardDirection, Vector3 rightDirection, Vector3 upDirection, float maxDegree)
    {
        Vector2 unitCircleDirection = Random.insideUnitCircle.normalized;
        Vector3 offsetDirection = new Vector3(rightDirection.x * unitCircleDirection.x, upDirection.y * unitCircleDirection.y, 0f);
        float offsetMagnitude = Mathf.Tan(Random.Range(0f, maxDegree * Mathf.Deg2Rad));
        Vector3 finalDirection = forwardDirection + (offsetDirection * offsetMagnitude);
        return finalDirection.normalized;
    }
}
