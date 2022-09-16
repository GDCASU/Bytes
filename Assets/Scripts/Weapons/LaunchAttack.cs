using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaunchAttack : SingleOrRepeatingAttack
{
    [SerializeField] int fireRate;
    [SerializeField] float reloadDuration;
    [SerializeField] float launchSpeed;
    [SerializeField] Projectile projectile;
    [SerializeField] Transform visualProjectileSpawn;

    float repeatInterval;

    protected override float RepeatInterval => repeatInterval;

    protected override bool CanPerform => true; // ALTER

    protected override void PerformManuever()
    {
        print("PERFORM");
        Ray ray = new Ray(transform.position, transform.forward);
        Projectile enabledProjectile = Instantiate(projectile);
        enabledProjectile.transform.position = transform.position;
        enabledProjectile.Launch(ray, launchSpeed, CharacterType.Enemy, visualProjectileSpawn.position);
        // resourse drain
    }

    public override void RestoreResource()
    {
        //
    }

    protected override void UpdateDelayProgress(float elapsedTime)
    {
        //
    }

    protected override void UpdateIntervalProgress(float elapsedTime)
    {
        //
    }

    public override bool AddCooldownObserver(Action<float> cooldownUpdated) => false;
    public override void RemoveCooldownObserver(Action<float> cooldownUpdated) { }

    public override bool AddResourceObserver(Action<int> resourceUpdated)
    {
        //
        return true;
    }
    public override void RemoveResourceObserver(Action<int> resourceUpdated)
    {
        //
    }

    private void OnValidate()
    {
        repeatInterval = 1f / fireRate;
    }
}
