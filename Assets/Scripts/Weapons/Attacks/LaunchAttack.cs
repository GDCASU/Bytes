using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
public class LaunchAttack : SingleOrRepeatingAttack
{
    enum CooldownType
    {
        Silent,
        FrameUpdater
    }

    [SerializeField] int fireRate;
    [SerializeField] float reloadDuration;
    [SerializeField] float launchSpeed;
    [SerializeField] Projectile projectile;
    [SerializeField] Transform visualProjectileSpawn;
    [SerializeField] CooldownType cooldownType;
    [SerializeField] BaseResource resource;

    float repeatInterval;
    ICooldown cooldown;

    protected override float RepeatInterval => repeatInterval;

    protected override bool CanPerform => true; // ALTER

    public override BaseResource Resource => resource;

    public override ICooldown Cooldown => cooldown;

    private void OnValidate()
    {
        repeatInterval = 1f / fireRate;
        if (cooldownType == CooldownType.Silent && !(cooldown is SilentCooldown))
        {
            cooldown = new SilentCooldown(repeatInterval);
        }
        else if (cooldownType == CooldownType.FrameUpdater && !(cooldown is FrameUpdaterCooldown))
        {
            cooldown = new FrameUpdaterCooldown(this, repeatInterval);
        }
    }

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
}
*/