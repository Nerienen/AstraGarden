using System;
using System.Collections;
using System.Collections.Generic;
using ProjectUtils.Helpers;
using UnityEngine;

public class Grabbable :  Interactable
{
    private Rigidbody _rb;

    private Transform _grabPoint;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
    }

    public override void Interact(Transform grabPoint)
    {
        base.Interact(grabPoint);
        
        _grabPoint = grabPoint;
        _rb.useGravity = !interacting;
    }

    private void FixedUpdate()
    {
        if(!interacting) return;

        Vector3 targetPos = Vector3.Lerp(transform.position, _grabPoint.transform.position, Time.deltaTime * 15f);
        _rb.velocity = (targetPos-transform.position).normalized*Vector3.Distance(transform.position, targetPos) / Time.fixedDeltaTime;
    }
}
