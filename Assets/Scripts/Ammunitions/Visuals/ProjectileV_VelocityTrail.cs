using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileV_VelocityTrail : ProjectileVisual
{
    [SerializeField] float headWidth;
    [SerializeField] float convergeDuration;

    TrailRenderer trail;

    void Awake()
    {
        if (!visual.TryGetComponent(out trail))
            Debug.LogError("Assign a Transform with a TrailRenderer to " + name);

        trail.startWidth = headWidth;
    }

    void OnValidate()
    {
        if (Application.isPlaying)
        {
            trail.startWidth = headWidth;
        }
    }

    public override void Play(ProjectileVisualData data)
    {
        visual.position = data.startPosition;
        trail.Clear();
        StartCoroutine(Converge());
    }

    public override void Stop() => StopAllCoroutines();

    IEnumerator Converge()
    {
        float elapsedTime = 0f;
        Vector3 startPosition = visual.localPosition;
        Vector3 endPosition = new Vector3(0f, 0f, 0f);
        
        while(elapsedTime < convergeDuration)
        {
            visual.localPosition = Vector3.Lerp(startPosition, endPosition, elapsedTime / convergeDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        visual.localPosition = endPosition;
    }
}
