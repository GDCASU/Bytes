using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IEquipable
{
    public bool IsEquipped { get; }
    public Vector3 EquipOffsetPosition { get; }
    public Vector3 EquipOffsetRotation { get; }
    public bool CanEquip(Transform wielder);
    public void Equip(Transform wielder);
    public void Unequip(Transform wielder);
}

public struct EquipData
{
    public Transform parent;
}

public struct UnequipData
{
    public Transform parent;
    public Vector3 position;
    public Vector3 rotation;
}