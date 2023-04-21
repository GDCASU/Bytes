using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DashAugmentation : Augmentation
{
    //TODO: make rb velocity change if player is dashing up/down slope

    [SerializeField] float _distance = 5f;
    [SerializeField] int _collisionDamage;
    [SerializeField] float _activeTime;
    [SerializeField] float _cooldown;
    float _dashForce = 6.0f;
    Collider _damageCollider;
    Coroutine _damageRoutine;
    WaitForSeconds _activeWait;
    Renderer _renderer;
    bool _isEquipped;
    PlayerInput _playerInput;
    Damageable _damageable;
    Rigidbody _rb;
    Transform _orientation;
    Vector3 _dashDirection;
    bool _active;
    bool _onCooldown;

    public override bool IsEquipped => _isEquipped;

    protected override void Awake()
    {
        base.Awake();
        _damageCollider = transform.GetChild(0).GetComponent<Collider>();
        _damageCollider.enabled = false;
        _activeWait = new WaitForSeconds(_activeTime);
        _renderer = GetComponent<Renderer>();
        _dashForce = _distance / _activeTime;
    }

    private void Update()
    {
        if (_active) _rb.velocity = new Vector3(_dashDirection.x, _rb.velocity.y, _dashDirection.z);
        if (equipNow)
        {
            equipNow = false;
            Interact(GameObject.Find("Player").transform.GetChild(1).gameObject);
        }
    }

    void OnValidate()
    {
        _activeWait = new WaitForSeconds(_activeTime);
        _dashForce = _distance / _activeTime;
    }

    public override void Equip(GameObject equipper)
    {
        _playerInput = equipper.GetComponent<PlayerInput>();
        // _damageable = equipper.GetComponent<Damageable>();
        _rb = equipper.GetComponent<Rigidbody>();

        _renderer.enabled = false;
        interactCollider.enabled = false;
        _damageCollider.gameObject.layer = equipper.layer;

        _orientation = equipper.transform.Find("Orientation");

        // foreach (Hurtbox box in _damageable.Hurtboxes)
        // {
        //     Physics.IgnoreCollision(_damageCollider, box.AttachedCollider);
        // }
        _isEquipped = true;
    }

    public override void Unequip()
    {
        _renderer.enabled = true;
        interactCollider.enabled = true;
        _damageCollider.enabled = false;
        // foreach (Hurtbox box in _damageable.Hurtboxes)
        // {
        //     Physics.IgnoreCollision(_damageCollider, box.AttachedCollider, false);
        // }
        _isEquipped = false;
    }

    public override void Trigger(bool inputPressed)
    {
        if (!inputPressed || _onCooldown)
            return;

        // Dash forward by default if no movement keys are pressed. Otherwise, dash in the direction the player is moving.
        if (_playerInput.MoveVector.x != 0.0f || _playerInput.MoveVector.y != 0.0f)
            _dashDirection = _orientation.forward * _playerInput.MoveVector.y + _orientation.right * _playerInput.MoveVector.x;
        else
            _dashDirection = _orientation.forward;

        _dashDirection *= _dashForce;
        
        // _rb.AddForce(dashDirection, ForceMode.Impulse);
        // _rb.AddForce(new Vector3(dashDirection.x, 0, dashDirection.z), ForceMode.VelocityChange);

        if (_damageRoutine != null)
            StopCoroutine(_damageRoutine);
        _damageRoutine = StartCoroutine(EnableDamageCollider());

        _onCooldown = true;
        Invoke(nameof(DisableCooldown), _cooldown);

        handler.Battery.Drain(batteryCost);
    }

    IEnumerator EnableDamageCollider()
    {
        _damageCollider.enabled = true;
        _active = true;
        yield return _activeWait;
        _damageCollider.enabled = false;
        _active = false;
    }

    void DisableCooldown()
    {
        _onCooldown = false;
    }

    void OnTriggerEnter(Collider other)
    {
        if (!_isEquipped)
            return;

        // if (other.gameObject.layer == _damageable.OpponentAllegiance.GetLayer())
        // {
        //     other.GetComponent<Hurtbox>().Owner.ReceiveDamage(_collisionDamage);
        // }
    }
}
