using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.ProBuilder.Shapes;

public class LootGenerator : MonoBehaviour
{
    [Header("Loot")]
    public int increasedResourceChance = 0;
    public int increasedAugmentationChance = 0;
    public int increasedTacticalChance = 0;
    public int increasedHealthUpgradeChance = 0;
    public int increasedBatteryUpgradeChance = 0;

    private List<GameObject> masterRooms;

    int resourceChanceMult, augmentationChanceMult, tacticalChanceMult,
            healthUpgradeChanceMult, batteryUpgradeChanceMult;             // Multipliers for chest chance increase (chest chances will increase with each room generation)

    private void Start()
    {
        resourceChanceMult = 0;
        augmentationChanceMult = 0;
        tacticalChanceMult = 0;
        healthUpgradeChanceMult = 0;
        batteryUpgradeChanceMult = 0;
    }

    public void SpawnLoot()
    {
        masterRooms = MapGenerator.Instance.masterRooms;
        if (masterRooms.Count > 0)     // If atleast one room exists in master rooms list
        {
            foreach (GameObject room in masterRooms)
            {
                Room room_s = room.GetComponent<Room>();
                ActivateSpawners(room_s, room_s.roomType);
            }
        }
    }

    private void ActivateSpawners(Room room, RoomType roomType)
    {
        if (room.spawnPads.Count > 0) // Checking for atleast one spawnPad
        {
            foreach (ChestSpawnPad spawnPad in room.spawnPads)
            {
                switch (roomType)
                {
                    case RoomType.Start:    // No chest spawn in starting room
                        break;
                    case RoomType.General:
                        GeneralRoomChestSpawnCase(spawnPad);
                        break;
                    case RoomType.Augmentation:
                        AugmentationRoomSpawnCase(spawnPad);
                        break;
                    case RoomType.Keycard:
                        KeycardRoomChestSpawnCase(spawnPad);
                        break;
                    case RoomType.Trial:
                        TrialRoomChestSpawnCase(spawnPad);
                        break;
                    case RoomType.ToBoss:
                        break;
                }
            }

            // foreach(ContainerSpawnPad spawnPad in room.spawnPads

        }

    }

    private void GeneralRoomChestSpawnCase(ChestSpawnPad spawnPad)
    {
        if (spawnPad.CheckForceSpawn(LootCode.None))
            return;

        if (spawnPad.SpawnChanceBasedChest(LootCode.HealthUp, healthUpgradeChanceMult))
        {
            healthUpgradeChanceMult = 0;
            return;
        }
        else
            healthUpgradeChanceMult += increasedHealthUpgradeChance;

        if (spawnPad.SpawnChanceBasedChest(LootCode.BatteryUp, batteryUpgradeChanceMult))
        {
            batteryUpgradeChanceMult = 0;
            return;
        }
        else
            batteryUpgradeChanceMult += increasedBatteryUpgradeChance;

        if (spawnPad.SpawnChanceBasedChest(LootCode.Resource, resourceChanceMult))
        {
            resourceChanceMult = 0;
            return;
        }
        else
            resourceChanceMult += increasedResourceChance;
    }

    bool augmentationFlag_A;
    private void AugmentationRoomSpawnCase(ChestSpawnPad spawnPad)
    {
        if (!augmentationFlag_A)
        {
            spawnPad.CheckForceSpawn(LootCode.Augmentation);
            augmentationFlag_A = true;
        }
    }

    bool tacticalFlag_K;
    private void KeycardRoomChestSpawnCase(ChestSpawnPad spawnPad)
    {
        if (!tacticalFlag_K)
            spawnPad.CheckForceSpawn(LootCode.Tactical);

        if (tacticalFlag_K)
        {
            int roll = UnityEngine.Random.Range(0, 101);
            if (roll <= 50)
            {
                spawnPad.CheckForceSpawn(LootCode.HealthUp);
            }
            else
                spawnPad.CheckForceSpawn(LootCode.BatteryUp);
        }
        tacticalFlag_K = true;
    }

    bool augmentationFlag_T;
    private void TrialRoomChestSpawnCase(ChestSpawnPad spawnPad)
    {
        if (!augmentationFlag_T)
            spawnPad.CheckForceSpawn(LootCode.Augmentation);

        if (augmentationFlag_T)
        {
            int roll = UnityEngine.Random.Range(0, 101);
            if (roll <= 50)
            {
                spawnPad.CheckForceSpawn(LootCode.HealthUp);
            }
            else
                spawnPad.CheckForceSpawn(LootCode.BatteryUp);
        }
        augmentationFlag_T = true;
    }

    private void SpawnContainers()
    {

    }
}
