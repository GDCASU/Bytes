using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipableEntity: MonoBehaviour
{
    public enum EntityType { Weapon = 0, Ability = 1 }
    public enum AbilityType { None = 0, Dash = 1 }

    public EntityType entityType;
    public AbilityType abilityType; // Only used if the entity type is Ability

    [SerializeField] 
    private bool isEquiped;

    public bool CheckIfEquiped() => isEquiped;

    public void Equip() => isEquiped = true;

    public void Unequip() => isEquiped = false;
}
