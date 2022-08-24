using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Pool;

public abstract class Projectile: MonoBehaviour
{
    [SerializeField]
    protected float impactDamage;
    [field: SerializeField]
    public float Lifespan { get; private set; }

    protected Ray ray;
    protected CharacterType targetType;
    protected WaitForSeconds ageWait;
    protected Coroutine ageRoutine;
    protected Action<Projectile> returnSelf;

    protected virtual void Awake()
    {
        transform.SetParent(ProjectileManager.singleton.transform);
        ageWait = new WaitForSeconds(Lifespan);
    }

    protected virtual void OnValidate()
    {
        ageWait = new WaitForSeconds(Lifespan);
    }

    public abstract void Launch(Ray ray, float launchSpeed, CharacterType targetType);

    protected virtual void CommenceAging() => ageRoutine = StartCoroutine(TrackAge());

    protected virtual IEnumerator TrackAge()
    {
        yield return ageWait;
        ageRoutine = null;
        Perish();
    }

    public virtual void ReturnSelfTo(Action<Projectile> someDelegate)
    {
        returnSelf = someDelegate;
    }

    protected virtual void Perish()
    {
        if (returnSelf != null)
            returnSelf(this);
        else
            Destroy(gameObject);

        if (ageRoutine != null)
        {
            StopCoroutine(ageRoutine);
            ageRoutine = null;
        }
    }
}