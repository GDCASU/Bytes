/*
 * Author: Cristion Dominguez
 * Date: 13 Jan. 2023
 */

using System;
using System.Collections;
using UnityEngine;

public class InstantTrailVisual : MonoBehaviour, IProjectileVisual
{
    public event Action Finished;
    [SerializeField] LineRenderer _line;
    [SerializeField] float _width;
    [SerializeField] float _dissipateDuration;
    Color _originalColor;

    void Awake()
    {
        _originalColor = _line.material.color;
        _line.startWidth = _width;
        _line.endWidth = _width;
    }

    void OnValidate()
    {
        if (Application.isPlaying)
        {
            _line.startWidth = _width;
            _line.endWidth = _width;
        }
    }

    public void Play(ProjectileVisualData data)
    {
        _line.SetPosition(0, data.startPosition);
        _line.SetPosition(1, data.endPosition);
        StartCoroutine(Dissipate());
    }

    public void Stop() => StopAllCoroutines();

    IEnumerator Dissipate()
    {
        float elapsedTime = 0f;
        Color startColor = _originalColor;
        Color endColor = new Color(startColor.r, startColor.g, startColor.b, 0f);

        while(elapsedTime < _dissipateDuration)
        {
            _line.material.color = Color.Lerp(startColor, endColor, elapsedTime / _dissipateDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        _line.material.color = endColor;

        Finished?.Invoke();
    }
}
