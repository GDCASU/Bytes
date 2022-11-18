using System;
using System.Collections;
using UnityEngine;

public class InstantLaunchSkill : SingleOrRepeatingSkill
{
    [SerializeField] int[] _fireRates;
    [SerializeField] int _fireRate;
    [SerializeField] float _launchSpeed;
    [SerializeField] GameObject _projectilePrefab;
    [SerializeField] Transform _visualSpawn;
    [SerializeField] int _projectileCost;

    float _repeatInterval;
    Weapon _weapon;
    Old_Projectile _projectile;
    SimplePool _pool;

    private void Awake()
    {
        _weapon = GetComponent<Weapon>();
        _projectile = _projectilePrefab.GetComponent<Old_Projectile>();

        int capacity = _projectile.Lifespan > 0f ? Mathf.CeilToInt(_projectile.Lifespan * _fireRate) : _fireRate;
        _pool = new SimplePool(_projectilePrefab, capacity);
    }

    private void OnValidate()
    {
        _repeatInterval = 1f / _fireRate;
    }

    public override float RepeatInterval => _repeatInterval;

    public override bool CanPerform(bool isStarting)
    {
        if (!isStarting && ammo.Current >= _projectileCost)
            return true;

        return false;
    }

    protected override void ExecuteManuever()
    {
        Ray ray = new Ray(_weapon.Handler.ProjectileSpawn.position, _weapon.Handler.ProjectileSpawn.forward);
        Projectile projectile = (Projectile)_pool.Get();
        projectile.transform.position = _weapon.Handler.ProjectileSpawn.position;
        projectile.Launch(ray, _launchSpeed, _weapon.Handler.Wielder.Allegiance.GetOpposite(), _visualSpawn.position);
        ammo.Drain(_projectileCost);
        
        // Invoke animation.
    }

    protected override void UpdateChargeProgress(float elapsedTime)
    {
        // TODO
    }

    protected override void UpdateIntervalProgress(float elapsedTime)
    {
        // TODO
    }
}
