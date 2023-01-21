using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DashAugmentation : Augmentation
{
    [SerializeField] float _groundedDashForce = 15.0f;
    [SerializeField] float _inAirDashForce = 6.0f;
    [SerializeField] int _collisionDamage;
    [SerializeField] float _damageTime;
    Collider _damageCollider;
    Coroutine _damageRoutine;
    WaitForSeconds _damageWait;
    Renderer _renderer;
    bool _isEquipped;
    Player _player;
    Damageable _damageable;
    PlayerController _controller;
    Rigidbody _rb;

    public override bool IsEquipped => _isEquipped;

    protected override void Awake()
    {
        base.Awake();
        _damageCollider = transform.GetChild(0).GetComponent<Collider>();
        _damageCollider.enabled = false;
        _damageWait = new WaitForSeconds(_damageTime);
        _renderer = GetComponent<Renderer>();
    }

    void OnValidate()
    {
        _damageWait = new WaitForSeconds(_damageTime);
    }

    public override void Equip(GameObject equipper)
    {
        _player = equipper.GetComponent<Player>();
        _damageable = equipper.GetComponent<Damageable>();
        _controller = equipper.GetComponent<PlayerController>();
        _rb = equipper.GetComponent<Rigidbody>();

        _renderer.enabled = false;
        interactCollider.enabled = false;
        _damageCollider.gameObject.layer = equipper.layer;
        foreach (Hurtbox box in _damageable.Hurtboxes)
        {
            Physics.IgnoreCollision(_damageCollider, box.AttachedCollider);
        }
        _isEquipped = true;
    }

    public override void Unequip()
    {
        _renderer.enabled = true;
        interactCollider.enabled = true;
        _damageCollider.enabled = false;
        foreach (Hurtbox box in _damageable.Hurtboxes)
        {
            Physics.IgnoreCollision(_damageCollider, box.AttachedCollider, false);
        }
        _isEquipped = false;
    }

    public override void Trigger(bool inputPressed)
    {
        if (!inputPressed)
            return;

        // Dash forward by default if no movement keys are pressed. Otherwise, dash in the direction the player is moving.
        Vector3 dashDirection;
        if (_player.MoveVector.x != 0.0f || _player.MoveVector.y != 0.0f)
            dashDirection = new Vector3(_player.MoveVector.x, 0, _player.MoveVector.y);
        else
            dashDirection = new Vector3(0, 0, 1);

        dashDirection *= (_controller.isGrounded ? _groundedDashForce : _inAirDashForce);
        _rb.AddRelativeForce(dashDirection, ForceMode.Impulse);

        if (_damageRoutine != null)
            StopCoroutine(_damageRoutine);
        _damageRoutine = StartCoroutine(EnableDamageCollider());

        handler.Battery.Drain(batteryCost);
    }

    IEnumerator EnableDamageCollider()
    {
        _damageCollider.enabled = true;
        yield return _damageWait;
        _damageCollider.enabled = false;
    }

    void OnTriggerEnter(Collider other)
    {
        if (!_isEquipped)
            return;

        if (other.gameObject.layer == _damageable.OpponentAllegiance.GetLayer())
        {
            other.GetComponent<Hurtbox>().Owner.ReceiveDamage(_collisionDamage);
        }
    }
}
