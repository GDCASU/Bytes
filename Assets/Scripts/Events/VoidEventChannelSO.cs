/*
 * Source: https://github.com/UnityTechnologies/open-project-1
 */

using System;
using UnityEngine;

/// <summary>
/// Channel for passing events that have no arguments.
/// </summary>
[CreateAssetMenu(fileName = "VoidEventChannel", menuName = "ScriptableObjects/Events/Void Event Channel")]
public class VoidEventChannelSO : DescriptionSO
{
	public Action EventRaised;

	public void RaiseEvent() => EventRaised?.Invoke();
}


