using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PadType
{ 
    Container = 0,
    Chest,
    Teleporter
}

public enum LootCode
{
    None = 0,
    Resource,
    Augmentation,
    Tactical,
    HealthUp,
    BatteryUp,
    Ammo,
    Health,
    Keycard
}

public class SpawnPad : MonoBehaviour
{
    protected Vector3 spawnPoint;
    protected MapGenerator mapGenerator;
    protected PadType type;

    protected bool debug;

    private void Awake()
    {
        mapGenerator = MapGenerator.Instance;

        if (mapGenerator.debug)
            debug = true;
        else
            debug = false;
    }

    public PadType GetSpawnerType()
    {
        return type;
    }
}
