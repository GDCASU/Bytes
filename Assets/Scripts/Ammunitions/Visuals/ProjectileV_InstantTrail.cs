using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileV_InstantTrail : ProjectileVisual
{
    [SerializeField] float width;
    [SerializeField] float dissipateDuration;

    LineRenderer line;
    Color originalColor;

    void Awake()
    {
        if (!line.TryGetComponent(out line))
            Debug.LogError("Assign a Transform with a Line Renderer to " + name);

        originalColor = line.material.color;
        line.startWidth = width;
        line.endWidth = width;
    }

    void OnValidate()
    {
        if (Application.isPlaying)
        {
            line.startWidth = width;
            line.endWidth = width;
        }
    }

    public override void Play(ProjectileVisualData data)
    {
        line.SetPosition(0, data.startPosition);
        line.SetPosition(1, data.endPosition);
        StartCoroutine(Dissipate());
    }

    public override void Stop() => StopAllCoroutines();

    IEnumerator Dissipate()
    {
        float elapsedTime = 0f;
        Color startColor = originalColor;
        Color endColor = new Color(startColor.r, startColor.g, startColor.b, 0f);

        while(elapsedTime < dissipateDuration)
        {
            line.material.color = Color.Lerp(startColor, endColor, elapsedTime / dissipateDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        line.material.color = endColor;

        Finished();
    }
}
