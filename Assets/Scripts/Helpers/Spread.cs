using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spread
{
    public static Vector3 DeviatingDirection(Transform origin, float maxSpread)
    {
        Vector2 spreadDirection = Random.insideUnitCircle.normalized;
        Vector3 offsetDirection = new Vector3(origin.right.x * spreadDirection.x, origin.up.y * spreadDirection.y, 0f);
        float offsetMagnitude = Mathf.Tan(Random.Range(0f, maxSpread * Mathf.Deg2Rad));
        Vector3 finalDirection = origin.forward + (offsetDirection * offsetMagnitude);
        return finalDirection;
    }

    public static Vector3 DeviatingDirection(Vector3 forwardDirection, Vector3 rightDirection, Vector3 upDirection, float maxSpread)
    {
        Vector2 spreadDirection = Random.insideUnitCircle.normalized;
        Vector3 offsetDirection = new Vector3(rightDirection.x * spreadDirection.x, upDirection.y * spreadDirection.y, 0f);
        float offsetMagnitude = Mathf.Tan(Random.Range(0f, maxSpread * Mathf.Deg2Rad));
        Vector3 finalDirection = forwardDirection + (offsetDirection * offsetMagnitude);
        return finalDirection;
    }
}
