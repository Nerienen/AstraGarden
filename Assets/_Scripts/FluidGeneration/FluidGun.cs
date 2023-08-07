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
    [SerializeField] private Player_InputManager inputManager;
    [SerializeField] private Transform shootPoint;

    [Header("Ammo")]
    [SerializeField] private AmmoTypeAmmoDataDictionary ammunition;
    
    [Header("Gun parameters")]
    [SerializeField] private float shootingPower;
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

    private void OnEnable()
    {
        inputManager.OnScroll += OnScroll;
    }

    private void OnDisable()
    {
        inputManager.OnScroll -= OnScroll;
    }

    private void Start()
    {
        _emitter = GetComponent<StudioEventEmitter>();
        _lastChange = float.MinValue;
        _lastTimeAttacked = float.MinValue;

        //Initialize Viusals
        OnAmmoChanged?.Invoke(AmmoType.WaterAmmo, ammunition[AmmoType.WaterAmmo].currentAmmo, ammunition[AmmoType.WaterAmmo].maxAmmo);
        OnAmmoChanged?.Invoke(AmmoType.EnergyAmmo,  ammunition[AmmoType.EnergyAmmo].currentAmmo, ammunition[AmmoType.EnergyAmmo].maxAmmo);
        OnChangeType?.Invoke(_currentAmmoType);

        ResourcesController.Instance.OnFluidCollected += ReloadAmmo;
    }

    public void ChargeDrop()
    {
        if(CurrentDrop == null) return;
        AmmoData ammoData = ammunition[_currentAmmoType];
        
        if(ammoData.currentAmmo <= 0) return;

        ammoData.currentAmmo -= CurrentDrop.Grow(dropGrowSpeed);
        if (ammoData.currentAmmo < 0.01f)    {
            ammoData.currentAmmo = 0;
            CurrentDrop.emitterCharge.Stop();
        }
        
        OnAmmoChanged?.Invoke(_currentAmmoType, ammoData.currentAmmo, ammoData.maxAmmo);
    }
    
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
    
    public void InstantiateDrop()
    {
        if(Time.time - _lastTimeAttacked < attackSpeed) return;
        AmmoData ammoData = ammunition[_currentAmmoType];

        if(ammoData.currentAmmo <= 0) return;
        ammoData.currentAmmo -= 0.1f;
        if (ammoData.currentAmmo < 0.01f) ammoData.currentAmmo = 0;
            
        OnAmmoChanged?.Invoke(_currentAmmoType, ammoData.currentAmmo, ammoData.maxAmmo);
        CurrentDrop = ObjectPool.Instance.InstantiateFromPool(ammoData.bulletPrefab, shootPoint.position, Quaternion.identity).GetComponent<Drop>();

        if(CurrentDrop != null) CurrentDrop.Initialize(shootPoint);
    }

    public void ReloadAmmo(AmmoType ammoType, float quantity)
    {
        AmmoData ammoData = ammunition[ammoType];
        ammoData.currentAmmo = Mathf.Clamp(ammoData.currentAmmo + quantity, 0, ammoData.maxAmmo);
        
        OnAmmoChanged?.Invoke(ammoType, ammoData.currentAmmo, ammoData.maxAmmo);
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

    private void OnScroll()
    {
        if (CurrentDrop == null && Time.timeScale > 0 && Time.time - _lastChange >= changeSpeed)
        {
            _lastChange = Time.time;
            ChangeCurrentAmmoType();

            //Play change type sound
            _emitter.Play();
        }
    }
}


