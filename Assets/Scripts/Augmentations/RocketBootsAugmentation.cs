using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RocketBootsAugmentation : Augmentation
{
    //TODO: make rb velocity change if player is dashing up/down slope

    [SerializeField] float _maxJumpHeight = 10f;
    [SerializeField] float _minJumpHeight = 5f;
    [SerializeField] int _damage;
    [SerializeField] float _cooldown;
    [SerializeField] float _fullChargeTime;
    float startCS;
    float startWS;
    float startSS;
    float endCS;
    float endWS;
    float endSS;
    Collider _damageCollider;
    Coroutine _chargeRoutine;
    Renderer _renderer;
    bool _isEquipped;
    PlayerInput _playerInput;
    PlayerController _controller;
    Damageable _damageable;
    Rigidbody _rb;
    bool _charging;
    public float _chargeTimer;
    public bool _onCooldown;

    public override bool IsEquipped => _isEquipped;

    protected override void Awake()
    {
        base.Awake();
        _damageCollider = transform.GetChild(0).GetComponent<Collider>();
        _damageCollider.enabled = false;
        _renderer = GetComponent<Renderer>();
    }

    public override void Equip(GameObject equipper)
    {
        _playerInput = equipper.GetComponent<PlayerInput>();
        // _damageable = equipper.GetComponent<Damageable>();
        _rb = equipper.GetComponent<Rigidbody>();

        _renderer.enabled = false;
        interactCollider.enabled = false;
        _damageCollider.gameObject.layer = equipper.layer;
        _controller = equipper.GetComponent<PlayerController>();

        startWS = _controller.walkSpeed;
        startSS = _controller.sprintSpeed;
        startCS = _controller.crouchSpeed;
        endWS = startWS * .25f;
        endSS = startSS * .25f;
        endCS = startCS * .25f;

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
        //If augment key is released
        if (!inputPressed) 
        {
            if (_chargeRoutine != null)
            {
                StopCoroutine(_chargeRoutine);
                _chargeRoutine = null;
                RocketJump();
            }
            return;
        }

        //If augment key is pressed

        //If augment on cooldown or not grounded, return
        if (_onCooldown || !_controller.grounded)
            return;

        //Reset charge time
        _chargeTimer = 0f;

        //Start coroutine for charging
        if (_chargeRoutine != null)
            StopCoroutine(_chargeRoutine);
        _chargeRoutine = StartCoroutine(ChargeJump());

        _controller.walkSpeed = endWS;
        _controller.sprintSpeed = endSS;
        _controller.crouchSpeed = endCS;

        _onCooldown = true;
    }

    IEnumerator ChargeJump()
    {
        while (_chargeTimer <= _fullChargeTime)
        {
            _chargeTimer += Time.deltaTime;
            yield return null;
        }
        RocketJump();
        _chargeRoutine = null;
    }

    void RocketJump()
    {
        float jumpHeight = _minJumpHeight + (_maxJumpHeight - _minJumpHeight) * Mathf.Min(_chargeTimer / _fullChargeTime, 1f);
        _rb.AddForce(new Vector3(0, Mathf.Sqrt(2f * 9.81f * jumpHeight) * _rb.mass, 0), ForceMode.Impulse);

        handler.Battery.Drain(batteryCost);
        Invoke(nameof(DisableCooldown), _cooldown);
        _controller.walkSpeed = startWS;
        _controller.sprintSpeed = startSS;
        _controller.crouchSpeed = startCS;
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
