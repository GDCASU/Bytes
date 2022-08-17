using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipableEntity: MonoBehaviour
{
    public enum EntityType { Weapon = 0, Ability = 1 }

    [SerializeField] 
    private bool isEquiped;
    [SerializeField]
    public EntityType entityType;

    public bool CheckIfEquiped() => isEquiped;

    public void Equip() => isEquiped = true;

    public void Unequip() => isEquiped = false;
}
