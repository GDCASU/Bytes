using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class ProjectileL_Collision : Projectile
{
    [SerializeField] float _lifespan;
    Rigidbody _body;

    public override float Lifespan => _lifespan;

    protected override void Awake()
    {
        base.Awake();
        _body = GetComponent<Rigidbody>();
    }

    public override void Launch(Ray ray, float launchSpeed, CombatantAllegiance targetAllegiance, Vector3 visualSpawnPosition)
    {
        CommenceAging();
        this.targetAllegiance = targetAllegiance;

        transform.rotation = Quaternion.LookRotation(ray.direction);
        _body.velocity = ray.direction * launchSpeed;

        ProjectileVisualData data = new();
        data.startPosition = visualSpawnPosition;
        visual.Play(data);
    }

    protected void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer == targetAllegiance.GetLayer())
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