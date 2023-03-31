/*
 * Author: Cristion Dominguez
 * Date: 4 Jan. 2023
 */

using UnityEngine;

public class InteractionHandler : MonoBehaviour
{
    [SerializeField] Camera _camera;
    [SerializeField] float _interactRange = 5.0f;
    GameObject _interactableObj;

    private void Update()
    {
        if (Physics.Raycast(_camera.transform.position, _camera.transform.forward, out RaycastHit hit, _interactRange, Constants.LayerMask.Interactable))
        {
            // Display "Interact" message.
            _interactableObj = hit.transform.gameObject;
        }
        else
            _interactableObj = null;
    }

    public void AttemptInteraction()
    {
        if (_interactableObj)
            _interactableObj.GetComponent<IInteractable>().Interact(gameObject);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == Constants.Layer.Touchable)
            other.GetComponent<ITouchable>().Touch(gameObject);
    }
}
