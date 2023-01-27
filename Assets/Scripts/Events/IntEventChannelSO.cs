/*
 * Source: https://github.com/UnityTechnologies/open-project-1
 */

using System;
using UnityEngine;

/// <summary>
/// Channel for passing events that have an integer argument.
/// </summary>
[CreateAssetMenu(fileName = "IntEventChannel", menuName = "ScriptableObjects/Events/Int Event Channel")]
public class IntEventChannelSO : DescriptionSO
{
	public Action<int> EventRaised;

	public void RaiseEvent(int value) => EventRaised?.Invoke(value);
}


