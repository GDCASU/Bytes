/*
 * Author: Cristion Dominguez
 * Date: 13 Jan. 2023
 */

using UnityEngine;

[RequireComponent(typeof(Collider), typeof(Rigidbody))]
public class CollisionProjectile : Projectile
{
    [SerializeField] float _lifespan;
    Collider _collider;
    Rigidbody _body;

    public override float Lifespan => _lifespan;

    protected override void Awake()
    {
        base.Awake();
        _collider = GetComponent<Collider>();
        _body = GetComponent<Rigidbody>();
    }

    public override void Launch(Ray ray, float launchSpeed, Vector3 visualSpawnPosition, Damageable launcher)
    {
        CommenceAging();
        this.launcher = launcher;

        gameObject.layer = launcher.PersonalAllegiance.GetLayer();
        transform.position = ray.origin;
        transform.rotation = Quaternion.LookRotation(ray.direction);
        _body.velocity = ray.direction * launchSpeed;

        ProjectileVisualData data = new();
        data.startPosition = visualSpawnPosition;
        visual.Play(data);
    }

    void OnCollisionEnter(Collision collision)
    {
        if (_collider.isTrigger)
            return;

        if (collision.gameObject.layer == launcher.OpponentAllegiance.GetLayer())
        {
            collision.gameObject.GetComponent<Hurtbox>().Owner.ReceiveDamage(impactDamage);
            visual.Stop();
            Perish();
        }
        else if (collision.gameObject.layer == Constants.Layer.Environment)
        {
            visual.Stop();
            Perish();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!_collider.isTrigger)
            return;

        if (other.gameObject.layer == launcher.OpponentAllegiance.GetLayer())
        {
            other.GetComponent<Hurtbox>().Owner.ReceiveDamage(impactDamage);
            visual.Stop();
            Perish();
        }
        else if (other.gameObject.layer == Constants.Layer.Environment)
        {
            visual.Stop();
            Perish();
        }
    }
}