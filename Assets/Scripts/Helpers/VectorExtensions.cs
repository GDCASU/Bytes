using UnityEngine;

public class Rotation
{
    public static float GetAngularDirection_2D(float fromAngle, Vector2 toDirection)
    {
        float angle1 = Mathf.Acos(toDirection.x);
        float angle2 = Mathf.Asin(toDirection.y);
        float directionAngle = (angle2 < 0f ? angle1 : 2 * Mathf.PI - angle1) * Mathf.Rad2Deg;

        float clockwiseDistance, counterClockwiseDistance;
        if (directionAngle < fromAngle)
        {
            clockwiseDistance = 360f - fromAngle + directionAngle;
            counterClockwiseDistance = fromAngle - directionAngle;
        }
        else
        {
            clockwiseDistance = directionAngle - fromAngle;
            counterClockwiseDistance = 360f - directionAngle + fromAngle;
        }

        if (clockwiseDistance < counterClockwiseDistance)
        {
            return 1;
        }
        else if (clockwiseDistance > counterClockwiseDistance)
        {
            return -1;
        }
        else
        {
            return 0;
        }
    }

    public static float GetAngularDisplacement_2D(float fromAngle, Vector2 toDirection, float maxAngleStep)
    {
        float angle1 = Mathf.Acos(toDirection.x);
        float angle2 = Mathf.Asin(toDirection.y);
        float directionAngle = (angle2 < 0f ? angle1 : 2 * Mathf.PI - angle1) * Mathf.Rad2Deg;

        float clockwiseDistance, counterClockwiseDistance;
        if (directionAngle < fromAngle)
        {
            clockwiseDistance = 360f - fromAngle + directionAngle;
            counterClockwiseDistance = fromAngle - directionAngle;
        }
        else
        {
            clockwiseDistance = directionAngle - fromAngle;
            counterClockwiseDistance = 360f - directionAngle + fromAngle;
        }

        if (clockwiseDistance < counterClockwiseDistance)
        {
            return Mathf.Min(clockwiseDistance, maxAngleStep);
        }
        else if (clockwiseDistance > counterClockwiseDistance)
        {
            return -Mathf.Min(counterClockwiseDistance, maxAngleStep);
        }
        else
        {
            return 0;
        }
    }

    public static float RotateAngleToDirection_2D(float fromAngle, Vector2 toDirection, float maxAngleStep)
    {
        float newAngle = fromAngle + GetAngularDisplacement_2D(fromAngle, toDirection, maxAngleStep);

        if (newAngle < 0f)
        {
            newAngle += 360f;
        }
        else if (newAngle > 360f)
        {
            newAngle -= 360f;
        }

        return newAngle;
    }

    public static float GetVerticalAngularDisplacement(float fromAngle, Vector3 toDirection, float maxAngleStep)
    {
        return 0;
    }
}
