using System;
using UnityEngine;

public struct ProjectileVisualData
{
    public Vector3 startPosition;
    public Vector3 endPosition;
}

public abstract class ProjectileVisual : MonoBehaviour
{
    public Action Finished;

    public abstract void Play(ProjectileVisualData data);
    public abstract void Stop();
}