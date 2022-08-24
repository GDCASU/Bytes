/*
 * Author: JamesLeeNZ
 * Source: https://forum.unity.com/threads/projectile-trajectory-accounting-for-gravity-velocity-mass-distance.425560/
 */

using UnityEngine;

public static class AimPrediction
{
    /* 
     * FirstOrderIntercept: Calculation of how far ahead of the target a shooter needs to aim based on speed, projectile speed, and distance.
     */
    /// <summary>
    /// Returns the position to launch a projectile at to hit the target, utilizing absolute target position and velocity.
    /// </summary>
    public static Vector3 FirstOrderIntercept(
        Vector3 shooterPosition,
        Vector3 shooterVelocity,
        float shotSpeed,
        Vector3 targetPosition,
        Vector3 targetVelocity
    )
    {
        Vector3 targetRelativePosition = targetPosition - shooterPosition;
        Vector3 targetRelativeVelocity = targetVelocity - shooterVelocity;
        float t = FirstOrderInterceptTime
        (
            shotSpeed,
            targetRelativePosition,
            targetRelativeVelocity
        );
        return targetPosition + t * (targetRelativeVelocity);
    }

    /// <summary>
    /// Returns the time a projectile shall hit the target, utilizing relative target position and velocity.
    /// </summary>
    public static float FirstOrderInterceptTime
    (
        float shotSpeed,
        Vector3 targetRelativePosition,
        Vector3 targetRelativeVelocity
    )
    {
        float velocitySquared = targetRelativeVelocity.sqrMagnitude;
        if (velocitySquared < 0.001f)
            return 0f;

        float a = velocitySquared - shotSpeed * shotSpeed;

        //handle similar velocities
        if (Mathf.Abs(a) < 0.001f)
        {
            float t = -targetRelativePosition.sqrMagnitude /
            (
                2f * Vector3.Dot
                (
                    targetRelativeVelocity,
                    targetRelativePosition
                )
            );
            return Mathf.Max(t, 0f); //don't shoot back in time
        }

        float b = 2f * Vector3.Dot(targetRelativeVelocity, targetRelativePosition);
        float c = targetRelativePosition.sqrMagnitude;
        float determinant = b * b - 4f * a * c;

        if (determinant > 0f)
        { //determinant > 0; two intercept paths (most common)
            float t1 = (-b + Mathf.Sqrt(determinant)) / (2f * a),
                    t2 = (-b - Mathf.Sqrt(determinant)) / (2f * a);
            if (t1 > 0f)
            {
                if (t2 > 0f)
                    return Mathf.Min(t1, t2); //both are positive
                else
                    return t1; //only t1 is positive
            }
            else
                return Mathf.Max(t2, 0f); //don't shoot back in time
        }
        else if (determinant < 0f) //determinant < 0; no intercept path
            return 0f;
        else //determinant = 0; one intercept path, pretty much never happens
            return Mathf.Max(-b / (2f * a), 0f); //don't shoot back in time
    }

    /// <summary>
    /// Calculates the angle of trajectory to launch a gravity-bound projectile to reach a target.
    /// </summary>
    /// <returns> Boolean representing whether the projectile can reach the target's distance with the provided velocity. </returns>
    public static bool CalculateTrajectory(float targetDistance, float projectileVelocity, out float calculatedAngle)
    {
        calculatedAngle = 0.5f * (Mathf.Asin((-Physics.gravity.y * targetDistance) / (projectileVelocity * projectileVelocity)) * Mathf.Rad2Deg);
        if (float.IsNaN(calculatedAngle))
        {
            calculatedAngle = 0;
            return false;
        }
        return true;
    }
}
