using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityController : MonoBehaviour
{
    private Queue<EquipableEntity> abilities;
    [SerializeField]
    [Range(0, 5)] private int maxAbilities = 3;

    private void Awake()
    {
        abilities = new Queue<EquipableEntity>();
    }

    public void TakeNewAbility(EquipableEntity newAbility)
    {
        foreach (EquipableEntity ability in abilities)
        {
            if (ability.GetComponent<EquipableEntity>().abilityType == newAbility.abilityType)
            {
                print($"The {newAbility.abilityType} ability is already with the player.");
                return;
            }
        }

        if (abilities.Count < maxAbilities)
        {
            abilities.Enqueue(newAbility);
        }
        else
        {
            DisableAbility(abilities.Dequeue(), newAbility.transform.position);
            abilities.Enqueue(newAbility);
        }
        newAbility.gameObject.SetActive(false);

        switch (newAbility.abilityType)
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

    private void DisableAbility(EquipableEntity oldAbility, Vector3 newAbilityPos)
    {
        switch (oldAbility.abilityType)
        {
            case EquipableEntity.AbilityType.Dash:
                this.GetComponent<PlayerController>().EnableDash(false);
                oldAbility.transform.position = newAbilityPos;
                oldAbility.gameObject.SetActive(true);
                print("Dash disabled");
                break;
            default:
                Debug.LogError("Unknown ability type received...");
                break;
        }
    }
}
