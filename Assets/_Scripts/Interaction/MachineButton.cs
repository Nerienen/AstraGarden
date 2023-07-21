using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MachineButton : Interactable
{
    private HoldPoint _holdPoint;
    private Machine _machine;
    private Animator _animator;
    [field: SerializeField] public Plant.PlantTypes buttonType { get; private set; }
    
    private void Start()
    {
        _holdPoint = GetComponentInParent<HoldPoint>();
        _animator = GetComponent<Animator>();
        _machine = GetComponentInParent<Machine>();
    }

    public override bool Interact()
    {
        if(!_holdPoint.IsHoldingObject) return false;

        _animator.ResetTrigger("Press");
        _animator.SetTrigger("Press");

        Plant plant = _holdPoint.CurrentHoldenObject.GetComponent<Plant>();
        if (plant.CurrentType == buttonType || plant.PlantData.health <= 0) return false;
        
        switch (buttonType)
        {
            case Plant.PlantTypes.WaterPlant:
                if (_machine.WaterAmount <= 0) return false;
                _machine.WaterAmount--;
                if (_machine.WaterAmount <= 0) SetOutlineColor(Color.red);
                break;
            case Plant.PlantTypes.EnergyPlant:
                if (_machine.EnergyAmount <= 0) return false;
                _machine.EnergyAmount--;
                if (_machine.EnergyAmount <= 0) SetOutlineColor(Color.red);
                break;
            case Plant.PlantTypes.OxygenPlant:
                if (_machine.OxygenAmount <= 0) return false;
                _machine.OxygenAmount--;
                if (_machine.OxygenAmount <= 0) SetOutlineColor(Color.red);
                break;
        }
        
        _holdPoint.CurrentHoldenObject.GetComponent<Plant>().ChangeType(buttonType);
        return true;
    }

    public override bool Interact(Transform grabPoint)
    {
        return false;
    }
}
