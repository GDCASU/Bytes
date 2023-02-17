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
    public enum WeaponType { Light = 0, Medium = 1, Heavy = 2 };


    [Header("Specialized Properties")]
    [SerializeField] int weaponType;
    [SerializeField] bool isAutomatic;

    bool isTriggerHeld;
    bool continousRoutineActive;
    WaitForSeconds shootCooldownWait;
    WaitForSeconds reloadWait;

    AmmoStorage ammoStorage;

    [Header("Recoil Values")]
    [SerializeField] float xRecoil;
    [SerializeField] float yRecoil;
    [SerializeField] float zRecoil;
    [SerializeField] GameObject camHolder;
    Recoil camRecoil;

    [Header("Bloom Values")]
    [SerializeField] float bloomInc;
    [SerializeField] float bloomDec;
    [SerializeField] float bloomCap;
    public float bloom = 0;

    FirearmAnimatorInvoker animInvoker;

    protected override void Awake()
    {
        base.Awake();
        currentAmmo = maxAmmo;

        camRecoil = camHolder.GetComponent<Recoil>();
        camRecoil.recoilX = xRecoil;
        camRecoil.recoilY = yRecoil;
        camRecoil.recoilZ = zRecoil;

        ammoStorage = gameObject.GetComponentInParent<AmmoStorage>();

        animInvoker = GetComponent<FirearmAnimatorInvoker>();
        animInvoker.Bind(animator);
        FirearmAnimationData data = new FirearmAnimationData();
        data.shootSpeed = fireRate;
        data.reloadSpeed = 1f / reloadDuration;
        animInvoker.SetParameters(data);
    }

    protected void Update()
    {
        if (bloom >= 0) {
            bloom -= bloomDec * Time.deltaTime;
        }
        else
        {
            bloom = 0;
        }
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
        Ray ray;
        if (bloom >= 0) {
           ray = new Ray(projectileSpawn.position, projectileSpawn.forward + new Vector3(Random.Range(-bloom, bloom), Random.Range(-bloom, bloom), 0)); // added Vector3 is for bloom
        }
        else {
            ray = new Ray(projectileSpawn.position, projectileSpawn.forward); // added Vector3 is for bloom

        }

        Projectile enabledProjectile = projectilePool.Get();
        enabledProjectile.transform.position = projectileSpawn.position;
        enabledProjectile.Launch(ray, launchSpeed, Target, visualProjectileSpawn.position);
        currentAmmo--;

        //bloom----------
        bloom += bloomInc;
        if (bloom >= bloomCap)
        {
            bloom = bloomCap;
        }
        //---------------

        camRecoil.recoilFire();
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
        int ammoReloaded = 0;

        switch (weaponType) { // determines which ammo to reload and assigns ammoReloaded to the returned value
            case 1:
                ammoReloaded = ammoStorage.lightReloaded(maxAmmo, currentAmmo);
                break;
            case 2:
                ammoReloaded = ammoStorage.mediumReloaded(maxAmmo, currentAmmo);
                break;
            case 3:
                ammoReloaded = ammoStorage.heavyReloaded(maxAmmo, currentAmmo);
                break;
        }//end switch

        if (ammoReloaded > 0) {
            animInvoker.Play(FirearmAnimation.Reload);

            canFire = false;
            isReloading = true;
            yield return reloadWait;
            currentAmmo = ammoReloaded;
            isReloading = false;
            canFire = true;
        }
        else
        {
            //no ammo to reload
        }

    }
}
