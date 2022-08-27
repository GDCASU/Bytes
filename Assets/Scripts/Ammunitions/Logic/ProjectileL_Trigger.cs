using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileL_Trigger : Projectile
{
    protected Rigidbody body;

    protected override void Awake()
    {
        base.Awake();
        body = GetComponent<Rigidbody>();
    }

    public override void Launch(Ray ray, float launchSpeed, CharacterType targetType, Vector3 visualSpawnPosition)
    {
        CommenceAging();
        this.targetType = targetType;

        transform.rotation = Quaternion.LookRotation(ray.direction);
        body.velocity = ray.direction * launchSpeed;

        if (visual)
        {
            ProjectileVisualData data = new ProjectileVisualData();
            data.startPosition = visualSpawnPosition;
            visual.Play(data);
        }
    }

    protected void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == targetType.GetLayer())
        {
            other.transform.root.GetComponent<Character>().ReceiveDamage(impactDamage);
            if (visual)
                visual.Stop();
            Perish();
        }
        else if (other.gameObject.layer == Constants.Layer.Environment)
        {
            if (visual)
                visual.Stop();
            Perish();
        }
    }
}
