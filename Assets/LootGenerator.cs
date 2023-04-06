using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LootGenerator : MonoBehaviour
{
    private List<GameObject> masterRooms;

    [Header("Loot")]
    public float increasedResourceChance = 0;
    public float increasedAugmentationChance = 0;
    public float increasedTacticalChance = 0;
    public float increasedHealthUpgradeChance = 0;
    public float increasedBatteryUpgradeChance = 0;

    float resourceChanceMult, augmentationChanceMult, tacticalChanceMult,
            healthUpgradeChanceMult, batteryUpgradeChanceMult;             // Multipliers for chest chance increase (chest chances will increase with each room generation)

    private void Start()
    {
        masterRooms = MapGenerator.Instance.masterRooms;

        resourceChanceMult = 0;
        augmentationChanceMult = 0;
        tacticalChanceMult = 0;
        healthUpgradeChanceMult = 0;
        batteryUpgradeChanceMult = 0;
    }

    public void SpawnLoot()
    {
        foreach (GameObject room in masterRooms)
        {
            // Write iterative code in here
        }
    }

    #region Loot
    public void MultLoot(LootCode code)
    {
        switch (code)
        {
            case LootCode.Resource:
                resourceChanceMult += increasedResourceChance;
                break;
            case LootCode.Augmentation:
                augmentationChanceMult += increasedAugmentationChance;
                break;
            case LootCode.Tactical:
                tacticalChanceMult += increasedTacticalChance;
                break;
            case LootCode.HealthUp:
                healthUpgradeChanceMult += increasedHealthUpgradeChance;
                break;
            case LootCode.BatteryUp:
                batteryUpgradeChanceMult += increasedBatteryUpgradeChance;
                break;
            default:
                Debug.Log("Error: specified loot code does not exist in MultLoot().");
                break;
        }
    }

    public float GetLootMult(LootCode code)
    {
        switch (code)
        {
            case LootCode.Resource:
                return resourceChanceMult;
            case LootCode.Augmentation:
                return augmentationChanceMult;
            case LootCode.Tactical:
                return tacticalChanceMult;
            case LootCode.HealthUp:
                return healthUpgradeChanceMult;
            case LootCode.BatteryUp:
                return batteryUpgradeChanceMult;
            default:
                Debug.Log("Error: specified loot code does not exist in GetLootMult().");
                return 0;
        }
    }
    #endregion
}
