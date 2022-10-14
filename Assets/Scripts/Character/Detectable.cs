/*
 * Author: Cristion Dominguez
 * Date: 6 Oct. 2022
 */

using UnityEngine;

[RequireComponent(typeof(ICombatant))]
public class Detectable: MonoBehaviour
{
    [SerializeField] bool _isVisible = true;
    [SerializeField] Transform[] _detectionPoints;

    ICombatant _combatant;

    public bool IsVisible => _isVisible;
    public ICombatant Combatant => _combatant;

    public Transform[] DetectionPoints => _detectionPoints;
}