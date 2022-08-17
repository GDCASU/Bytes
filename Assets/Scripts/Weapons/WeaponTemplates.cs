using UnityEngine;
using UnityEngine.InputSystem;

public abstract class IWeapon: MonoBehaviour
{
    public Animator animator;

    protected virtual void Start()
    {
        if (transform.parent == null)
            animator.enabled = false;
    }
    public virtual void OnAnimatorIK()
    {
        animator.rootPosition = transform.localPosition;
        animator.Play("Rest");
    }

    public abstract void Block(bool isStarting);
    public abstract void Reload();
    public abstract void Shoot(bool isStarting);
    public abstract void Strike();
}

public abstract class MeleeWeapon : IWeapon
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
}

public abstract class RangedWeapon: IWeapon
{
    [Header("Basic Properties")]
    [SerializeField]
    protected int maxAmmo;
    [SerializeField]
    protected float fireRate;
    [SerializeField]
    protected float reloadTime;
    [SerializeField]
    protected float launchSpeed;
    [SerializeField]
    protected GameObject projectile;
    [SerializeField]
    protected Transform projectileSpawn;
    [Header("Read-Only Properties")]
    [SerializeField]
    protected int currentAmmo;

    protected bool canFire = true;
    protected bool isTriggerDown = false;
    protected bool isReloading = false;

    public override void Block(bool isStarting) { }
}