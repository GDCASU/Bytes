using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class ContainerSpawnPad : SpawnPad
{
    [Header("Chest Prefabs")]
    public GameObject ammoBox;
    public GameObject heathBox;
    public GameObject keycardBox;

    [Header("Force Spawn")]
    public bool forceSpawnAmmo;
    public bool forceSpawnHealth;
    public bool forceSpawnKeycard;

    [Header("Default Chances")]
    [Range(0, 100)] public float ammoChance;
    [Range(0, 100)] public float healthChance;
    [Range(0, 100)] public float keycardChance;

    private bool containerSpawnedbyForce, containerSpawnedbyChance;

    void Start()
    {
        type = PadType.Container;
    }

    public bool CheckForceSpawn(LootCode lootCode)
    {
        if (forceSpawnAmmo || lootCode == LootCode.Ammo)
        {
            Instantiate(ammoBox, gameObject.transform.position, Quaternion.identity, gameObject.transform);       // Spawn resource chest
            if (debug) Debug.Log("Ammo Box Spawned by Force");
            return true;
        }
        if (forceSpawnHealth || lootCode == LootCode.Health)
        {
            Instantiate(heathBox, gameObject.transform.position, Quaternion.identity, gameObject.transform);           // Spawn Augmentation
            if (debug) Debug.Log("Health Box Spawned by Force");
            return true;
        }
        if (forceSpawnKeycard || lootCode == LootCode.Keycard)
        {
            Instantiate(keycardBox, gameObject.transform.position, Quaternion.identity, gameObject.transform);       // Spawn Tactical
            if (debug) Debug.Log("KeycardBox Spawned by Force");
            return true;
        }
        return false;
    }

    public bool SpawnChanceBasedContainer(LootCode lootCode, int increasedChance)
    {
        int roll = UnityEngine.Random.Range(0, 100);

        if (lootCode == LootCode.Ammo)
        {
            if ((roll <= ammoChance + increasedChance) && (ammoChance > 0))
            {
                Instantiate(ammoBox, gameObject.transform.position, Quaternion.identity, gameObject.transform);           // Spawn resource
                if (debug) Debug.Log($"Ammo Box Spawned by Chance: {roll}%");
                return true;
            }
        }
        if (lootCode == LootCode.Health)
        {
            if ((roll <= healthChance + increasedChance) && (healthChance > 0))
            {
                Instantiate(heathBox, gameObject.transform.position, Quaternion.identity, gameObject.transform);       // Spawn Augmentation
                if (debug) Debug.Log($"Health Box Spawned by Chance: {roll}%");
                return true;
            }
        }
        if (lootCode == LootCode.Keycard)
        {
            if ((roll <= keycardChance + increasedChance) && (keycardChance > 0))
            {
                Instantiate(keycardBox, gameObject.transform.position, Quaternion.identity, gameObject.transform);       // Spawn tactical
                if (debug) Debug.Log($"Keycard Box Spawned by Chance: {roll}%");
                return true;
            }
        }
        return false;
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireCube(transform.position, new Vector3(1, 0.5f, 1));
    }
}
