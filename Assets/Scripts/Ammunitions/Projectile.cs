using UnityEngine;

public abstract class Projectile: MonoBehaviour
{
    [SerializeField]
    protected float impactDamage;
    [SerializeField]
    protected float lifetime;

    protected Ray ray;
    protected float age = 0f;

    public abstract void Launch(Ray ray, float launchSpeed);
}