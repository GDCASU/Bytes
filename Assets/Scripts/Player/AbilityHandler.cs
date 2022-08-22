using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityHandler : MonoBehaviour
{
    private Queue abilityObjects;
    [SerializeField]
    [Range(0, 5)] private int maxAbilities = 3;

    private void Awake()
    {
        abilityObjects = new Queue();
    }

    public void TakeNewAbility(GameObject newAbility)
    {
        foreach (GameObject abilityObj in abilityObjects)
        {
            if (abilityObj.GetComponent<EquipableEntity>().abilityType == newAbility.GetComponent<EquipableEntity>().abilityType)
            {
                print($"The {newAbility.GetComponent<EquipableEntity>().abilityType} ability is already with the player.");
                return;
            }
        }

        if (abilityObjects.Count < maxAbilities)
        {
            abilityObjects.Enqueue(newAbility);
        }
        else
        {
            DisableAbility((GameObject) abilityObjects.Dequeue(), newAbility.transform.position);
            abilityObjects.Enqueue(newAbility);
        }
        newAbility.gameObject.SetActive(false);

        switch (newAbility.GetComponent<EquipableEntity>().abilityType)
        {
            case EquipableEntity.AbilityType.Dash:
                this.GetComponent<PlayerController>().EnableDash(true);
                print("Player can dash!");
                break;
            default:
                Debug.LogError("Unknown ability type received...");
                break;
        }
    }

    private void DisableAbility(GameObject oldAbility, Vector3 newAbilityPos)
    {
        switch (oldAbility.GetComponent<EquipableEntity>().abilityType)
        {
            case EquipableEntity.AbilityType.Dash:
                this.GetComponent<PlayerController>().EnableDash(false);
                oldAbility.transform.position = newAbilityPos;
                oldAbility.SetActive(true);
                print("Dash disabled");
                break;
            default:
                Debug.LogError("Unknown ability type received...");
                break;
        }
    }
}
