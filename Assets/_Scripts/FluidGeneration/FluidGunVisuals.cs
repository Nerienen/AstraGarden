using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FluidGunVisuals : MonoBehaviour
{
    [Header("Ammo Display")]
    [SerializeField] private Liquid energyDisplay;
    [SerializeField] private Liquid waterDisplay;
    
    [Header("Mode Display")]
    [SerializeField] private Material waterModeMaterial;
    [SerializeField] private Material energyModeMaterial;
    [SerializeField] private MeshRenderer[] weaponModeDisplays;

    private FluidGun _gun;
    
    private void Awake()
    {
        _gun = GetComponent<FluidGun>();
        
        //Event subscription
        _gun.OnAmmoChanged += AmmoChanged;
        _gun.OnChangeType += DisplayMode;
    }

    #region AmmoDisplay

    private void AmmoChanged(AmmoType ammoType, float currentAmmo, float maxAmmo)
    {
        switch (ammoType)
        {
            case AmmoType.WaterAmmo: 
                ReloadWater(currentAmmo, maxAmmo);
                break;
            case AmmoType.EnergyAmmo:
                ReloadEnergy(currentAmmo, maxAmmo);
                break;
        }
    }

    private void ReloadWater(float currentAmmo, float maxAmmo)
    {
        waterDisplay.fillAmount = 0.61f - 0.21f * currentAmmo / maxAmmo;
        if (waterDisplay.fillAmount >= 0.61f) waterDisplay.fillAmount = 10;
    }    
    
    private void ReloadEnergy(float currentAmmo, float maxAmmo)
    {
        energyDisplay.fillAmount = 0.61f - 0.21f * currentAmmo / maxAmmo;
        if (energyDisplay.fillAmount >= 0.61f) energyDisplay.fillAmount = 10;
    }

    #endregion

    #region ModeDisplay

    private void DisplayMode(AmmoType currentType)
    {
        switch (currentType)
        {
            case AmmoType.WaterAmmo: 
                DisplayWaterMode();
                break;
            case AmmoType.EnergyAmmo:
                DisplayEnergyMode();
                break;
        }
       
    }

    private void DisplayEnergyMode()
    {
        foreach (var display in weaponModeDisplays)
        {
            display.material = energyModeMaterial; 
        }
    }
    
    private void DisplayWaterMode()
    {
        foreach (var display in weaponModeDisplays)
        {
            display.material = waterModeMaterial; 
        }
    }

    #endregion
}
