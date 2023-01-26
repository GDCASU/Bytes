/*
 * Author: Cristion Dominguez
 * Date: 23 Jan. 2023
 */

using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Damageable))]
public class OpponentDetection : MonoBehaviour
{
    [SerializeField] Transform _eye;
    [SerializeField] float _viewDistance;
    [SerializeField, Range(0, 180)] float _maxHorizontalAngle;
    [SerializeField, Range(0, 90)] float _maxVerticalAngle;
    [SerializeField] Transform _center;
    [SerializeField] float _senseRadius;
    [SerializeField] float _checkRate;
    Damageable _damageable;
    Collider[] _detectedColliders = new Collider[15];
    Coroutine _detectRoutine;
    WaitForSeconds _checkWait;
    Vector3 _visiblePointOnOpponent;
    RaycastHit _hit;

    public UnityEvent OpponentFound;
    public UnityEvent OpponentLost;
    public float MaxHorizontalAngle => _maxHorizontalAngle;
    public float MaxVerticalAngle => _maxVerticalAngle;

    public Damageable OpponentDamageable { get; private set; }
    public Vector3 VisiblePointOnOpponent => _visiblePointOnOpponent;
    public Vector3 LastSeenOpponentPosition { get; private set; }

    private void Awake()
    {
        _damageable = GetComponent<Damageable>();
    }

    private void Start()
    {
        StartDetecting();
    }

    public void StartDetecting()
    {
        _detectRoutine = StartCoroutine(CR_Detect());
    }

    public void StopDetecting()
    {
        if (_detectRoutine != null)
            StopCoroutine(_detectRoutine);
    }

    IEnumerator CR_Detect()
    {
        while (true)
        {
            while (!OpponentDamageable)
            {
                OpponentDamageable = FindOpponent();
                yield return _checkWait;
            }

            OpponentFound.Invoke();

            while (IsOpponentVisible(OpponentDamageable.transform, OpponentDamageable.TargetTransforms, out _visiblePointOnOpponent))
            {
                yield return Constants.WaitFor.fixedUpdate;
            }

            LastSeenOpponentPosition = OpponentDamageable.transform.position;
            OpponentDamageable = null;

            OpponentLost?.Invoke();
        }
    }

    Damageable FindOpponent()
    {
        int count = Physics.OverlapSphereNonAlloc(_eye.position, _viewDistance, _detectedColliders, _damageable.OpponentAllegiance.GetLayerMask(), QueryTriggerInteraction.Ignore);
        for (int i = 0; i < count; i++)
        {
            Collider collider = _detectedColliders[i];
            if (IsColliderVisible(collider))
            {
                return collider.GetComponent<Hurtbox>().Owner;
            }
        }

        return null;
    }

    bool IsColliderVisible(Collider collider) =>
        (Physics.Raycast(_eye.position, collider.transform.position - _eye.position, out _hit, _viewDistance, _damageable.OpponentAllegiance.GetLayerMask() | Constants.LayerMask.Environment, QueryTriggerInteraction.Ignore) &&
        collider == _hit.collider &&
        InPerception(_hit.point));

    bool IsOpponentVisible(Transform target, Transform[] detectionTransforms, out Vector3 visiblePoint)
    {
        foreach (Transform transform in detectionTransforms)
        {
            if (Physics.Raycast(_eye.position, transform.position - _eye.position, out _hit, _viewDistance, _damageable.OpponentAllegiance.GetLayerMask() | Constants.LayerMask.Environment, QueryTriggerInteraction.Ignore) &&
            _hit.transform.CompareTag(target.transform.tag) &&
            InPerception(_hit.point))
            {
                visiblePoint = transform.position;
                return true;
            }
        }

        visiblePoint = Vector3.zero;
        return false;
    }

    bool InPerception(Vector3 point)
    {
        if (Vector3.Distance(_center.position, point) <= _senseRadius)
            return true;

        Vector3 direction = _eye.InverseTransformPoint(point).normalized;
        float horizontalAngle = Mathf.Acos(direction.z) * Mathf.Rad2Deg;
        float verticalAngle = Mathf.Asin(direction.y) * Mathf.Rad2Deg;
        return horizontalAngle <= _maxHorizontalAngle && verticalAngle >= -_maxVerticalAngle && verticalAngle <= _maxVerticalAngle;
    }
}
