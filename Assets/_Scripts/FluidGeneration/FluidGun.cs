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
    
    [Header("Display")]
    [SerializeField] private Liquid energyDisplay;
    [SerializeField] private Liquid waterDisplay;
    [SerializeField] private Material waterModeMaterial;
    [SerializeField] private Material energyModeMaterial;
    [SerializeField] private MeshRenderer[] weaponModeDisplays;

    [Header("Gun parameters")] 
    [SerializeField] private float shootingPower;
    [SerializeField] private float waterAmmo;
    [SerializeField] private float energyAmmo;
    [SerializeField] private float maxWaterAmmo;
    [SerializeField] private float maxEnergyAmmo;

    [SerializeField] private float changeSpeed;
    private float _lastChange;
    [field:SerializeReference] public float attackSpeed { get; private set; }
    [SerializeField] private float cadence;
    private float _lastTimeAttacked;
    private FMODUnity.StudioEventEmitter _emitter;

    [Header("Physics")] 
    [SerializeField] private LayerMask ignoredLayers;
    
    public Drop currentDrop { get; private set;}
    private bool _throwsEnergy;

    private void Start()
    {
        _emitter = GetComponent<StudioEventEmitter>();
        _lastChange = float.MinValue;
        _lastTimeAttacked = float.MinValue;
        waterDisplay.fillAmount = 0.61f - 0.21f * waterAmmo / maxWaterAmmo;
        energyDisplay.fillAmount = 0.61f - 0.21f * energyAmmo / maxEnergyAmmo;

        if (energyDisplay.fillAmount >= 0.61f) energyDisplay.fillAmount = 10;
        if (waterDisplay.fillAmount >= 0.61f) waterDisplay.fillAmount = 10;
        DisplayMode();
    }

    private void Update()
    {
        if (Input.mouseScrollDelta.y != 0 && currentDrop == null && Time.timeScale > 0 && Time.time - _lastChange >= changeSpeed)
        {
            _lastChange = Time.time;
            _emitter.Play();
            _throwsEnergy = !_throwsEnergy;
            DisplayMode();
        }
    }

    public void ChargeDrop()
    {
        if(currentDrop == null)return;
        if (!_throwsEnergy)
        {
            if(waterAmmo <= 0) return;

            waterAmmo -= currentDrop.Grow(cadence);
            if (waterAmmo < 0.01f)
            {
                waterAmmo = 0;
                currentDrop.emitterCharge.Stop();
            }
            
            waterDisplay.fillAmount = 0.6f - 0.2f * waterAmmo / maxWaterAmmo;
            if (waterDisplay.fillAmount >= 0.6f) waterDisplay.fillAmount = 10;
        }
        else
        {
            if(energyAmmo <= 0) return;

            energyAmmo -= currentDrop.Grow(cadence);
            //Sonido
            if (energyAmmo < 0.01f)    {
                energyAmmo = 0;
                currentDrop.emitterCharge.Stop();
            }
            energyDisplay.fillAmount = 0.6f - 0.2f * energyAmmo / maxEnergyAmmo;
            if (energyDisplay.fillAmount >= 0.6f) energyDisplay.fillAmount = 10;
        }
    }
    
    public void ShootDrop()
    {
        if(Physics.Raycast(Helpers.Camera.transform.position, Helpers.Camera.transform.forward, out var hit,Mathf.Infinity, ~ignoredLayers))
        {
            _lastTimeAttacked = Time.time;
            Vector3 dir = hit.point - shootPoint.position;
            dir.Normalize();
            
            Debug.DrawLine(shootPoint.position, hit.point, Color.red, 10);
            Debug.DrawLine(Helpers.Camera.transform.position, hit.point, Color.green, 10);
            currentDrop.Throw(dir, shootingPower);
            if(_throwsEnergy) AudioManager.instance.PlayOneShot(FMODEvents.instance.shootEnergy, transform.position);
            else AudioManager.instance.PlayOneShot(FMODEvents.instance.shootWater, transform.position);
            currentDrop = null;
        }
    }

    public void InstantiateDrop()
    {
        if(Time.time - _lastTimeAttacked < attackSpeed) return;

        if (_throwsEnergy)
        { 
            if(energyAmmo <= 0) return;
            energyAmmo -= 0.1f;
            if (energyAmmo < 0.01f) energyAmmo = 0;
            
            energyDisplay.fillAmount = 0.6f - 0.2f * energyAmmo / maxEnergyAmmo;
            if (energyDisplay.fillAmount >= 0.6f) energyDisplay.fillAmount = 10;
            currentDrop = ObjectPool.Instance.InstantiateFromPool(energyDropPrefab, shootPoint.position, Quaternion.identity).GetComponent<Drop>();
        }
        else
        {
            if(waterAmmo <= 0) return;
            waterAmmo -= 0.1f;
            if (waterAmmo < 0.01f) waterAmmo = 0;
            
            waterDisplay.fillAmount = 0.6f - 0.2f * waterAmmo / maxWaterAmmo;
            if (waterDisplay.fillAmount >= 0.6f) waterDisplay.fillAmount = 10;
            
            currentDrop = ObjectPool.Instance.InstantiateFromPool(waterDropPrefab, shootPoint.position, Quaternion.identity).GetComponent<Drop>();
        }
        
        currentDrop.Initialize(shootPoint);
    }

    public void ReloadAmmo(AmmoType ammoType, float quantity)
    {
        if (ammoType == AmmoType.WaterAmmo)
        {
            waterAmmo = Mathf.Clamp(waterAmmo + quantity, 0, maxWaterAmmo);
            waterDisplay.fillAmount = 0.6f - 0.2f * waterAmmo / maxWaterAmmo;
        }
        else
        {
            energyAmmo = Mathf.Clamp(energyAmmo + quantity, 0, maxEnergyAmmo);
            energyDisplay.fillAmount = 0.6f - 0.2f * energyAmmo / maxEnergyAmmo;
        }
    }

    private void DisplayMode()
    {
        foreach (var display in weaponModeDisplays)
        {
            display.material = _throwsEnergy ? energyModeMaterial : waterModeMaterial;
        }
    }
}

public enum AmmoType{
    WaterAmmo, EnergyAmmo
}
