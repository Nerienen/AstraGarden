using System;
using System.Collections;
using System.Collections.Generic;
using ProjectUtils.ObjectPooling;
using Unity.Mathematics;
using UnityEngine;

public abstract class Drop : MonoBehaviour
{
    [Header("Bindings")] 
    [SerializeField] private GameObject splashEffect;
    
    [Header("Drop Settings")]
    [SerializeField] private float maxDropScale;
    [SerializeField] private float maxDropSpeed;
    
    private Transform _followPoint;
    private Collider _collider;
    protected Rigidbody rb;
    
    private bool _isHolden;

    public virtual void Initialize(Transform followPoint)
    {
        rb = GetComponent<Rigidbody>();
        _collider = transform.GetChild(0).GetComponent<Collider>();

        rb.velocity = Vector3.zero;
        _collider.enabled = false;
        _isHolden = true;
        _followPoint = followPoint;

        transform.localScale = 0.1f*Vector3.one;
    }

    private void LateUpdate()
    {
        if (_isHolden)
        {
            transform.position = _followPoint.position;
            transform.forward = _followPoint.forward;
        }
        else
        {
            rb.velocity = Vector3.ClampMagnitude(rb.velocity, maxDropSpeed);
        }
    }

    public float Grow(float scalingFactor)
    {
        if (transform.localScale.x < maxDropScale)
        {
            float scaleDelta = Mathf.Clamp(transform.localScale.x + (scalingFactor) * Time.deltaTime, 0, maxDropScale) - transform.localScale.x;
            transform.localScale += scaleDelta * Vector3.one;
            return scaleDelta;
        }

        return 0;
    }

    public virtual void Throw(Vector3 direction, float force)
    {
        _isHolden = false;

        _collider.enabled = true;
        rb.mass = Mathf.Clamp(transform.localScale.x, 0.5f, 1.5f);
        rb.AddForce(direction*force, ForceMode.Impulse);
    }

    protected virtual void OnCollisionEnter(Collision collision)
    {
        DisplaySplash(collision.contacts[0].normal, collision.contacts[0].point);
    }
    
    protected virtual void OnCollisionStay(Collision collision)
    {
        DisplaySplash(collision.contacts[0].normal, collision.contacts[0].point);
    }

    protected void DisableDrop(Vector3 splashForward, Vector3 splashPosition)
    {
        DisplaySplash(splashForward, splashPosition);   
        gameObject.SetActive(false);
    }
    
    private void DisplaySplash(Vector3 splashForward, Vector3 splashPosition)
    {
        GameObject splash = ObjectPool.Instance.InstantiateFromPool(splashEffect, splashPosition, quaternion.identity, true);
        splash.transform.localScale = transform.localScale / 2f;
        splash.transform.forward = splashForward;
    }

    protected void ReduceScale()
    {
        transform.localScale *= 0.65f;
        if(transform.localScale.x < 0.15f) gameObject.SetActive(false);
    }

    protected void AddScale(Vector3 addition)
    {
        transform.localScale = Vector3.ClampMagnitude(transform.localScale+addition, 3.5f);
        rb.mass = Mathf.Clamp(transform.localScale.x, 0.5f, 3f);
    }
}
