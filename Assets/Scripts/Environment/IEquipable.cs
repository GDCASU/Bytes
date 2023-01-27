/*
 * Author: Cristion Dominguez
 * Date: 4 Jan. 2023
 */

using UnityEngine;

public interface IEquipable
{
    public bool IsEquipped { get; }
    public void Equip(GameObject equipper);
    public void Unequip();
}