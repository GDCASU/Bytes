using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipableEntity: MonoBehaviour
{
    public enum EntityType { Weapon = 0, Ability = 1 }
    public enum AbilityType { None = 0, Dash = 1 }

    public EntityType entityType;
    public AbilityType abilityType; // Only used if the entity type is Ability

    [field: SerializeField] public bool IsEquipped { get; private set; }

    public void Equip() => IsEquipped = true;
    public void Unequip() => IsEquipped = false;
}


public struct WeaponEquipData
{
    public Transform container;
    public Transform projectileSpawn;
    public CharacterType target;
}

public struct WeaponUnequipData
{
    public Vector3 dropPosition;
    public Quaternion dropRotation;
}

public struct AbilityEquipData
{

}

public struct AbilityUnequipData
{

}