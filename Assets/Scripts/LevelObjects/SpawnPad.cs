using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPad : MonoBehaviour
{
    protected Vector3 spawnPoint;
    protected MapGenerator mapGenerator;
    // protected Room room;
    // protected RoomType roomType;

    private void Awake()
    {
        mapGenerator = MapGenerator.Instance;
        // room = GetComponentInParent<Room>();
        // roomType = room.roomType;
    }
}
