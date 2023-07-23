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

    private Plant _currentPlant;
    private Interactable _interactable;
    private HoldPoint _holdPoint;
    private IInspectable _inspectable;
    public bool interacting { get; set; }

    public static PlayerInteract instance;

    public bool seenPlant;
    public bool seenInteract;

    private void Awake()
    {
        if(instance == null) instance = this;
        else Destroy(gameObject);
    }

    private void LateUpdate()
    {
        if (Physics.Raycast(Helpers.Camera.transform.position, Helpers.Camera.transform.forward ,out var hit, interactDistance, interacting? holdersLayer:interactLayer))
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
                if (_interactable.TryGetComponent(out MachineButton button))
                {
                    var holder = _interactable.GetComponentInParent<HoldPoint>();
                    if (holder != null && !holder.IsHoldingObject)
                        _interactable.SetOutlineWidth(0);
                    else
                    {
                        _interactable.SetOutlineWidth(5);
                        Plant plant = holder.CurrentHoldenObject.GetComponent<Plant>();
                        Machine machine = _interactable.GetComponentInParent<Machine>();
                        _interactable.SetOutlineColor(plant.CurrentType == button.buttonType || plant.PlantData.health <= 0 || machine.GetFillAmount(button.buttonType) <= 0 ? Color.red : Color.green);
                    }
                }
                else _interactable.SetOutlineWidth(5);
                
            }

            if (_interactable != null && !seenInteract)
            {
                seenInteract = true;
                UiController.instance.DisplayInteract();
            }
            if (!seenPlant && _interactable.TryGetComponent(out Plant _) && UiController.instance.eDisplayed)
            {
                seenPlant = true;
                UiController.instance.DisplayInspectPlant();
            }
            
            if(lastHoldPoint != null && lastHoldPoint != _holdPoint) lastHoldPoint.SetOutlineWidth(0);
            if (lastInteractable != null && lastInteractable != _interactable)
            {
                lastInteractable.SetOutlineWidth(0);
                if(!holdIsNull) _holdPoint = null;
            }
            
            if (interacting && !holdIsNull)
            {
                if (HandleMachineColor())
                {
                    _holdPoint.SetOutlineWidth(5);
                }
                else _holdPoint.SetOutlineWidth(5, _interactable);
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

            if (!interacting && _currentPlant != null)
            {
                _currentPlant.OnPlantDissolve -= ResetInteraction;
                _currentPlant = null;
            }
            if (interacting && _interactable.TryGetComponent(out _currentPlant))
                _currentPlant.OnPlantDissolve += ResetInteraction;

            if (_holdPoint != null)
            {
                if(HandleTank()) return;
                if(!_interactable.TryGetComponent(out Plant _) && _holdPoint.justForPlants) return;
                
                _holdPoint.SetOutlineWidth(0);
                _holdPoint.HoldObject(_interactable.transform);
                _holdPoint = null;
            }
        }
    }

    private void ResetInteraction()
    {
        _currentPlant.OnPlantDissolve -= ResetInteraction;

        interacting = false;
        _currentPlant = null;
    }

    private bool HandleTank()
    {
        if (_interactable.TryGetComponent(out Tank tank))
        {
            if (_holdPoint.TryGetComponent(out Machine machine))
            {
                switch (tank.liquidType)
                {
                    case Plant.PlantTypes.EnergyPlant:
                        machine.EnergyAmount += tank.GetLiquid();
                        break;
                    case Plant.PlantTypes.OxygenPlant:
                        machine.OxygenAmount += tank.GetLiquid();
                        break;
                    case Plant.PlantTypes.WaterPlant:
                        machine.WaterAmount += tank.GetLiquid();
                        break;
                }
                
                _holdPoint.SetOutlineColor(Color.white);
                return true;
            }
        }

        return false;
    }
    
    private bool HandleMachineColor()
    {
        if (!_interactable.TryGetComponent(out Tank t) || t.liquidQuantity <= 0) return false;
        
        if (_holdPoint.TryGetComponent(out Machine _))
        {
            _holdPoint.SetOutlineColor(Color.green);
        }

        return true;
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
