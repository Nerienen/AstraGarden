using System;
using System.Collections;
using System.Collections.Generic;
using ProjectUtils.Helpers;
using UnityEngine;

public class PlayerInteract : MonoBehaviour
{
    [SerializeField] private Transform objectGrabPointTransform;
    [SerializeField] private LayerMask interactLayer;

    [field:SerializeReference] public float interactDistance  { get; private set; }

    private Interactable _interactable;
    public bool interacting { get; private set; }

    public static PlayerInteract instance;

    private void Awake()
    {
        if(instance == null) instance = this;
        else Destroy(gameObject);
    }

    private void Update()
    {
        if (Physics.Raycast(Helpers.Camera.transform.position, Helpers.Camera.transform.forward, out var hit, interactDistance, interactLayer))
        {
            if (hit.transform.TryGetComponent(out _interactable) && !interacting && _interactable.showOutline)
            {
                _interactable.GetComponent<Outline>().OutlineWidth = 5;
            }
        }
        else
        {
            if (_interactable != null && !interacting  && _interactable.showOutline)
            {
                _interactable.GetComponent<Outline>().OutlineWidth = 0;
                _interactable = null;
            }
        }
        
        if (Input.GetKeyDown(KeyCode.E) && _interactable != null)
        {
            _interactable.GetComponent<Outline>().OutlineWidth = 0;
            interacting = !interacting;
            _interactable.Interact(objectGrabPointTransform);
        }
    }
}
