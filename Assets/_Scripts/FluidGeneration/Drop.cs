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
    private Rigidbody _rb;
    
    private bool _isHolden;

    public void Initialize(Transform followPoint)
    {
        _rb = GetComponent<Rigidbody>();
        _collider = transform.GetChild(0).GetComponent<Collider>();

        _rb.velocity = Vector3.zero;
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
            _rb.velocity = Vector3.ClampMagnitude(_rb.velocity, maxDropSpeed);
        }
    }

    public bool Grow(float scalingFactor)
    {
        if (transform.localScale.x < maxDropScale)
        {
            transform.localScale += (scalingFactor-0.05f) * Time.deltaTime * Vector3.one;
            return true;
        }

        return false;
    }

    public void Throw(Vector3 direction, float force)
    {
        _isHolden = false;

        _collider.enabled = true;
        _rb.mass = Mathf.Clamp(transform.localScale.x, 0.5f, 1.5f);
        _rb.AddForce(direction*force, ForceMode.Impulse);
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
        _rb.mass = Mathf.Clamp(transform.localScale.x, 0.5f, 3f);
    }
}
