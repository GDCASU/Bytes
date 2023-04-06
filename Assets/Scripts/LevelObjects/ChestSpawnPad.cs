using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChestSpawnPad : SpawnPad
{
    [Header("Debug")]
    public bool debug = false;

    [Header("Chest Prefabs")]
    public GameObject resourceChest;
    public GameObject augmentationChest;
    public GameObject tacticalChest;
    public GameObject healthUpgradeChest;
    public GameObject batteryUpgradeChest;

    [Header("Force Spawn")]
    public bool forceSpawnResource;
    public bool forceSpawnAugmentation;
    public bool forceSpawnTactical;
    public bool forceSpawnHealthUpgrade;
    public bool forceSpawnBatteryUpgrade;

    [Header("Default Chances")]
    [Range(0, 100)] public float resourceChance;
    [Range(0, 100)] public float augmentationChance;
    [Range(0, 100)] public float tacticalChance;
    [Range(0, 100)] public float healthChance;
    [Range(0, 100)] public float batteryChance;

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
        float resourceRoll = UnityEngine.Random.Range(0, 100.1f);
        resourceRoll = (float)System.Math.Round(resourceRoll, 2);

        float augmentationRoll = UnityEngine.Random.Range(0, 100.1f);
        augmentationRoll = (float)System.Math.Round(augmentationRoll, 2);

        float tacticalRoll = UnityEngine.Random.Range(0, 100.1f);
        tacticalRoll = (float)System.Math.Round(tacticalRoll, 2);

        float healthRoll = UnityEngine.Random.Range(0, 100.1f);
        healthRoll = (float)System.Math.Round(healthRoll, 2);

        float batteryRoll = UnityEngine.Random.Range(0, 101.1f);
        batteryRoll = (float)System.Math.Round(batteryRoll, 2);

        // resourceRoll = (float)System.Math.Round(resourceRoll * mapGenerator.GetLootMult(LootCode.Resource), 2);
        // augmentationRoll = (float)System.Math.Round(augmentationRoll * mapGenerator.GetLootMult(LootCode.Augmentation), 2);
        // tacticalRoll = (float)System.Math.Round(tacticalRoll * mapGenerator.GetLootMult(LootCode.Tactical), 2);
        // healthRoll = (float)System.Math.Round(healthRoll * mapGenerator.GetLootMult(LootCode.HealthUp), 2);
        // batteryRoll = (float)System.Math.Round(batteryRoll * mapGenerator.GetLootMult(LootCode.BatteryUp), 2);

        if (resourceRoll <= resourceChance)
        {
            // Spawn resource
            if (debug) Debug.Log($"Resouce Chest Spawned by Chance: {resourceRoll}%");
            return true;
        }
        else
            mapGenerator.MultLoot(LootCode.Resource);

        if (augmentationRoll <= augmentationChance)
        {
            // Spawn Augmentation
            if (debug) Debug.Log($"Augmentation Chest Spawned by Chance: {augmentationRoll}%");
        }
        else
            mapGenerator.MultLoot(LootCode.Augmentation);

        if (tacticalRoll <= tacticalChance)
        {
            // Spawn tactical
            if (debug) Debug.Log($"Tactical Chest Spawned by Chance: {tacticalRoll}%");
            return true;
        }
        else
            mapGenerator.MultLoot(LootCode.Tactical);

        if (healthRoll <= healthChance)
        {
            // Spawn health
            if (debug) Debug.Log($"Health Upgrade Chest Spawned by Chance: {healthRoll}%");
            return true;
        }
        else
            mapGenerator.MultLoot(LootCode.HealthUp);

        if (batteryRoll <= batteryChance)
        {
            // spawn battery
            if (debug) Debug.Log($"Battery Upgrade Chest Spawned by Chance: {batteryRoll}%");
            return true;
        }
        else
            mapGenerator.MultLoot(LootCode.BatteryUp);

        return false;
    }
    

}
