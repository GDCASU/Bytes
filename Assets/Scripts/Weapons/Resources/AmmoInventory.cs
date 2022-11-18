using System;
using System.Collections.Generic;
using UnityEngine;

public enum AmmoType
{
    Light,
    Medium,
    Heavy,
    Special
}

public class AmmoInventory : MonoBehaviour
{
    [SerializeField, NamedArray(typeof(AmmoType))]
    List<StaticResource> _inventory = new List<StaticResource>();
    Weapon _weapon;

    public bool CanDrain(int amount, AmmoType type) => amount >= _inventory[(int)type].Current;
    public void Drain(int amount, AmmoType type) => _inventory[(int)type].Drain(amount);
    public void Fill(int amount, AmmoType type) => _inventory[(int)type].Fill(amount);

    public void Reload(List<AmmoType> types)
    {
        for (int i = 0; i < types.Count; i++)
        {
            
        }
    }

    private void Awake()
    {
        _weapon = GetComponent<Weapon>();
    }

    private void OnValidate()
    {
        Array values = Enum.GetValues(typeof(AmmoType));
        for (int i = _inventory.Count; i < values.Length; i++)
            _inventory.Add(new StaticResource());
    }
}