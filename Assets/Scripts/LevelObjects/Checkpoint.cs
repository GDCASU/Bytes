using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

/*
 * Author: Alben Trang
 * Date: 8/31/2022
 */
public class Checkpoint : MonoBehaviour
{
    private bool isEnabled;

    private void Awake()
    {
        isEnabled = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (isEnabled && other.gameObject.CompareTag("Player"))
        {
            CheckpointManager.Instance.SetLatestCheckpoint(this.gameObject);
            Disable();
        }
    }

    public bool GetEnabled() => isEnabled;

    public void Enable()
    {
        isEnabled = true;
        this.gameObject.GetComponent<Renderer>().material.color = Color.yellow;
    }

    public void Disable()
    {
        isEnabled = false;
        this.gameObject.GetComponent<Renderer>().material.color = Color.green;
    }
}
