using System;
using System.Collections;
using System.Collections.Generic;
using ProjectUtils.Helpers;
using UnityEngine;

public class PlantInspector : MonoBehaviour, IInspectable
{
    [SerializeField] private PlantStatsUI _plantStatsUI;
    private Transform _pivot;
    private Plant _plant;
    private bool _isInspectable;

    private void Start()
    {
        _plant = GetComponent<Plant>();
        _pivot = _plantStatsUI.transform.parent;
        _plantStatsUI.transform.position = _pivot.position+_pivot.forward*0.5f;
        _plantStatsUI.transform.forward = _pivot.forward;
    }

    public bool IsInspectable
    {
        get => _isInspectable;
        set => _isInspectable = value;
    }

    public async void Inspect(Vector3 inspectorPivotForward)
    {
        if(!_isInspectable) return;
        
        _pivot.forward =  Vector3.Slerp(_pivot.forward, inspectorPivotForward, Time.deltaTime*10f);
        
        _plantStatsUI.SetData(_plant.PlantData);
        if (!_plantStatsUI.gameObject.activeInHierarchy)
        {
            await _plantStatsUI.ShowStatsAsync();
        }
    }

    public void StopInspecting()
    {
        if (_plantStatsUI.gameObject.activeInHierarchy)
        {
            _plantStatsUI.DisableAsync();
        }
    }
}
