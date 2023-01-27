/*
 * Author: Cristion Dominguez
 * Date: 13 Jan. 2023
 */

using System.Collections;
using UnityEngine;

public class InstantLaunchAbility : SequenceAbility
{
    [SerializeField] WeaponAbilityType _type;
    [SerializeField] AmmoType _ammoType;
    [SerializeField] Projectile _projectile;
    [SerializeField] Transform _projectileVisualSpawn;
    [SerializeField] float _launchSpeed;
    [SerializeField] int _launchCost = 1;
    [SerializeField] float _chargeDuration;
    [SerializeField] float _repeatInterval;
    StaticResource _ammo;
    SimplePool<Projectile> _pool;
    Coroutine _poolDisposalRoutine;
    WaitForSeconds _poolDisposedWait = new WaitForSeconds(3);
    CharacterAllegiance _previousAllegiance;

    public override WeaponAbilityType Type => _type;
    public override AmmoType ExpectedAmmo => _ammoType;
    protected override string SequenceName => "Instant Launch";
    protected override bool CanExecuteEvent => _ammo.Current >= _launchCost;
    protected override float ChargeDuration => _chargeDuration;
    protected override float RepeatInterval => _repeatInterval;

    protected override void Awake()
    {
        base.Awake();
        WeaponHost.Started += Dev_Start;
        WeaponHost.Equiped += OnEquip;
        WeaponHost.Unequiped += OnUnequip;
    }

    void Dev_Start()
    {
        _ammo = WeaponHost.Inventory.GetAmmo(_ammoType);
        _pool = new SimplePool<Projectile>(_projectile, _ammo.Max);
    }

    protected override void ExecuteEvent()
    {
        Transform projectileSpawn = WeaponHost.Handler.ProjectileSpawn;
        Ray ray = new Ray(projectileSpawn.position, projectileSpawn.forward);
        Projectile projectile = _pool.Get();
        projectile.Launch(ray, _launchSpeed, _projectileVisualSpawn.position, WeaponHost.Handler.Combatant);
        _ammo.Drain(_launchCost);

        if (_ammo.Current == 0)
            WeaponHost.TriggerAbility(WeaponAbilityType.Reload, true);
    }

    protected override void OnFailedEvent() => WeaponHost.TriggerAbility(WeaponAbilityType.Reload, true);

    void OnEquip()
    {
        if (_poolDisposalRoutine != null)
            StopCoroutine(_poolDisposalRoutine);

        
    }

    void OnUnequip() => _poolDisposalRoutine = StartCoroutine(CR_CountdownPoolDisposal());

    void OnDestroy() => _pool.Dispose();

    IEnumerator CR_CountdownPoolDisposal()
    {
        yield return _poolDisposedWait;
        _pool.Dispose();
        _poolDisposalRoutine = null;
    }
}
