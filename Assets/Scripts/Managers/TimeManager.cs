/*
 * Author: Cristion Dominguez
 * Date: ???
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeManager : MonoSingleton<TimeManager>
{
    [SerializeField, Range(0f, 5f)]
    float timeScale = 1f;

    private void OnValidate() => Time.timeScale = timeScale;
}
