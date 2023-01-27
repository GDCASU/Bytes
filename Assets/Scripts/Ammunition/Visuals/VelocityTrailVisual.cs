/*
 * Author: Cristion Dominguez
 * Date: 13 Jan. 2023
 */

using System;
using System.Collections;
using UnityEngine;

public class VelocityTrailVisual : MonoBehaviour, IProjectileVisual
{
    public event Action Finished;
    [SerializeField] TrailRenderer _trail;
    [SerializeField] float _headWidth;
    [SerializeField] float _tailWidth;
    [SerializeField] float _length;
    [SerializeField] float _convergeDuration;

    void Awake()
    {
        _trail.startWidth = _headWidth;
        _trail.endWidth = _tailWidth;
        _trail.time = _length;
    }

    void OnValidate()
    {
        if (Application.isPlaying)
        {
            _trail.startWidth = _headWidth;
        }
    }

    public void Play(ProjectileVisualData data)
    {
        _trail.transform.position = data.startPosition;
        _trail.Clear();
        StartCoroutine(Converge());
    }

    public void Stop() => StopAllCoroutines();

    IEnumerator Converge()
    {
        float elapsedTime = 0f;
        Vector3 startPosition = _trail.transform.localPosition;
        Vector3 endPosition = Vector3.zero;
        
        while(elapsedTime < _convergeDuration)
        {
            _trail.transform.localPosition = Vector3.Lerp(startPosition, endPosition, elapsedTime / _convergeDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        _trail.transform.localPosition = endPosition;

        Finished?.Invoke();
    }
}
