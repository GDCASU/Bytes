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
    BatteryUp
}

public class SpawnPad : MonoBehaviour
{
    protected Vector3 spawnPoint;
    protected MapGenerator mapGenerator;
    protected PadType type;

    [Header("Debug")]
    [SerializeField] protected bool debug = false;

    private void Awake()
    {
        mapGenerator = MapGenerator.Instance;
    }

    public PadType GetSpawnerType()
    {
        return type;
    }
}
