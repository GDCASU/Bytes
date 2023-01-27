/*
 * Author: Cristion Dominguez
 * Date: 13 Jan. 2023
 */

using System;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour, IInteractable, IEquipable
{
    public event Action Started;
    public event Action Equiped;
    public event Action Unequiped;
    public event Action Readied;
    public event Action Stored;

    [field: SerializeField] public Vector3 EquipOffset;
    public bool IsEquipped { get; private set; }
    public WeaponHandler Handler { get; private set; }
    public AmmoInventory Inventory { get; private set; }
    public List<AmmoType> ExpectedAmmos { get; private set; }

    [SerializeField] AnimatorOverrideController _overrideController;
    IWeaponAbility[] _skills = new IWeaponAbility[4];
    Collider _interactCollider;

    void Awake()
    {
        IWeaponAbility[] attachedSkills = GetComponents<IWeaponAbility>();
        ExpectedAmmos = new List<AmmoType>();

        IWeaponAbility skill;
        for (int i = 0; i < attachedSkills.Length; i++)
        {
            skill = attachedSkills[i];
            _skills[(int)skill.Type] = skill;

            if (skill.ExpectedAmmo != AmmoType.None && !ExpectedAmmos.Contains(skill.ExpectedAmmo))
                ExpectedAmmos.Add(attachedSkills[i].ExpectedAmmo);
        }

        Inventory = GetComponent<AmmoInventory>();
        _interactCollider = GetComponent<Collider>();
    }

    void Start() => Started?.Invoke();

    public void Interact(GameObject interactor)
    {
        WeaponHandler handler;
        if (interactor.TryGetComponent(out handler))
        {
            Handler = handler;
            handler.EquipWeapon(this);
        }
    }

    public void Equip(GameObject equipper)
    {
        _interactCollider.enabled = false;
        IsEquipped = true;
        Equiped?.Invoke();
    }

    public void Unequip()
    {
        Store();
        Handler = null;
        _interactCollider.enabled = true;
        IsEquipped = false;
        Unequiped?.Invoke();
    }

    public void Ready() => Readied?.Invoke();

    public void Store()
    {
        foreach (IWeaponAbility skill in _skills)
            skill?.Cancel();

        Stored?.Invoke();
    }

    public void TriggerAbility(WeaponAbilityType type, bool isStarting)
    {
        IWeaponAbility skill = _skills[(int)type];
        if (skill == null)
            return;

        skill.Trigger(isStarting);
    }

    public void EnqueueManeuver(Maneuver maneuver) => Handler?.MQueue.Enqueue(maneuver);
}