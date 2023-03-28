using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChestSpawnPad : SpawnPad
{
    public bool debug = false;

    public GameObject resourceChest;
    public GameObject augmentationChest;
    public GameObject tacticalChest;
    public GameObject healthUpgradeChest;
    public GameObject batteryUpgradeChest;

    public bool forceSpawnResource;
    public bool forceSpawnAugmentation;
    public bool forceSpawnTactical;
    public bool forceSpawnHealthUpgrade;
    public bool forceSpawnBatteryUpgrade;

    [Range(0, 100)] public int resourceChance;
    [Range(0, 100)] public int augmentationChance;
    [Range(0, 100)] public int tacticalChance;
    [Range(0, 100)] public int healthChance;
    [Range(0, 100)] public int batteryChance;

    private bool chestSpawnedbyForce, chestSpawnedbyChance;
    void Start()
    {
        chestSpawnedbyForce = CheckForceSpawn();
        
        if (!chestSpawnedbyForce)
            chestSpawnedbyChance = SpawnChanceBasedChest();
    }

    bool CheckForceSpawn()
    {
        if (forceSpawnResource)
        {
            // Spawn resource chest
            if (debug) Debug.Log("Resource Chest Spawned by Force");
            return true;
        }
        if (forceSpawnAugmentation)
        {
            // Spawn Augmentation
            if (debug) Debug.Log("Augmentation Chest Spawned by Force");
            return true;
        }
        if (forceSpawnTactical)
        {
            // Spawn Tactical
            if (debug) Debug.Log("Tactical Chest Spawned by Force");
            return true;
        }
        if (forceSpawnHealthUpgrade)
        {
            // Spawn Health Upgrade
            if (debug) Debug.Log("Health Upgrade Chest Spawned by Force");
            return true;
        }
        if (forceSpawnBatteryUpgrade)
        {
            // Spawn Battery Upgrade
            if (debug) Debug.Log("Battery Upgrade Chest Spawned by Force");
            return true;
        }
        return false;
    }

    bool SpawnChanceBasedChest()
    {
        int roll = UnityEngine.Random.Range(0, 101);

        if (roll <= resourceChance) // Add += resourceChanceMult
        {
            // Spawn resource
            if (debug) Debug.Log("Resouce Chest Spawned by Chance");
            return true;
        }
        if (roll <= augmentationChance) // Add += augmentationChanceMult
        {
            // Spawn Augmentation
            if (debug) Debug.Log("Augmentation Spawned by Chance");
        }
        if (roll <= tacticalChance) // Add += ta tacticalChanceMult
        {
            // Spawn tactical
            if (debug) Debug.Log("Tactical Chest Spawned by Chance");
            return true;
        }
        if (roll <= healthChance) // Add += healthChanceMult
        {
            // Spawn health
            if (debug) Debug.Log("Health Upgradce Chest Spawned by Chance");
            return true;
        }
        if (roll <= batteryChance) // Add += batteryChanceMult
        {
            // spawn battery
            if (debug) Debug.Log("Battery Upgrade Chest Spawned by Chance");
            return true;
        }
        return false;
    }
    

}
