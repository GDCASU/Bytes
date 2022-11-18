using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ProjectileVisual))]
public class Old_ProjectileL_Collision : Old_Projectile
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

        ProjectileVisualData data = new ProjectileVisualData();
        data.startPosition = visualSpawnPosition;
        visual.Play(data);
    }

    protected void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer == targetType.GetLayer())
        {
            collision.transform.root.GetComponent<Character>().ReceiveDamage(impactDamage);
            visual.Stop();
            Perish();
        }
        else if (collision.gameObject.layer == Constants.Layer.Environment)
        {
            visual.Stop();
            Perish();
        }
    }
}