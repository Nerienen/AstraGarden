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
    [SerializeField] private float ammo;
    [SerializeField] private float maxAmmo;
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

    public void ChargeDrop()
    {
        if(ammo <= 0) return;
        
        if (currentDrop == null)
        {
            InstantiateDrop();
            return;
        }

        if (currentDrop.Grow(cadence)) ammo -= cadence * Time.deltaTime;
        if (ammo < 0) ammo = 0;
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
            
        if (_throwsEnergy) currentDrop = ObjectPool.Instance.InstantiateFromPool(energyDropPrefab, shootPoint.position, Quaternion.identity).GetComponent<Drop>();
        currentDrop = ObjectPool.Instance.InstantiateFromPool(waterDropPrefab, shootPoint.position, Quaternion.identity).GetComponent<Drop>();
        
        currentDrop.Initialize(shootPoint);
    }
}
