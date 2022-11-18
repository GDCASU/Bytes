using System;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(ProjectileVisual))]
public abstract class Projectile : MonoBehaviour, IPoolItem
{
    public event Action<IPoolItem> Finished;
    public abstract float Lifespan { get; }

    [SerializeField] protected float impactDamage;
    protected ProjectileVisual visual;
    protected Ray ray;
    protected CombatantAllegiance targetAllegiance;
    protected WaitForSeconds ageWait;
    protected bool isPoolAlive = true;

    protected virtual void Awake()
    {
        visual = GetComponent<ProjectileVisual>();
        transform.SetParent(ProjectileManager.Instance.transform);
        ageWait = new WaitForSeconds(Lifespan);
    }

    protected virtual void OnValidate() => ageWait = new WaitForSeconds(Lifespan);

    protected virtual void CommenceAging() => StartCoroutine(TrackAge());

    protected virtual IEnumerator TrackAge()
    {
        yield return ageWait;
        Perish();
    }

    protected virtual void Perish()
    {
        StopCoroutine(TrackAge());

        if (visual)
            visual.Stop();

        if (isPoolAlive)
        {   
            gameObject.SetActive(false);
            Finished(this);
        }
        else
            Destroy(gameObject);
    }

    public virtual void OnGet()
    {
        gameObject.SetActive(true);
    }

    public virtual void OnPoolDisposed()
    {
        isPoolAlive = false;
    }

    public abstract void Launch(Ray ray, float launchSpeed, CombatantAllegiance targetAllegiance, Vector3 visualSpawnPosition);   
}
