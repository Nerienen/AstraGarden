using System;
using System.Collections;
using System.Collections.Generic;
using ProjectUtils.Helpers;
using UnityEngine;

public class PlayerInteract : MonoBehaviour
{
    [SerializeField] private Transform objectGrabPointTransform;
    [SerializeField] private LayerMask interactLayer;
    [SerializeField] private LayerMask holdersLayer;

    [field:SerializeReference] public float interactDistance  { get; private set; }

    private Interactable _interactable;
    private HoldPoint _holdPoint;
    public bool interacting { get; set; }

    public static PlayerInteract instance;

    private void Awake()
    {
        if(instance == null) instance = this;
        else Destroy(gameObject);
    }

    private void Update()
    {
        if (Physics.Raycast(Helpers.Camera.transform.position, Helpers.Camera.transform.forward, out var hit, interactDistance, interacting? holdersLayer:interactLayer))
        {
            if (interacting && hit.transform.TryGetComponent(out _holdPoint))
            {
                _holdPoint.GetComponent<Outline>().OutlineWidth = 5;
            }
            else if (hit.transform.TryGetComponent(out _interactable) && _interactable.showOutline)
            {
                _interactable.GetComponent<Outline>().OutlineWidth = 5;
            }
        }
        else
        {
            if (interacting && _holdPoint != null)
            {
                _holdPoint.GetComponent<Outline>().OutlineWidth = 0;
                _holdPoint = null;
            }
            else if (_interactable != null && !interacting  && _interactable.showOutline)
            {
                _interactable.GetComponent<Outline>().OutlineWidth = 0;
                _interactable = null;
            }
        }
        
        if (Input.GetKeyDown(KeyCode.E) && _interactable != null)
        {
            _interactable.GetComponent<Outline>().OutlineWidth = 0;
            _interactable.Interact(objectGrabPointTransform);

            if (_holdPoint != null)
            {
                _holdPoint.GetComponent<Outline>().OutlineWidth = 0;
                _holdPoint.HoldObject(_interactable.transform);
                _holdPoint = null;
            }
        }
    }
}
