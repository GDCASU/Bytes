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

    private void Start()
    {
        isEquiped = false;
    }

    public bool CheckIfEquiped() => isEquiped;

    public void ChangeEquip()
    {
        isEquiped = !isEquiped;
    }
}
