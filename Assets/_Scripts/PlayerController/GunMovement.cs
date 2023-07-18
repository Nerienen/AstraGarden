using System;
using System.Collections;
using System.Collections.Generic;
using ProjectUtils.Helpers;
using UnityEngine;

public class GunMovement : MonoBehaviour
{
    [SerializeField] private float swaySoftness;
    
    private void LateUpdate()
    {
        transform.rotation = Quaternion.Slerp(transform.rotation, Helpers.Camera.transform.rotation, Time.deltaTime*swaySoftness);
    }
}
