using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public partial class Firearm : RangedWeapon
{
    [Header("Specialized Properties")]
    [SerializeField]
    private bool isAutomatic;

    private bool wasFiringPinActivated = true;

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

    private void Update()
    {
        if (wasFiringPinActivated && canFire)
        {
            if (!isReloading && currentAmmo <= 0)
            {
                StartCoroutine(UndergoReload());
            }
            else
            {
                FireBullet();
                if (!isAutomatic)
                    wasFiringPinActivated = false;
            }
        }
    }

    public override void Shoot(bool isStarting)
    {
        wasFiringPinActivated = isStarting;
    }

    private void FireBullet()
    {
        Ray ray = new Ray(projectileSpawn.position, projectileSpawn.forward);
        Instantiate(projectile, projectileSpawn.position, Quaternion.identity).GetComponent<Projectile>().Launch(ray, isProjectileInstant ? 0 : launchSpeed);
        currentAmmo--;
        StartCoroutine(Cooldown());

        animator.SetTrigger("Shoot");
    }

    public override void Strike()
    {
        Debug.Log("Strike");
    }

    public override void Reload()
    {
        if (isReloading || currentAmmo == maxAmmo) return;
        StartCoroutine(UndergoReload());
    }

    private IEnumerator Cooldown()
    {
        canFire = false;
        yield return cooldownWait;
        canFire = true;
    }

    private IEnumerator UndergoReload()
    {
        animator.SetBool("MustReload", true);

        canFire = false;
        isReloading = true;
        yield return reloadWait;
        currentAmmo = maxAmmo;
        canFire = true;
        isReloading = false;
    }
}
