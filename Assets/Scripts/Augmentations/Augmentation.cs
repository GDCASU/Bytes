/*
 * Author: Cristion Dominguez
 * Date: 18 Jan. 2023
 */

using UnityEngine;

public abstract class Augmentation : MonoBehaviour, IInteractable, IEquipable
{
    [SerializeField] protected int batteryCost;
    protected Collider interactCollider;
    protected AugmentationHandler handler;

    protected virtual void Awake()
    {
        interactCollider = GetComponent<Collider>();
    }

    public virtual void Interact(GameObject interactor)
    {
        if (interactor.TryGetComponent(out handler))
            handler.TakeAugmentation(this);
    }

    public abstract bool IsEquipped { get; }
    public abstract void Equip(GameObject equipper);
    public abstract void Unequip();
    public abstract void Trigger(bool inputPressed);
}