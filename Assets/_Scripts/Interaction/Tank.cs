using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tank : Grabbable
{
    [SerializeField] private Liquid _liquid;
    [field: SerializeReference] public Plant.PlantTypes liquidType { get; private set; }
    public float liquidQuantity;

    public event Action onGetLiquid;
    
    public float GetLiquid()
    {
        float res = liquidQuantity;
        liquidQuantity = 0;

        _liquid.fillAmount = 10;
        onGetLiquid?.Invoke();
        return res;
    }
}
