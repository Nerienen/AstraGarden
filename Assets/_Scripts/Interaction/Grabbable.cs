using System;
using System.Collections;
using System.Collections.Generic;
using ProjectUtils.Helpers;
using UnityEngine;

public class Grabbable : Interactable
{
    private Rigidbody _rb;
    private Collider _collider;
    protected bool _grabbing;
    private Transform _grabPoint;

    public bool grabbable { get; protected set; } = true;

    [Header("Grabbable Parameters")]
    [SerializeField] private PhysicMaterial bouncyMaterial;
    [SerializeField] private PhysicMaterial grabbedMaterial;

    protected virtual void Awake()
    {
        _rb = GetComponent<Rigidbody>();
        _collider = GetComponent<Collider>();
        if (_collider == null) _collider = GetComponentInChildren<Collider>(); 
        outlineColor = Color.white;
    }

    public override bool Interact()
    {
        return false;
    }

    public override bool Interact(Transform grabPoint)
    {
        if(!grabbable) return false;
        
        PlayerInteract.instance.interacting = !PlayerInteract.instance.interacting;
        _rb.constraints = RigidbodyConstraints.None;
        _grabbing = !_grabbing;
        _grabPoint = grabPoint;

        if (_grabbing)
        {
            _rb.constraints = RigidbodyConstraints.FreezeRotation;
            _collider.material = grabbedMaterial;
        }
        else
        {
            _rb.constraints = RigidbodyConstraints.None;
            _collider.material = bouncyMaterial;
        }

        return true;
    }

    private void FixedUpdate()
    {
        if(!_grabbing) return;

        Vector3 targetPos = Vector3.Lerp(transform.position, _grabPoint.transform.position, Time.deltaTime * 10f);
        _rb.velocity = (targetPos-transform.position).normalized*Vector3.Distance(transform.position, targetPos) / Time.fixedDeltaTime;
    }
}
