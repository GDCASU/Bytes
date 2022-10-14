/*
 * Source: https://github.com/UnityTechnologies/open-project-1
 */

using UnityEngine;

/// <summary>
/// Base class for ScriptableObjects that require a public description field.
/// </summary>
public class DescriptionSO : ScriptableObject
{
	[TextArea] public string description;
}
