using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPad : MonoBehaviour
{
    protected Vector3 spawnPoint;
    protected Room room;

    private void Awake()
    {
        room = GetComponentInParent<Room>();
    }

}
