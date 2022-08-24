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
            if (hit.collider.GetComponent<EquipableEntity>())
            {
                ExamineEntity(hit);
            }
        }
    }

    private void ExamineEntity(RaycastHit hit)
    {
        if (!hit.collider.GetComponent<EquipableEntity>().CheckIfEquiped())
        {
            switch (hit.collider.GetComponent<EquipableEntity>().entityType)
            {
                case EquipableEntity.EntityType.Weapon:
                    print("Got Weapon");
                    this.GetComponent<WeaponHandler>().TakeNewWeapon(hit.transform.GetComponent<IWeapon>());
                    break;
                case EquipableEntity.EntityType.Ability:
                    print("Got Ability");
                    this.GetComponent<AbilityHandler>().TakeNewAbility(hit.transform.GetComponent<EquipableEntity>());
                    break;
                default:
                    Debug.LogError("Equipable entity has an unrecognizable type.");
                    break;
            }
        }
    }
}
