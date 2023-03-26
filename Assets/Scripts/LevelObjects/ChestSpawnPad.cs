using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChestSpawnPad : SpawnPad
{
    public GameObject resourceChest;
    public GameObject tacticalChest;
    public GameObject augmentationChest;
    public GameObject healthUpgradeChest;
    public GameObject batteryUpgradeChest;

    [Range(0, 100)] public int resourceChance;
    [Range(0, 100)] public int tacticalChance;
    [Range(0, 100)] public int healthChance;
    [Range(0, 100)] public int batteryChance;

    void Start()
    {
        SpawnChest();
    }

    void SpawnChest()
    {
        int roll = UnityEngine.Random.Range(0, 101);

        if (roll < resourceChance)
        {
            // Spawn resource
        }
        else if (roll <= tacticalChance)
        {
            // Spawn tactical
        }
        else if (roll <= healthChance)
        {
            // Spawn health
        }
        else if (roll <= batteryChance)
        {
            // spawn battery
        }

    }
    

}
