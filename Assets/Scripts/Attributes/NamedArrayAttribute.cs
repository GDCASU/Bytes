/*
 * Defines an attribute that makes the array use enum values as labels.
 * Use like this:
 *     [NamedArray(typeof(eDirection))] public GameObject[] m_Directions;
 * 
 * Author: Ardenian
 * Source: https://forum.unity.com/threads/how-to-change-the-name-of-list-elements-in-the-inspector.448910/
 */

using UnityEngine;
using System;

public class NamedArrayAttribute : PropertyAttribute
{
    public Type TargetEnum;
    public NamedArrayAttribute(Type TargetEnum)
    {
        this.TargetEnum = TargetEnum;
    }
}