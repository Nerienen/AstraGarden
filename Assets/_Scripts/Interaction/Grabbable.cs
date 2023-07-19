using System;
using System.Collections;
using System.Collections.Generic;
using ProjectUtils.Helpers;
using UnityEngine;

public class Grabbable : Interactable
{
    private Rigidbody _rb;
    private Collider _collider;
    private bool _interacting;
    private Transform _grabPoint;

    [SerializeField] private PhysicMaterial bouncyMaterial;
    [SerializeField] private PhysicMaterial grabbedMaterial;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
        _collider = GetComponent<Collider>();
        showOutline = true;
    }

    public override void Interact(Transform grabPoint)
    {
        PlayerInteract.instance.interacting = !PlayerInteract.instance.interacting;
        _rb.constraints = RigidbodyConstraints.None;
        _interacting = !_interacting;
        _grabPoint = grabPoint;

        if (_interacting)
        {
            _rb.constraints = RigidbodyConstraints.FreezeRotation;
            _collider.material = grabbedMaterial;
        }
        else
        {
            _rb.constraints = RigidbodyConstraints.None;
            _collider.material = bouncyMaterial;
        }
        
    }

    private void FixedUpdate()
    {
        if(!_interacting) return;

        Vector3 targetPos = Vector3.Lerp(transform.position, _grabPoint.transform.position, Time.deltaTime * 10f);
        _rb.velocity = (targetPos-transform.position).normalized*Vector3.Distance(transform.position, targetPos) / Time.fixedDeltaTime;
    }
}
