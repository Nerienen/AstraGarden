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

    public float WaterAmount
    {
        get => waterAmount;
        set
        {
            waterAmount = value;
            _waterLiquid.fillAmount = 0.7f - 0.5f * value / 20;
            if (_waterLiquid.fillAmount >= 0.7f) _waterLiquid.fillAmount = 10;
        }
    }

    public float OxygenAmount {      
        get => oxygenAmount;
        set
        {
            oxygenAmount = value;
            _oxygenLiquid.fillAmount = 0.7f - 0.5f * value / 20;
            if (_oxygenLiquid.fillAmount >= 0.7f) _oxygenLiquid.fillAmount = 10;
        }
    }
    
    public float EnergyAmount {     
        get => energyAmount;
        set
        {
            energyAmount = value;
            _energyLiquid.fillAmount = 0.7f - 0.5f * value / 20;
            if (_energyLiquid.fillAmount >= 0.7f) _energyLiquid.fillAmount = 10;
        } 
    }

    private void Start()
    {
        WaterAmount = waterAmount;
        OxygenAmount = oxygenAmount;
        EnergyAmount = energyAmount;
    }
    
}
