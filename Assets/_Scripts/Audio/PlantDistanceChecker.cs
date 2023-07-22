using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlantDistanceChecker : MonoBehaviour
{
    [SerializeField] private float radius;
    [SerializeField] private float changingSmoothness;
    
    [SerializeField] private string parameterName;
    [SerializeField] private bool isMusicParameter;

    private Transform[] _points;
    private Transform _player;
    private float _value;
    private float _radius;

    private void Start()
    {
        _player = PlayerController.Instance.transform;

        Plant[] plants = FindObjectsOfType<Plant>();
        _points = new Transform[plants.Length];
        for (int i = 0; i < plants.Length; i++)
        {
            _points[i] = plants[i].transform;
        }
    }

    private void Update()
    {
        Vector3 playerPos = _player.position;
        _radius = radius * radius - _player.localScale.x;

        try
        {
            Vector3 closestPoint = _points.First(pointPos => SqrDist(pointPos.position, playerPos) <= _radius).position;
            float sqrDist = SqrDist(closestPoint, playerPos);
            _value = Mathf.Lerp(_value, sqrDist / _radius, Time.deltaTime * changingSmoothness);
        
            if(isMusicParameter) MusicManager.Instance.SetMusicParameter(parameterName, _value);
            else  MusicManager.Instance.SetMusicParameter(parameterName, _value);
        }
        catch (InvalidOperationException e)
        {
            _value = Mathf.Lerp(_value, 1, Time.deltaTime * changingSmoothness);
            if (_value > 0.99f) _value = 1;

            if(isMusicParameter) MusicManager.Instance.SetMusicParameter(parameterName, _value);
            else  MusicManager.Instance.SetMusicParameter(parameterName, _value);
        }
    }

    private float SqrDist(Vector3 v1, Vector3 v2)
    {
        return (v1.x - v2.x)*(v1.x - v2.x) /*+ (v1.y - v2.y)*(v1.y - v2.y) */+ (v1.z - v2.z)*(v1.z - v2.z);
    }

    #if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            if(_points == null) return;
            foreach (Transform point in _points)
            {
                Gizmos.DrawWireSphere(point.position, radius);
            }
        }
    #endif
}
