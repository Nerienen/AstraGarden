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
               Outline outline = _holdPoint.GetComponent<Outline>();
               outline.OutlineWidth = 5;
               outline.OutlineColor = Color.white;
            }
            else if (hit.transform.TryGetComponent(out _interactable))
            {
                Outline outline = _interactable.GetComponent<Outline>();
                outline.OutlineWidth = 5;
                outline.OutlineColor = _interactable.outlineColor;
            }
        }
        else
        {
            if (interacting && _holdPoint != null)
            {
                _holdPoint.GetComponent<Outline>().OutlineWidth = 0;
                _holdPoint = null;
            }
            else if (_interactable != null && !interacting)
            {
                _interactable.GetComponent<Outline>().OutlineWidth = 0;
                _interactable = null;
            }
        }
        
        if (Input.GetKeyDown(KeyCode.E) && _interactable != null)
        {
            _interactable.GetComponent<Outline>().OutlineWidth = 0;
            if(!_interactable.Interact(objectGrabPointTransform))
                _interactable.Interact();

            if (_holdPoint != null)
            {
                _holdPoint.GetComponent<Outline>().OutlineWidth = 0;
                _holdPoint.HoldObject(_interactable.transform);
                _holdPoint = null;
            }
        }
    }
}
