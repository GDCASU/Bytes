using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Pool;

public abstract class Weapon: MonoBehaviour
{
    [Header("Offset")]
    [SerializeField]
    protected Vector3 offsetPosition;
    [SerializeField]
    protected Vector3 offsetRotation;

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
    protected Collider attachedCollider;
    protected EquipableEntity equipableEntity; 

    protected virtual void Awake()
    {
        animator = GetComponent<Animator>();
        attachedCollider = GetComponent<Collider>();
        equipableEntity = GetComponent<EquipableEntity>();
    }

    protected void OnEnable() => wielder = PlayerController.singleton.GetComponent<Character>().Type;

    public abstract void Block(bool isStarting);
    public abstract void Reload();
    public abstract void Shoot(bool isStarting);
    public abstract void Strike();
    public abstract void PrepareWeapon(WeaponHandler handler);
    public abstract void NeglectWeapon(Vector3 dropPosition, Quaternion dropRotation);
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
    public override void PrepareWeapon(WeaponHandler handler) { }
    public override void NeglectWeapon(Vector3 dropPosition, Quaternion dropRotation) { }
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
    protected float launchSpeed;
    [SerializeField]
    protected Projectile projectile;
    [SerializeField]
    protected Transform visualProjectileSpawn;

    [Header("Read-Only Properties")]
    [SerializeField]
    protected int currentAmmo;

    protected Transform projectileSpawn;
    protected ObjectPool<Projectile> projectilePool;
    protected Action<Action<Projectile>> notifyBulletToDestoySelf;
    protected Coroutine neglectRoutine;
    protected WaitForSeconds neglectWait = new WaitForSeconds(3f);

    protected bool canFire = true;
    protected bool isReloading = false;

    public override void Block(bool isStarting) { }

    public override void PrepareWeapon(WeaponHandler handler)
    {
        attachedCollider.enabled = false;
        equipableEntity.Equip();
        transform.SetParent(handler.WeaponContainer);
        transform.localPosition = offsetPosition;
        transform.localRotation = Quaternion.Euler(offsetRotation);
        projectileSpawn = handler.ProjectileSpawn;

        if (neglectRoutine != null)
        {
            StopCoroutine(neglectRoutine);
            neglectRoutine = null;
            return;
        }

        int capacity = projectile.Lifespan > 0f ? Mathf.CeilToInt(projectile.Lifespan * fireRate) : fireRate;
        projectilePool = new ObjectPool<Projectile>
        (
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

    public override void NeglectWeapon(Vector3 dropPosition, Quaternion dropRotation)
    {
        neglectRoutine = StartCoroutine(WaitToNeglect());
        attachedCollider.enabled = true;
        equipableEntity.Unequip();
        transform.SetParent(null);
        transform.position = dropPosition;
        transform.rotation = dropRotation;
    }

    protected virtual IEnumerator WaitToNeglect()
    {
        yield return neglectWait;
        projectilePool.Clear();
        notifyBulletToDestoySelf?.Invoke(null);
        projectileSpawn = null;
        neglectRoutine = null;
    }
}