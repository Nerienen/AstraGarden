using System.Collections;
using System.Collections.Generic;
using ProjectUtils.Helpers;
using UnityEngine;

public class PlayerInteract : MonoBehaviour
{
    [SerializeField] private Transform objectGrabPointTransform;
    [SerializeField] private LayerMask interactLayer;

    [SerializeField] private float interactDistance;

    private Interactable _interactable;
    public bool interacting { get; private set; }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (_interactable == null)
            {
                if (Physics.Raycast(Helpers.Camera.transform.position, Helpers.Camera.transform.forward, out var hit, interactDistance, interactLayer))
                {
                   if (hit.transform.TryGetComponent(out _interactable))
                   {
                       _interactable.Interact(objectGrabPointTransform);
                       interacting = true;
                   }
                }
            }
            else
            {
                interacting = false;
                _interactable.Interact(objectGrabPointTransform);
                _interactable = null;
            }
        }
    }
}
