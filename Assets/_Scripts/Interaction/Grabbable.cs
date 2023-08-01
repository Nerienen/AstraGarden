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
    public bool hasBeenHolded;

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
            hasBeenHolded = true;
            _rb.constraints = RigidbodyConstraints.FreezeRotation;
            _rb.velocity = Vector3.zero;
            _rb.drag = 0;
            _collider.material = grabbedMaterial;
            onGrabObject?.Invoke();
            SetCollisions(true);
            holden = false;
        }
        else
        {
            SetCollisions(false);
            _rb.drag = 0.3f;
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

    public void MoveTransform(Vector3 targetPos)
    {
        targetPos = Vector3.Lerp(transform.position, targetPos, Time.deltaTime*25f);
        transform.position = targetPos;
    }
    
    public void Move(Vector3 targetPos)
    {
        targetPos = Vector3.Lerp(transform.position, targetPos, Time.deltaTime * 10f);
        
        float dist = Vector3.Distance(transform.position, targetPos);
        Vector3 velocity = (targetPos-transform.position).normalized *  dist / Time.deltaTime;
       
        if(velocity != new Vector3(Single.NaN, Single.NaN, Single.NaN))
            _rb.velocity = velocity;
    }
    

    public void SetHolden(bool value)
    {
        holden = value;
    }
}
