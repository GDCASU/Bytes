using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class ChestSpawnPad : SpawnPad
{
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
        type = PadType.Chest;
    }

    public bool CheckForceSpawn(LootCode lootCode)
    {
        if (forceSpawnResource || lootCode == LootCode.Resource)
        {
            Instantiate(resourceChest, gameObject.transform.position, Quaternion.identity, gameObject.transform);       // Spawn resource chest
            if (debug) Debug.Log("Resource Chest Spawned by Force");
            return true;
        }
        if (forceSpawnAugmentation || lootCode == LootCode.Augmentation)
        {
            Instantiate(augmentationChest, gameObject.transform.position, Quaternion.identity, gameObject.transform);           // Spawn Augmentation
            if (debug) Debug.Log("Augmentation Chest Spawned by Force");
            return true;
        }
        if (forceSpawnTactical || lootCode == LootCode.Tactical)
        {
            Instantiate(tacticalChest, gameObject.transform.position, Quaternion.identity, gameObject.transform);       // Spawn Tactical
            if (debug) Debug.Log("Tactical Chest Spawned by Force");
            return true;
        }
        if (forceSpawnHealthUpgrade || lootCode == LootCode.HealthUp)
        {
            Instantiate(healthUpgradeChest, gameObject.transform.position, Quaternion.identity, gameObject.transform);       // Spawn Health Upgrade
            if (debug) Debug.Log("Health Upgrade Chest Spawned by Force");
            return true;
        }
        if (forceSpawnBatteryUpgrade || lootCode == LootCode.BatteryUp)
        {
            Instantiate(batteryUpgradeChest, gameObject.transform.position, Quaternion.identity, gameObject.transform);     // Spawn Battery Upgrade
            if (debug) Debug.Log("Battery Upgrade Chest Spawned by Force");
            return true;
        }
        return false;
    }

    public bool SpawnChanceBasedChest(LootCode lootCode, int increasedChance)
    {
        int roll = UnityEngine.Random.Range(0, 100);

        if (lootCode == LootCode.Resource)
        {
            if ((roll <= resourceChance + increasedChance) && (resourceChance > 0))
            {
                Instantiate(resourceChest, gameObject.transform.position, Quaternion.identity, gameObject.transform);           // Spawn resource
                if (debug) Debug.Log($"Resouce Chest Spawned by Chance: {roll}%");
                return true;
            }
        }
        if (lootCode == LootCode.Augmentation)
        {
            if ((roll <= augmentationChance + increasedChance) && (augmentationChance > 0))
            {
                Instantiate(augmentationChest, gameObject.transform.position, Quaternion.identity, gameObject.transform);       // Spawn Augmentation
                if (debug) Debug.Log($"Augmentation Chest Spawned by Chance: {roll}%");
                return true;
            }
        }
        if (lootCode == LootCode.Tactical)
        {
            if ((roll <= tacticalChance + increasedChance) && (tacticalChance > 0))
            {
                Instantiate(tacticalChest, gameObject.transform.position, Quaternion.identity, gameObject.transform);       // Spawn tactical
                if (debug) Debug.Log($"Tactical Chest Spawned by Chance: {roll}%");
                return true;
            }
        }
        if (lootCode == LootCode.HealthUp)
        {
            if ((roll <= healthChance + increasedChance) && (healthChance > 0))
            {
                Instantiate(healthUpgradeChest, gameObject.transform.position, Quaternion.identity, gameObject.transform);       // Spawn health
                if (debug) Debug.Log($"Health Upgrade Chest Spawned by Chance: {roll}%");
                return true;
            }
        }
        if (lootCode == LootCode.BatteryUp)
        {
            if ((roll <= batteryChance + increasedChance) && (batteryChance > 0))
            {
                Instantiate(batteryUpgradeChest, gameObject.transform.position, Quaternion.identity, gameObject.transform);       // spawn battery
                if (debug) Debug.Log($"Battery Upgrade Chest Spawned by Chance: {roll}%");
                return true;
            }
        }

        return false;
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireCube(transform.position, new Vector3(1, 1, 2));
    }
}
