/*
 * Author: Cristion Dominguez
 * 24 Jan. 2023
 */

using System;
using UnityEngine;

/// <summary>
/// Channel for passing events that have a game object argument.
/// </summary>
[CreateAssetMenu(fileName = "GameObjectEventChannel", menuName = "ScriptableObjects/Events/Game Object Event Channel")]
public class GameObjectEventChannelSO : DescriptionSO
{
	public Action<GameObject> EventRaised;

	public void RaiseEvent(GameObject gameObject) => EventRaised?.Invoke(gameObject);
}


