using UnityEngine;
using UnityEngine.InputSystem;

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
}

public abstract class RangedWeapon: Weapon
{
    [Header("Basic Properties")]
    [SerializeField]
    protected int maxAmmo;
    [SerializeField]
    protected float fireRate;
    [SerializeField]
    protected float reloadTime;
    [SerializeField]
    protected bool isProjectileInstant;
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
    protected bool isReloading = false;

    public override void Block(bool isStarting) { }
}