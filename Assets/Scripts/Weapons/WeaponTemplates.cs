using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Pool;

public abstract class Weapon: MonoBehaviour
{
    protected CharacterType wielder;
    protected CharacterType Target
    {
        get
        {
            if (wielder == CharacterType.Player)
                return CharacterType.Enemy;
            else
                return CharacterType.Player;
        }
    }

    protected Animator animator;

    protected void OnEnable() => wielder = PlayerController.singleton.GetComponent<Character>().Type;

    public abstract void Block(bool isStarting);
    public abstract void Reload();
    public abstract void Shoot(bool isStarting);
    public abstract void Strike();
    public abstract void PrepareWeapon();
    public abstract void NeglectWeapon();
}

public abstract class MeleeWeapon : Weapon
{
    [Header("Basic Properties")]
    [SerializeField]
    protected float damage;
    [SerializeField]
    protected float strikeSpeed;
    [SerializeField]
    protected float damageBlockedPercentage;

    public override void Reload() { }
    public override void Shoot(bool isStarting) { }
    public override void PrepareWeapon() { }
    public override void NeglectWeapon() { }
}

public abstract class RangedWeapon: Weapon
{
    [Header("Basic Properties")]
    [SerializeField]
    protected int maxAmmo;
    [SerializeField]
    protected int fireRate;
    [SerializeField]
    protected float reloadTime;
    [SerializeField]
    protected bool isProjectileInstant;
    [SerializeField]
    protected float launchSpeed;
    [SerializeField]
    protected Projectile projectile;
    [SerializeField]
    protected Transform projectileSpawn;
    [Header("Read-Only Properties")]
    [SerializeField]
    protected int currentAmmo;

    protected ObjectPool<Projectile> projectilePool;
    protected Action<Action<Projectile>> notifyBulletToDestoySelf;
    protected Coroutine neglectRoutine;
    protected WaitForSeconds neglectWait = new WaitForSeconds(3f);

    protected bool canFire = true;
    protected bool isReloading = false;

    public override void Block(bool isStarting) { }

    public override void PrepareWeapon()
    {
        if (neglectRoutine != null)
        {
            StopCoroutine(neglectRoutine);
            neglectRoutine = null;
            return;
        }

        float projectileLifespan = projectile.Lifespan;
        int capacity = projectileLifespan > 0f ? Mathf.CeilToInt(projectileLifespan * fireRate) : fireRate;

        projectilePool = new ObjectPool<Projectile>(
            () =>
            {
                Projectile newProjectile = Instantiate(projectile);
                newProjectile.ReturnSelfTo((p) => projectilePool.Release(p));
                notifyBulletToDestoySelf += newProjectile.ReturnSelfTo;
                return newProjectile;
            },
            (p) => p.gameObject.SetActive(true),
            (p) => p.gameObject.SetActive(false),
            (p) =>
            {
                if (p)
                {
                    notifyBulletToDestoySelf -= p.ReturnSelfTo;
                    Destroy(p.gameObject);
                }
            },
            false,
            capacity,
            capacity + 1
        );
    }

    public override void NeglectWeapon() => neglectRoutine = StartCoroutine(WaitToNeglect());

    protected virtual IEnumerator WaitToNeglect()
    {
        yield return neglectWait;
        projectilePool.Clear();
        notifyBulletToDestoySelf?.Invoke(null);
        neglectRoutine = null;
    }
}