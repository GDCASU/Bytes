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
                        break;
                    case RoomType.ToBoss:
                        break;
                }
            }


            // foreach(ContainerSpawnPad spawnPad in room.spawnPads
        }

    }

    public void GeneralRoomChestSpawnCase(ChestSpawnPad spawnPad)
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

    private void SpawnContainers()
    {

    }
}
