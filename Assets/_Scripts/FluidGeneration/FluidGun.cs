using System;
using System.Collections;
using System.Collections.Generic;
using FMODUnity;
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
    
    [SerializeField] private float dropGrowSpeed;
    [SerializeField] private float attackSpeed;
    [SerializeField] private float changeSpeed;
    private float _lastChange;
    private float _lastTimeAttacked;

    [Header("Physics")] 
    [SerializeField] private LayerMask ignoredLayers;
    
    public Drop CurrentDrop { get; private set;}
    private AmmoType _currentAmmoType = AmmoType.WaterAmmo;
    
    private StudioEventEmitter _emitter;

    public event Action<AmmoType, float, float> OnAmmoChanged; 
    public event Action<AmmoType> OnChangeType;

    private void Start()
    {
        _emitter = GetComponent<StudioEventEmitter>();
        _lastChange = float.MinValue;
        _lastTimeAttacked = float.MinValue;

        //Initialize Viusals
        OnAmmoChanged?.Invoke(AmmoType.WaterAmmo, waterAmmo, maxWaterAmmo);
        OnAmmoChanged?.Invoke(AmmoType.EnergyAmmo, energyAmmo, maxEnergyAmmo);
        OnChangeType?.Invoke(_currentAmmoType);

        ResourcesController.Instance.OnFluidCollected += ReloadAmmo;
    }

    private void Update()
    {
        if (Input.mouseScrollDelta.y != 0 && CurrentDrop == null && Time.timeScale > 0 && Time.time - _lastChange >= changeSpeed)
        {
            _lastChange = Time.time;
            ChangeCurrentAmmoType();
            
            //Play change type sound
            _emitter.Play();
        }
    }

    #region DropCharging

    public void ChargeDrop()
    {
        if(CurrentDrop == null) return;
        
        switch (_currentAmmoType)
        {
            case AmmoType.WaterAmmo:
                ChargeWaterDrop();
                break;
            case AmmoType.EnergyAmmo:
                ChargeEnergyDrop();
                break;
        }
    }

    private void ChargeEnergyDrop()
    {
        if(energyAmmo <= 0) return;

        energyAmmo -= CurrentDrop.Grow(dropGrowSpeed);
        if (energyAmmo < 0.01f)    {
            energyAmmo = 0;
            CurrentDrop.emitterCharge.Stop();
        }
        
        OnAmmoChanged?.Invoke(AmmoType.EnergyAmmo, energyAmmo, maxEnergyAmmo);
    }
    
    private void ChargeWaterDrop()
    {
        if(waterAmmo <= 0) return;

        waterAmmo -= CurrentDrop.Grow(dropGrowSpeed);
        if (waterAmmo < 0.01f)
        {
            waterAmmo = 0;
            CurrentDrop.emitterCharge.Stop();
        }

        OnAmmoChanged?.Invoke(AmmoType.WaterAmmo, waterAmmo, maxWaterAmmo);
    }

    #endregion
    
    public void ShootDrop()
    {
        if(Physics.Raycast(Helpers.Camera.transform.position, Helpers.Camera.transform.forward, out var hit,Mathf.Infinity, ~ignoredLayers))
        {
            _lastTimeAttacked = Time.time;
            Vector3 dir = hit.point - shootPoint.position;
            dir.Normalize();
            
            CurrentDrop.Throw(dir, shootingPower);
            DisplaySounds();
            CurrentDrop = null;
        }
    }

    #region DropInstantiation

    public void InstantiateDrop()
    {
        if(Time.time - _lastTimeAttacked < attackSpeed) return;

        switch (_currentAmmoType)
        {
            case AmmoType.WaterAmmo:
                InstantiateWaterDrop();
                break;
            case AmmoType.EnergyAmmo:
                InstantiateEnergyDrop();
                break;
        }

        if(CurrentDrop != null) CurrentDrop.Initialize(shootPoint);
    }

    private void InstantiateWaterDrop()
    {
        if(waterAmmo <= 0) return;
        waterAmmo -= 0.1f;
        if (waterAmmo < 0.01f) waterAmmo = 0;
            
        OnAmmoChanged?.Invoke(AmmoType.WaterAmmo, waterAmmo, maxWaterAmmo);
        CurrentDrop = ObjectPool.Instance.InstantiateFromPool(waterDropPrefab, shootPoint.position, Quaternion.identity).GetComponent<Drop>();
    }

    private void InstantiateEnergyDrop()
    { 
        if(energyAmmo <= 0) return;
        energyAmmo -= 0.1f;
        if (energyAmmo < 0.01f) energyAmmo = 0;
            
        OnAmmoChanged?.Invoke(AmmoType.EnergyAmmo, energyAmmo, maxEnergyAmmo);
        CurrentDrop = ObjectPool.Instance.InstantiateFromPool(energyDropPrefab, shootPoint.position, Quaternion.identity).GetComponent<Drop>();
    }

    #endregion
   

    public void ReloadAmmo(AmmoType ammoType, float quantity)
    {
        switch (ammoType)
        {
            case AmmoType.WaterAmmo:
                waterAmmo = Mathf.Clamp(waterAmmo + quantity, 0, maxWaterAmmo);
                OnAmmoChanged?.Invoke(AmmoType.WaterAmmo, waterAmmo, maxWaterAmmo);
                break;
            case AmmoType.EnergyAmmo:
                energyAmmo = Mathf.Clamp(energyAmmo + quantity, 0, maxEnergyAmmo);
                OnAmmoChanged?.Invoke(AmmoType.EnergyAmmo, energyAmmo, maxEnergyAmmo);
                break;
        }
    }

    private void ChangeCurrentAmmoType()
    {
        int currentValue = (int) _currentAmmoType;
        _currentAmmoType = (AmmoType) ((currentValue + 1) % 2);
        
        //Change visuals
        OnChangeType?.Invoke(_currentAmmoType);
    }
    
    private void DisplaySounds()
    {
        if(AudioManager.instance == null) return;
        
        switch (_currentAmmoType)
        {
            case AmmoType.WaterAmmo:
                AudioManager.instance.PlayOneShot(FMODEvents.instance.shootWater, transform.position);
                break;
            case AmmoType.EnergyAmmo:
                AudioManager.instance.PlayOneShot(FMODEvents.instance.shootEnergy, transform.position);
                break;
        }
    }
}


