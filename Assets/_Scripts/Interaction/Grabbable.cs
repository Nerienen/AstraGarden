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
    
    private bool _isPlant;

    public bool grabbable { get;  set; } = true;
    protected bool holden;

    public event Action onGrabObject;

    [Header("Grabbable Parameters")]
    [SerializeField] private PhysicMaterial bouncyMaterial;
    [SerializeField] private PhysicMaterial grabbedMaterial;

    protected override void Awake()
    {
        base.Awake();
        
        _rb = GetComponent<Rigidbody>();
        _collider = GetComponent<Collider>();
        if (_collider == null) _collider = GetComponentInChildren<Collider>(); 
        outline.OutlineColor = Color.white;
        if (GetComponent<Plant>() != null) _isPlant = true;
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
            onGrabObject?.Invoke();
            SetCollisions(true);
            holden = false;
        }
        else
        {
            SetCollisions(false);
            _rb.constraints = RigidbodyConstraints.None;
            _collider.material = bouncyMaterial;
        }

        return true;
    }

    private void SetCollisions(bool ignore)
    {
        if (_isPlant)
        {
            Physics.IgnoreLayerCollision(11, 10, ignore);
            Physics.IgnoreLayerCollision(11, 6, ignore);
            Physics.IgnoreLayerCollision(11, 14, ignore);
        }
        else
        {
            Physics.IgnoreLayerCollision(12, 6, ignore);
        }
    }

    private void FixedUpdate()
    {
        if(!_grabbing) return;
        
        Vector3 targetPos = Vector3.Lerp(transform.position, _grabPoint.transform.position, Time.deltaTime * 10f);
        _rb.velocity = (targetPos-transform.position).normalized*Vector3.Distance(transform.position, targetPos) / Time.fixedDeltaTime;
    }

    public void SetHolden(bool value)
    {
        holden = value;
    }
}
