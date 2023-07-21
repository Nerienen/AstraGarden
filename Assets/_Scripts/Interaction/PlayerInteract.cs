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
    private IInspectable _inspectable;
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
            ManageInspectable(hit);

            var lastHoldPoint = _holdPoint;
            var lastInteractable = _interactable;
            
            var hold = hit.transform.GetComponent<HoldPoint>();
            bool holdIsNull = hold == null || hold.IsHoldingObject;
            if (!holdIsNull) _holdPoint = hold;
            
            //Display outline if watching to an object
            if (!interacting && hit.transform.TryGetComponent(out _interactable))
            {
                _interactable.SetOutlineWidth(5);
            }

            if(lastHoldPoint != null && lastHoldPoint != _holdPoint) lastHoldPoint.SetOutlineWidth(0);
            if (lastInteractable != null && lastInteractable != _interactable)
            {
                lastInteractable.SetOutlineWidth(0);
                if(!holdIsNull) _holdPoint = null;
            }
            
            if (interacting && !holdIsNull)
            {
                _holdPoint.SetOutlineWidth(5, _interactable);
            }
        }
        else
        {
            if (_inspectable != null)
            {
             _inspectable.StopInspecting();
             _inspectable = null;
            }
            
            if (_holdPoint != null)
            {
                _holdPoint.SetOutlineWidth(0);
                _holdPoint = null;
            }
            if (!interacting && _interactable != null)
            {
                _interactable.SetOutlineWidth(0);
                _interactable = null;
            }
        }
        
        if (Input.GetKeyDown(KeyCode.E) && _interactable != null)
        {
            _interactable.SetOutlineWidth(0);
            if(!_interactable.Interact(objectGrabPointTransform))
                _interactable.Interact();

            if (_holdPoint != null)
            {
                Plant plant = _interactable.GetComponent<Plant>();
                if(plant == null && _holdPoint.justForPlants) return;
                
                _holdPoint.SetOutlineWidth(0);
                _holdPoint.HoldObject(_interactable.transform);
                _holdPoint = null;
            }
        }
    }

    private void ManageInspectable(RaycastHit hit)
    {
        IInspectable inspectable = hit.transform.GetComponent<IInspectable>();
        if (Input.GetKey(KeyCode.Tab))
        {
            if (_inspectable != null && inspectable != null && inspectable != _inspectable)
            {
                _inspectable.StopInspecting();
                _inspectable = inspectable;
            }
            else if (inspectable != null)
            {
                _inspectable = inspectable;
                _inspectable.Inspect(-Helpers.Camera.transform.forward);
            }
        }
        else if (_inspectable != null)
        {
            _inspectable.StopInspecting();
            _inspectable = null;
        }
    }

    private bool CheckIfInteracting(bool hold, bool interact)
    {
        if (interacting) return false;
        bool res = false;
        
        if (hold)
        {
            _holdPoint.SetOutlineWidth(0);
            _holdPoint = null;
            res = true;
        }
        if (interact)
        {
            _interactable.SetOutlineWidth(0);
            _interactable = null;
            res = true;
        }

        return res;
    }
}
