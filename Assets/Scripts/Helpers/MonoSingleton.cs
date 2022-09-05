/*
 * Author: Seona Bellamy
 * Source: https://blackcatgames.medium.com/easy-singletons-in-unity-1f6905784d3f
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class MonoSingleton<T> : MonoBehaviour where T : MonoSingleton<T>
{
    protected static T _instance;
    public static T Instance
    {
        get
        {
            if (_instance == null)
                Debug.LogError(typeof(T) + " is missing.");

            return _instance;
        }
    }

    protected virtual void Awake()
    {
        if (_instance == null && _instance != this)
        {
            _instance = this as T;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            // Debug.Log("Second instance of " + typeof(T) + " was created. Destroying second instance.");
            Destroy(this.gameObject);
        }
    }

    private void OnDestroy()
    {
        if (_instance == this)
            _instance = null;
    }
}
