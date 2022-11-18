using System;
using UnityEngine;

public class TestSkill : MonoBehaviour, IWeaponSkill
{
    [SerializeField] SkillType _type;
    [SerializeField] int _priority;
    [SerializeField] bool _shouldObstructOnPress, _shouldObstructOnRelease;

    SkillStatus _status = SkillStatus.Inactive;

    public event Action<IWeaponSkill> Deactivated;
#pragma warning disable CS0067
    public event Action ResourceExpended;
    public SkillStatus Status => _status;
    public SkillType Type => _type;
    public int Priority => _priority;
    public AmmoType AmmoType => 0;
    public bool Obstructs(bool isStarting)
    {
        return isStarting ? _shouldObstructOnPress : _shouldObstructOnRelease;
    }
    public bool CanPerform(bool isStarting) => true;
    public void Perform(bool isStarting, bool pauseImmediately = false)
    {
        if (isStarting)
        {
            _status = SkillStatus.Active;
            print("The " + _type.ToString() + " has been performed.");
        }
        else
        {
            _status = SkillStatus.Inactive;
            print("The " + _type.ToString() + " has been stopped.");
            Deactivated(this);
        }
    }
    public void Pause()
    {
        _status = SkillStatus.Paused;
        print("The " + _type.ToString() + " has been paused.");
    }
    public void Resume()
    {
        _status = SkillStatus.Active;
        print("The " + _type.ToString() + " has been resumed.");
    }
    public void Halt()
    {
        _status = SkillStatus.Inactive;
        print("The " + _type.ToString() + " has been halted.");
    }
}
