using System;
using System.Collections;
using System.Collections.Generic;
using ProjectUtils.Helpers;
using ProjectUtils.ObjectPooling;
using UnityEngine;

public class FluidGun : MonoBehaviour
{
    [Header("Bindings")]
    [SerializeField] private GameObject waterDropPrefab;
    [SerializeField] private GameObject energyDropPrefab;
    [SerializeField] private Transform shootPoint;

    [Header("Gun parameters")] 
    [SerializeField] private float shootingPower;
    [SerializeField] private float waterAmmo;
    [SerializeField] private float energyAmmo;
    [SerializeField] private float maxWaterAmmo;
    [SerializeField] private float maxEnergyAmmo;
    [field:SerializeReference] public float attackSpeed { get; private set; }
    [SerializeField] private float cadence;
    private float _lastTimeAttacked;

    [Header("Physics")] 
    [SerializeField] private LayerMask ignoredLayers;
    
    public Drop currentDrop { get; private set;}
    private bool _throwsEnergy;

    private void Start()
    {
        _lastTimeAttacked = float.MinValue;
    }

    private void Update()
    {
        if (Input.mouseScrollDelta.y != 0 && currentDrop == null) _throwsEnergy = !_throwsEnergy;
    }

    public void ChargeDrop()
    {

        if (!_throwsEnergy)
        {
            if(waterAmmo <= 0) return;
        
            if (currentDrop == null)
            {
                InstantiateDrop();
                return;
            }

            waterAmmo -= currentDrop.Grow(cadence);
            if (waterAmmo < 0.01f) waterAmmo = 0;
        }
        else
        {
            if(energyAmmo <= 0) return;
        
            if (currentDrop == null)
            {
                InstantiateDrop();
                return;
            }

            energyAmmo -= currentDrop.Grow(cadence);
            if (energyAmmo < 0.01f) energyAmmo = 0;
        }
       
    }

    public void ShootDrop()
    {
        if(Physics.Raycast(Helpers.Camera.transform.position, Helpers.Camera.transform.forward, out var hit,Mathf.Infinity, ~ignoredLayers))
        {
            _lastTimeAttacked = Time.time;
            Vector3 dir = hit.point - shootPoint.position;
            dir.Normalize();
            
            currentDrop.Throw(dir, shootingPower);
            currentDrop = null;
        }
    }

    private void InstantiateDrop()
    {
        if(Time.time - _lastTimeAttacked < attackSpeed) return;
        waterAmmo -= 0.1f;

        if (_throwsEnergy) currentDrop = ObjectPool.Instance.InstantiateFromPool(energyDropPrefab, shootPoint.position, Quaternion.identity).GetComponent<Drop>();
        else currentDrop = ObjectPool.Instance.InstantiateFromPool(waterDropPrefab, shootPoint.position, Quaternion.identity).GetComponent<Drop>();
        
        currentDrop.Initialize(shootPoint);
    }

    public void ReloadAmmo(AmmoType ammoType, float quantity)
    {
        if (ammoType == AmmoType.WaterAmmo)
            waterAmmo = Mathf.Clamp(waterAmmo + quantity, 0, maxWaterAmmo);
        else
            energyAmmo = Mathf.Clamp(energyAmmo + quantity, 0, maxEnergyAmmo);
    }
}

public enum AmmoType{
    WaterAmmo, EnergyAmmo
}
