using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
        if (isEnabled)
        {
            CheckpointManager.Instance.SetLatestCheckpoint(this.gameObject);
            DisableCheckpoint();
        }
    }

    public void DisableCheckpoint()
    {
        this.gameObject.GetComponent<Renderer>().material.color = Color.green;
        isEnabled = false;
    }
}
