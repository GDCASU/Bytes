/*
 * Author: Cristion Dominguez
 * Date: 13 Jan. 2023
 */

using System.Collections;
using UnityEngine;

[RequireComponent(typeof(IProjectileVisual))]
public abstract class Projectile : PoolItem
{
    public abstract float Lifespan { get; }

    [SerializeField] protected int impactDamage;
    protected IProjectileVisual visual;
    protected Ray ray;
    protected Damageable launcher;
    protected WaitForSeconds ageWait;
    protected bool isPoolAlive = true;

    protected virtual void Awake()
    {
        visual = GetComponent<IProjectileVisual>();
        transform.SetParent(ProjectileManager.Instance.transform);
        ageWait = new WaitForSeconds(Lifespan);
    }

    protected virtual void OnValidate() => ageWait = new WaitForSeconds(Lifespan);

    protected virtual void CommenceAging() => StartCoroutine(CR_Age());

    protected virtual IEnumerator CR_Age()
    {
        yield return ageWait;
        Perish();
    }

    protected virtual void Perish()
    {
        StopCoroutine(CR_Age());

        visual.Stop();

        if (isPoolAlive)
        {
            gameObject.SetActive(false);
            ReturningToPool();
        }
        else
            Destroy(gameObject);
    }

    public override void OnGet()
    {
        gameObject.SetActive(true);
    }

    public override void OnPoolDisposed()
    {
        isPoolAlive = false;
    }

    public abstract void Launch(Ray ray, float launchSpeed, Vector3 visualSpawnPosition, Damageable launcher);
}