using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.ProBuilder.Shapes;
public class LootGenerator : MonoBehaviour
{
    #region Variables
    [Header("Chests")]
    public int increasedResourceChance = 0;
    public int increasedAugmentationChance = 0;
    public int increasedTacticalChance = 0;
    public int increasedHealthUpgradeChance = 0;
    public int increasedBatteryUpgradeChance = 0;

    [Header("Containers")]
    public int increasedAmmoBoxChance = 0;
    public int increasedHealthBoxChance = 0;
    public int increasedKeycardBoxChance = 0;

    List<GameObject> masterRooms;

    int resourceChanceMult, augmentationChanceMult, tacticalChanceMult,
            healthUpgradeChanceMult, batteryUpgradeChanceMult;             // Multipliers for chest chance increase (chest chances will increase with each room generation)

    int ammoChanceMult, healthChanceMult, keycardChanceMult;

    bool keycardSpawn;
    #endregion

    void Start()
    {
        resourceChanceMult = 0;
        augmentationChanceMult = 0;
        tacticalChanceMult = 0;
        healthUpgradeChanceMult = 0;
        batteryUpgradeChanceMult = 0;

        ammoChanceMult = 0;
        healthChanceMult = 0;
        keycardChanceMult = 0;

        keycardSpawn = false;
    }

    #region Spawn Loot Procedure
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

            foreach (GameObject room in masterRooms)    // Look for ToBoss room
            {
                Room room_s = room.GetComponent<Room>();
                if (room_s.roomType == RoomType.ToBoss)
                    ActivateToBossSpawners(room_s);
            }
        }
    }

    void ActivateSpawners(Room room, RoomType roomType)
    {
        if (room.spawnPads.Count > 0) // Checking for atleast one spawnPad
        {
            foreach (SpawnPad spawnPad in room.spawnPads)
            {
                if (spawnPad is ChestSpawnPad)
                {
                    ChestSpawnPad chestSpawnPad = spawnPad as ChestSpawnPad;
                    switch (roomType)
                    {
                        case RoomType.General:
                            GeneralRoomChestSpawnCase(chestSpawnPad);
                            break;
                        case RoomType.Augmentation:
                            AugmentationRoomSpawnCase(chestSpawnPad);
                            break;
                        case RoomType.Keycard:
                            KeycardRoomChestSpawnCase(chestSpawnPad);
                            break;
                        case RoomType.Trial:
                            TrialRoomChestSpawnCase(chestSpawnPad);
                            break;
                        case RoomType.ToBoss:
                            GeneralRoomChestSpawnCase(chestSpawnPad);
                            break;
                    }
                }
                else if (spawnPad is ContainerSpawnPad)
                {
                    ContainerSpawnPad containerSpawnPad = spawnPad as ContainerSpawnPad;
                    switch (roomType)
                    {
                        case RoomType.General:
                            GeneralRoomContainerSpawnCase(containerSpawnPad);
                            break;
                        case RoomType.Keycard:
                            KeycardRoomContainerSpawnCase(containerSpawnPad);
                            break;
                        case RoomType.Trial:
                            TrialRoomContainerSpawnCase(containerSpawnPad);
                            break;
                    }
                }
            }
        }
    }

    void ActivateToBossSpawners(Room room)
    {
        foreach (SpawnPad spawnPad in room.spawnPads)
        {
            if (spawnPad is ChestSpawnPad)
            {
                ChestSpawnPad chestSpawnPad = spawnPad as ChestSpawnPad;
                GeneralRoomChestSpawnCase(chestSpawnPad);
            }
            else if (spawnPad is ContainerSpawnPad)
            {
                ContainerSpawnPad containerSpawnPad = spawnPad as ContainerSpawnPad;
                ToBossRoomContianerSpawnCase(containerSpawnPad);
            }
            else if (spawnPad is TeleporterSpawnPad)
            {
                TeleporterSpawnPad teleporterSpawnPad = spawnPad as TeleporterSpawnPad;
                teleporterSpawnPad.SpawnTeleporter();
            }
        }
    }
    #endregion

    #region Spawn Cases
    void GeneralRoomChestSpawnCase(ChestSpawnPad spawnPad)
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

        if (spawnPad.SpawnChanceBasedChest(LootCode.Tactical, tacticalChanceMult))
        {
            tacticalChanceMult = 0;
            return;
        }
        else
            tacticalChanceMult += increasedTacticalChance;

    }

    void GeneralRoomContainerSpawnCase(ContainerSpawnPad spawnPad)
    {
        if (spawnPad.CheckForceSpawn(LootCode.None))
            return;

        if (!keycardSpawn && spawnPad.SpawnChanceBasedContainer(LootCode.Keycard, keycardChanceMult))
        {
            keycardChanceMult = 0;
            keycardSpawn = true;
            return;
        }
        else
            keycardChanceMult += increasedKeycardBoxChance;

        if (spawnPad.SpawnChanceBasedContainer(LootCode.Health, healthChanceMult))
        {
            healthChanceMult = 0;
            return;
        }
        else
            healthChanceMult += increasedHealthBoxChance;

        if (spawnPad.SpawnChanceBasedContainer(LootCode.Ammo, ammoChanceMult))
        {
            ammoChanceMult = 0;
            return;
        }
        else
            ammoChanceMult += increasedAmmoBoxChance;
    }

    bool augmentationFlag_A;
    void AugmentationRoomSpawnCase(ChestSpawnPad spawnPad)
    {
        if (!augmentationFlag_A)
        {
            spawnPad.CheckForceSpawn(LootCode.Augmentation);
            augmentationFlag_A = true;
        }
    }

    bool tacticalFlag_K;
    void KeycardRoomChestSpawnCase(ChestSpawnPad spawnPad)
    {
        if (!tacticalFlag_K)
        {
            spawnPad.CheckForceSpawn(LootCode.Tactical);
            tacticalFlag_K = true;
        }
        else
        {
            int roll = UnityEngine.Random.Range(0, 101);
            if (roll <= 50)
            {
                spawnPad.CheckForceSpawn(LootCode.HealthUp);
            }
            else
                spawnPad.CheckForceSpawn(LootCode.BatteryUp);
        }
    }

    bool healthFlag_K;
    void KeycardRoomContainerSpawnCase(ContainerSpawnPad spawnPad)
    {
        if (!healthFlag_K)
        {
            spawnPad.CheckForceSpawn(LootCode.Health);
            healthFlag_K = true;
        }
        else
            spawnPad.CheckForceSpawn(LootCode.Ammo);
    }

    bool augmentationFlag_T;
    void TrialRoomChestSpawnCase(ChestSpawnPad spawnPad)
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

    bool healthFlag_T;
    void TrialRoomContainerSpawnCase(ContainerSpawnPad spawnPad)
    {
        if (!healthFlag_T)
        {
            spawnPad.CheckForceSpawn(LootCode.Health);
            healthFlag_T = true;
        }
        else
        {
            if (spawnPad.SpawnChanceBasedContainer(LootCode.Keycard, increasedKeycardBoxChance))
            {
                keycardChanceMult = 0;
                keycardSpawn = true;
            }
            else
            {
                spawnPad.CheckForceSpawn(LootCode.Ammo);
            }
        }
    }

    void ToBossRoomContianerSpawnCase(ContainerSpawnPad spawnPad)
    {
        if (!keycardSpawn)
        {
            spawnPad.CheckForceSpawn(LootCode.Keycard);
            keycardChanceMult = 0;
            keycardSpawn = true;
        }
        else
        {
            if (spawnPad.CheckForceSpawn(LootCode.None))
                return;

            if (spawnPad.SpawnChanceBasedContainer(LootCode.Health, healthChanceMult))
            {
                healthChanceMult = 0;
                return;
            }
            else
                batteryUpgradeChanceMult += increasedBatteryUpgradeChance;

            if (spawnPad.SpawnChanceBasedContainer(LootCode.Ammo, ammoChanceMult))
            {
                ammoChanceMult = 0;
                return;
            }
            else
            {
                ammoChanceMult += increasedAmmoBoxChance;
                healthChanceMult += increasedHealthBoxChance;
                keycardChanceMult += increasedKeycardBoxChance;
            }
        }
    }
    #endregion
}
