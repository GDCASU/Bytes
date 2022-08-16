using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class PlayerController
{
    [System.Serializable]
    public class InteractVariables
    {
        [HideInInspector] public Collider[] hitColliders;

        public float interactRadius = 5.0f;
        public int maxColliders = 10;
    }

    void InteractInput()
    {
        if (InputManager.PlayerActions.Interact.WasPerformedThisFrame())
        {
            interactVariables.hitColliders = new Collider[interactVariables.maxColliders];
            int collidersHit = Physics.OverlapSphereNonAlloc(this.gameObject.transform.position, 
                interactVariables.interactRadius, interactVariables.hitColliders);
            for (int i = 0; i < collidersHit; i++)
            {
                if (interactVariables.hitColliders[i].GetComponent<EquipableEntity>())
                {
                    ExamineEntity(i);
                }
            }
        }
    }

    private void ExamineEntity(int i)
    {
        if (!interactVariables.hitColliders[i].GetComponent<EquipableEntity>().CheckIfEquiped())
        {
            switch (interactVariables.hitColliders[i].GetComponent<EquipableEntity>().entityType)
            {
                case EquipableEntity.EntityType.Weapon:
                    print("Test Weapon");
                    this.GetComponent<WeaponHandler>().AddNewWeapon(interactVariables.hitColliders[i].gameObject);
                    return;
                case EquipableEntity.EntityType.Ability:
                    print("Test Ability");

                    return;
                default:
                    Debug.LogError("Equipable entity has an unrecognizable type.");
                    return;
            }
        }
    }
}
