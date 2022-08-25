using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public partial class Firearm : RangedWeapon
{
    [Header("Specialized Properties")]
    [SerializeField]
    private bool isAutomatic;

    private WaitForSeconds cooldownWait;
    private WaitForSeconds reloadWait;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        currentAmmo = maxAmmo;
    }

    protected void OnValidate()
    {
        cooldownWait = new WaitForSeconds(1f / fireRate);
        reloadWait = new WaitForSeconds(reloadTime);
    }

    private void OnDisable()
    {
        StopAllCoroutines();
        animator.StopPlayback();
    }

    public override void Shoot(bool isStarting)
    {
        if (isAutomatic)
        {
            canFire = isStarting;
            if (isStarting)
            {
                StartCoroutine(ContinouslyFireBullets());
            }
        }
        else
        {
            if (isStarting && !isReloading)
            {
                if (currentAmmo > 0)
                    FireBullet();
                else
                    StartCoroutine(UndergoReload());
            }
        }
    }

    private void FireBullet()
    {
        Ray ray = new Ray(projectileSpawn.position, projectileSpawn.forward);
        projectilePool.Get().Launch(ray, isProjectileInstant ? 0 : launchSpeed, Target);
        currentAmmo--;

        animator.SetTrigger("Shoot");
    }

    public override void Strike()
    {
        print("Strike");
    }

    public override void Reload()
    {
        if (isReloading || currentAmmo == maxAmmo) return;
        StartCoroutine(UndergoReload());
    }

    private IEnumerator ContinouslyFireBullets()
    {
        while (canFire)
        {
            if (!isReloading)
            {
                if (currentAmmo <= 0)
                {
                    yield return StartCoroutine(UndergoReload());
                    continue;
                }

                FireBullet();
            }
            
            yield return cooldownWait;
        }
    }

    private IEnumerator UndergoReload()
    {
        animator.SetBool("MustReload", true);

        isReloading = true;
        yield return reloadWait;
        currentAmmo = maxAmmo;
        isReloading = false;
    }
}
