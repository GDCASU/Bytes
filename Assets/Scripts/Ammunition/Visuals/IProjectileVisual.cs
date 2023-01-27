/*
 * Author: Cristion Dominguez
 * Date: 13 Jan. 2023
 */

using System;
using UnityEngine;

public struct ProjectileVisualData
{
    public Vector3 startPosition;
    public Vector3 endPosition;
}

public interface IProjectileVisual
{
    public event Action Finished;
    public abstract void Play(ProjectileVisualData data);
    public abstract void Stop();
}