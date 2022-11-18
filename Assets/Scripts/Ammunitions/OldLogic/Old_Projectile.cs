/*
 * Author: Cristion Dominguez
 * Date: ???
 */

using System;
using System.Collections;
using UnityEngine;

public abstract class Old_Projectile: MonoBehaviour
{
    [SerializeField] protected float impactDamage;
    [field: SerializeField] public float Lifespan { get; private set; }

    protected ProjectileVisual visual;
    protected Ray ray;
    protected CharacterType targetType;
    protected WaitForSeconds ageWait;
    protected Action<Old_Projectile> returnSelf;

    protected virtual void Awake()
    {
        visual = GetComponent<ProjectileVisual>();
        transform.SetParent(ProjectileManager.Instance.transform);
        ageWait = new WaitForSeconds(Lifespan);
    }

    protected virtual void OnValidate() => ageWait = new WaitForSeconds(Lifespan);

    protected virtual void OnDisable()
    {
        if (visual)
            visual.Stop();
    }

    public abstract void Launch(Ray ray, float launchSpeed, CharacterType targetType, Vector3 visualSpawnPosition);

    protected virtual void CommenceAging() => StartCoroutine(TrackAge());

    protected virtual IEnumerator TrackAge()
    {
        yield return ageWait;
        Perish();
    }

    public virtual void ReturnSelfTo(Action<Old_Projectile> someDelegate)
    {
        returnSelf = someDelegate;
    }

    protected virtual void Perish()
    {
        if (returnSelf != null)
            returnSelf(this);
        else
            Destroy(gameObject);

        StopCoroutine(TrackAge());
    }
}