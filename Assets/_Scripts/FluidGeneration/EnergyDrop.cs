using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnergyDrop : Drop
{
    [Header("Materials")] 
    [SerializeField] private Material regularMaterial;
    [SerializeField] private Material suctionMaterial;

    private Vector3 _target;
    private SphereCollider _triggerCollider;
    private Renderer _renderer;

    public override void Initialize(Transform followPoint)
    {
        base.Initialize(followPoint);
        _renderer = transform.GetChild(0).GetComponent<Renderer>();
        _triggerCollider = GetComponent<SphereCollider>();

        _renderer.material = regularMaterial;
    }

    public override void Throw(Vector3 direction, float force)
    {
        base.Throw(direction, force);

        _renderer.material = suctionMaterial;
        _triggerCollider.radius = 2.75f/transform.localScale.x;
        
        _target = Vector3.zero;
        _renderer.sharedMaterial.SetVector("_SuctionPosition", new Vector4(_target.x, _target.y, _target.z));
    }

    protected override void OnCollisionEnter(Collision collision)
    {
       base.OnCollisionEnter(collision);
       AudioManager.instance.PlayOneShot(FMODEvents.instance.energyReboundOnWall, transform.position);

       EnergyDrop energyDrop = collision.gameObject.GetComponent<EnergyDrop>();
       if (energyDrop != null)
       {
           if (transform.localScale.x > collision.transform.localScale.x)
           {
               AddScale(collision.transform.localScale);
               energyDrop.gameObject.SetActive(false);
               _triggerCollider.radius = 2.75f/transform.localScale.x;
           }
           else
           {
               energyDrop.AddScale(transform.localScale);
               gameObject.SetActive(false);
               _triggerCollider.radius = 2.75f/transform.localScale.x;
           }
           return;
       }

       if (collision.collider.CompareTag("EnergyPoint"))
       {
           EnergyReceiver energyReceiver = collision.collider.GetComponent<EnergyReceiver>();
           if(energyReceiver != null) energyReceiver.AddEnergy(transform.localScale.x);
           gameObject.SetActive(false);
           return;
       }
       
       ReduceScale();
       _triggerCollider.radius = 2.75f/transform.localScale.x;
    }
    
    protected override void OnCollisionStay(Collision collision)
    {
        base.OnCollisionStay(collision);

        EnergyDrop energyDrop = collision.gameObject.GetComponent<EnergyDrop>();
        if (energyDrop != null)
        {
            if (transform.localScale.x > collision.transform.localScale.x)
            {
                AddScale(collision.transform.localScale);
                energyDrop.gameObject.SetActive(false);
            }
            else
            {
                energyDrop.AddScale(transform.localScale);
                gameObject.SetActive(false);
            }
            return;
        }
        
        if (collision.collider.CompareTag("EnergyPoint"))
        {
            EnergyReceiver energyReceiver = collision.collider.GetComponent<EnergyReceiver>();
            if(energyReceiver != null) energyReceiver.AddEnergy(transform.localScale.x);
            gameObject.SetActive(false);
            return;
        }
        
        ReduceScale();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (_target == Vector3.zero && other.CompareTag("EnergyPoint"))
        {
            _target = other.transform.position;
            _renderer.sharedMaterial.SetVector("_SuctionPosition", new Vector4(_target.x, _target.y, _target.z));
            rb.velocity = (_target - transform.position).normalized * Mathf.Clamp(rb.velocity.magnitude, 5, 20);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (_target == Vector3.zero && other.CompareTag("EnergyPoint"))
        {
            _target = other.transform.position;
            _renderer.sharedMaterial.SetVector("_SuctionPosition", new Vector4(_target.x, _target.y, _target.z));
            rb.velocity = (_target - transform.position).normalized * Mathf.Clamp(rb.velocity.magnitude, 5, 20);
        }
    }
    
}
