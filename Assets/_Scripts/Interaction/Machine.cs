using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Machine : MonoBehaviour
{
    [SerializeField] private Liquid _energyLiquid;
    [SerializeField] private Liquid _waterLiquid;
    [SerializeField] private Liquid _oxygenLiquid;

    [SerializeField] private float waterAmount;
    [SerializeField] private float oxygenAmount;
    [SerializeField] private float energyAmount;

    private Plant.PlantTypes _currentType;

    public float WaterAmount
    {
        get => waterAmount;
        set
        {
            waterAmount = value;
            _currentType = Plant.PlantTypes.WaterPlant;
            _waterLiquid.fillAmount = 0.7f - 0.5f * value / 20;
            if (_waterLiquid.fillAmount >= 0.7f) _waterLiquid.fillAmount = 10;
        }
    }

    public float OxygenAmount {      
        get => oxygenAmount;
        set
        {
            oxygenAmount = value;
            _currentType = Plant.PlantTypes.OxygenPlant;
            _oxygenLiquid.fillAmount = 0.7f - 0.5f * value / 20;
            if (_oxygenLiquid.fillAmount >= 0.7f) _oxygenLiquid.fillAmount = 10;
        }
    }
    
    public float EnergyAmount {     
        get => energyAmount;
        set
        {
            energyAmount = value;
            _currentType = Plant.PlantTypes.EnergyPlant;
            _energyLiquid.fillAmount = 0.7f - 0.5f * value / 20;
            if (_energyLiquid.fillAmount >= 0.7f) _energyLiquid.fillAmount = 10;
        } 
    }

    public bool InAnimation { get; private set; }
    public void SetInAnimation()
    {
        InAnimation = true;
    }public void SetOutAnimation()
    {
        InAnimation = false;
        GetComponent<HoldPoint>().CurrentHoldenObject.grabbable = true;
    }
    
    public void ChangePlantType()
    {
        GetComponent<HoldPoint>().CurrentHoldenObject.GetComponent<Plant>().ChangeType(_currentType);
    }

    private void Start()
    {
        WaterAmount = waterAmount;
        OxygenAmount = oxygenAmount;
        EnergyAmount = energyAmount;
    }

    public float GetFillAmount(Plant.PlantTypes types)
    {
        return types switch
        {
            Plant.PlantTypes.WaterPlant => WaterAmount,
            Plant.PlantTypes.OxygenPlant => OxygenAmount,
            Plant.PlantTypes.EnergyPlant => EnergyAmount,
        };
    }
}
