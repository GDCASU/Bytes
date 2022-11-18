/*
 * Author: Cristion Dominguez
 * Date: ???
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public partial class Firearm : RangedWeapon
{
    [Header("Specialized Properties")]
    [SerializeField] bool isAutomatic;
    
    bool isTriggerHeld;
    bool continousRoutineActive;
    WaitForSeconds shootCooldownWait;
    WaitForSeconds reloadWait;

    FirearmAnimatorInvoker animInvoker;

    protected override void Awake()
    {
        base.Awake();
        currentAmmo = maxAmmo;

        animInvoker = GetComponent<FirearmAnimatorInvoker>();
        animInvoker.Bind(animator);
        FirearmAnimationData data = new FirearmAnimationData();
        data.shootSpeed = fireRate;
        data.reloadSpeed = 1f / reloadDuration;
        animInvoker.SetParameters(data);
    }

    protected void OnValidate()
    {
        shootCooldownWait = new WaitForSeconds(1f / fireRate);
        reloadWait = new WaitForSeconds(reloadDuration);

        if (animInvoker)
        {
            FirearmAnimationData data = new FirearmAnimationData();
            data.shootSpeed = fireRate;
            data.reloadSpeed = 1f / reloadDuration;
            animInvoker.SetParameters(data);
        }
    }

    protected void OnEnable()
    {
        canFire = true;
        isReloading = false;
        isTriggerHeld = false;
        continousRoutineActive = false;

        animInvoker.ResetAnimator();
        if (currentAmmo <= 0)
            StartCoroutine(CR_Reload());
    }

    void OnDisable() => StopAllCoroutines();

    public override void Shoot(bool isStarting)
    {
        if (isAutomatic)
        {
            isTriggerHeld = isStarting;
            if (canFire && !continousRoutineActive && isStarting)
            {
                StartCoroutine(CR_ContinouslyFireBullets());
            }
        }
        else
        {
            if (canFire && isStarting)
            {
                if (currentAmmo > 0)
                {
                    FireBullet();
                    StartCoroutine(CR_UndergoShootCooldown());
                }
                else
                    StartCoroutine(CR_Reload());
            }
        }
    }

    void FireBullet()
    {
        Ray ray = new Ray(projectileSpawn.position, projectileSpawn.forward);
        Old_Projectile enabledProjectile = projectilePool.Get();
        enabledProjectile.transform.position = projectileSpawn.position;
        enabledProjectile.Launch(ray, launchSpeed, Target, visualProjectileSpawn.position);
        currentAmmo--;

        animInvoker.Play(FirearmAnimation.Shoot);
    }

    public override void Strike() { }

    public override void Reload()
    {
        if (!canFire || isReloading || currentAmmo == maxAmmo) return;
        StartCoroutine(CR_Reload());
    }

    IEnumerator CR_ContinouslyFireBullets()
    {
        continousRoutineActive = true;
        while (isTriggerHeld)
        {
            if (!isReloading)
            {
                if (currentAmmo <= 0)
                {
                    yield return StartCoroutine(CR_Reload());
                    continue;
                }

                FireBullet();
            }

            yield return StartCoroutine(CR_UndergoShootCooldown());
        }
        continousRoutineActive = false;
    }

    IEnumerator CR_UndergoShootCooldown()
    {
        canFire = false;
        yield return shootCooldownWait;
        canFire = true;
    }

    IEnumerator CR_Reload()
    {
        animInvoker.Play(FirearmAnimation.Reload);

        canFire = false;
        isReloading = true;
        yield return reloadWait;
        currentAmmo = maxAmmo;
        isReloading = false;
        canFire = true;
    }
}
