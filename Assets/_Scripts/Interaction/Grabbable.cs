using System;
using System.Collections;
using System.Collections.Generic;
using ProjectUtils.Helpers;
using UnityEngine;

public class Grabbable : Interactable
{
    private Rigidbody _rb;
    private bool _interacting;
    private Transform _grabPoint;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
        showOutline = true;
    }

    public override void Interact(Transform grabPoint)
    {
        _interacting = !_interacting;
        _grabPoint = grabPoint;
    }

    private void FixedUpdate()
    {
        if(!_interacting) return;

        Vector3 targetPos = Vector3.Lerp(transform.position, _grabPoint.transform.position, Time.deltaTime * 15f);
        _rb.velocity = (targetPos-transform.position).normalized*Vector3.Distance(transform.position, targetPos) / Time.fixedDeltaTime;
    }
}
