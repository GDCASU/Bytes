using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeManager : MonoBehaviour
{
    [SerializeField, Range(0f, 5f)]
    float timeScale = 1f;

    public static TimeManager singleton;

    void Awake()
    {
        if (!singleton)
            singleton = this;
        else
            Destroy(gameObject);
    }

    private void OnValidate()
    {
        Time.timeScale = timeScale;
    }
}
