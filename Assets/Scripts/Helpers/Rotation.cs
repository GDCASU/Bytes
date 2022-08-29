/*
 * Author: Cristion Dominguez
 * Date: 19 Aug. 2022
 */

using UnityEngine;

public static class Rotation
{
    /// <summary>
    /// In a unit circle, returns the absolute smallest angular displacement (w.r.t to the x-axis) from a starting unit vector to a destination unit vector.
    /// </summary>
    /// <param name="startingAngle"> Starting unit vector's angle from the x-axis in the [0, 360] range. </param>
    /// <param name="destinationVector"> Destination unit vector. </param>
    /// <param name="followingUnityStandard"> Following Unity standard shall return a positive displacement when the direction is clockwise and negative
    /// when the direction is counterclockwise; ignoring the standard shall return vice-versa. </param>
    /// <returns> Angular displacement in the [-360, 360] range. </returns>
    public static float GetHorizontalAngularDisplacement(float startingAngle, Vector2 destinationVector, bool followingUnityStandard = true)
    {
        float cosAngle = Mathf.Acos(destinationVector.x);
        if (followingUnityStandard)
        {
            float destinationAngle = (destinationVector.y > 0f ? Constants.Math.PI_2 - cosAngle : cosAngle) * Mathf.Rad2Deg;

            float clockwiseDistance, counterClockwiseDistance;
            if (destinationAngle > startingAngle)
            {
                clockwiseDistance = destinationAngle - startingAngle;
                counterClockwiseDistance = 360f - destinationAngle + startingAngle;
            }
            else
            {
                clockwiseDistance = 360f - startingAngle + destinationAngle;
                counterClockwiseDistance = startingAngle - destinationAngle;
            }

            if (clockwiseDistance < counterClockwiseDistance)
                return clockwiseDistance;
            else if (clockwiseDistance > counterClockwiseDistance)
                return -counterClockwiseDistance;
            else
                return 0;
        }
        else
        {
            float destinationAngle = (destinationVector.y > 0f ? cosAngle : Constants.Math.PI_2 - cosAngle) * Mathf.Rad2Deg;

            float counterClockwiseDistance, clockwiseDistance;
            if (destinationAngle > startingAngle)
            {
                counterClockwiseDistance = destinationAngle - startingAngle;
                clockwiseDistance = 360f - destinationAngle + startingAngle;
            }
            else
            {
                counterClockwiseDistance = 360f - startingAngle + destinationAngle;
                clockwiseDistance = startingAngle - destinationAngle;
            }

            if (counterClockwiseDistance < clockwiseDistance)
                return counterClockwiseDistance;
            else if (counterClockwiseDistance > clockwiseDistance)
                return -clockwiseDistance;
            else
                return 0;
        }
    }

    /// <summary>
    /// In a unit circle, rotates the current unit vector to a target unit vector w.r.t to the x-axis.
    /// </summary>
    /// <param name="currentAngle"> Current unit vector's angle from the x-axis in the [0, 360] range. </param>
    /// <param name="targetVector"> Target unit vector. </param>
    /// <param name="maxDegreesDelta"> Max angle in degrees the current vector is allowed to rotate. </param>
    /// <param name="followingUnityStandard"> Do angles grow in the clockwise direction? </param>
    /// <returns> Angle, in the [0, 360] range, of the current vector after rotating towards the target vector. </returns>
    public static float RotateTowardHorizontalVector(float currentAngle, Vector2 targetVector, float maxDegreesDelta, bool followingUnityStandard = true)
    {
        float displacement = GetHorizontalAngularDisplacement(currentAngle, targetVector, followingUnityStandard);
        float degreesDelta = displacement >= 0 ? Mathf.Min(displacement, maxDegreesDelta) : Mathf.Max(displacement, -maxDegreesDelta);
        float newAngle = currentAngle + degreesDelta;

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

    /// <summary>
    /// In a unit sphere, returns the absolute smallest angular displacement (w.r.t to the xz-plane) from a starting unit vector to a destination unit vector.
    /// </summary>
    /// <param name="startingAngle"> Starting unit vector's angle from the xz-plane in the [0, 90] and [270, 360] ranges. </param>
    /// <param name="destinationVector"> Destination unit vector. </param>
    /// <param name="followingUnityStandard"> Following Unity standard shall return a positive displacement when the direction is clockwise and negative
    /// when the direction is counterclockwise; ignoring the standard shall return vice-versa. </param>
    /// <returns> Angular displacement in the [-180, 180] range. </returns>
    public static float GetVerticalAngularDisplacement(float startingAngle, Vector3 destinationVector, bool followingUnityStandard = true)
    {
        float destinationAngle = Mathf.Asin(destinationVector.y) * Mathf.Rad2Deg;
        startingAngle = (startingAngle <= 90) ? startingAngle : startingAngle - 360f;

        if (followingUnityStandard)
        {
            return -destinationAngle - startingAngle;
        }
        else
        {
            return destinationAngle - startingAngle;
        }
    }

    /// <summary>
    /// In a unit sphere, rotates the current unit vector to a target unit vector w.r.t to the xz-plane.
    /// </summary>
    /// <param name="currentAngle"> Current unit vector's angle from the xz-plane in the [0, 90] and [270, 360] ranges. </param>
    /// <param name="targetVector"> Target unit vector. </param>
    /// <param name="maxDegreesDelta"> Max angle in degrees the current vector is allowed to rotate. </param>
    /// /// <param name="followingUnityStandard"> Do angles grow in the clockwise direction? </param>
    /// <returns> Angle, in the [0, 90] and [270, 360] ranges, of the current vector after rotating towards the target vector. </returns>
    public static float RotateTowardsVerticalVector(float currentAngle, Vector3 targetVector, float maxDegreesDelta, bool followingUnityStandard = true)
    {
        float displacement = GetVerticalAngularDisplacement(currentAngle, targetVector, followingUnityStandard);
        float degreesDelta = displacement >= 0 ? Mathf.Min(displacement, maxDegreesDelta) : Mathf.Max(displacement, -maxDegreesDelta);
        float newAngle = currentAngle + degreesDelta;

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
}
