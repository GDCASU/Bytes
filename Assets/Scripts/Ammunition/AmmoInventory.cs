/*
 * Author: Cristion Dominguez
 * Date: 13 Jan. 2023
 */

using System;
using System.Collections.Generic;
using UnityEngine;

public enum AmmoType
{
    Light,
    Medium,
    Heavy,
    Special,
    None
}

public class AmmoInventory : MonoBehaviour
{
    [SerializeField, NamedArray(typeof(AmmoType))]
    List<StaticResource> _inventory = new List<StaticResource>();

    void OnValidate()
    {
        Array values = Enum.GetValues(typeof(AmmoType));
        for (int i = _inventory.Count; i < values.Length - 1; i++)
            _inventory.Add(new StaticResource());
    }

    public StaticResource GetAmmo(AmmoType type) => _inventory[(int)type];
}