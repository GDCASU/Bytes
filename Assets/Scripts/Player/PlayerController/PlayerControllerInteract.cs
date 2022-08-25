using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class PlayerController
{
    [System.Serializable]
    public class InteractVariables
    {
        public float interactRange = 5.0f;
    }

    void InteractInput()
    {
        // Press the 'E' key to interact with a raycast
        if (InputManager.PlayerActions.Interact.WasPerformedThisFrame() && Physics.Raycast(playerCamera.transform.position,
            playerCamera.transform.forward, out RaycastHit hit, interactVariables.interactRange))
        {
            EquipableEntity entity = hit.transform.GetComponent<EquipableEntity>();
            if (entity) ExamineEntity(entity);
        }
    }

    private void ExamineEntity(EquipableEntity entity)
    {
        if (!entity.CheckIfEquiped())
        {
            switch (entity.entityType)
            {
                case EquipableEntity.EntityType.Weapon:
                    print("Got Weapon");
                    this.GetComponent<WeaponHandler>().TakeNewWeapon(entity.GetComponent<Weapon>());
                    break;
                case EquipableEntity.EntityType.Ability:
                    print("Got Ability");
                    this.GetComponent<AbilityHandler>().TakeNewAbility(entity);
                    break;
                default:
                    Debug.LogError("Equipable entity has an unrecognizable type.");
                    break;
            }
        }
    }
}
