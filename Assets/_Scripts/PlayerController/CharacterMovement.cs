using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMovement : MonoBehaviour
{
    [Header("Movement")] [SerializeField] private float movementSpeed;
    [SerializeField] private float maxSpeed;
    private Vector3 _direction;
    
    //==========BINDINGS==========
    private Rigidbody _rb;
    private Transform _myTransform;

    protected virtual void Awake()
    {
        _myTransform = transform;
        _rb = GetComponent<Rigidbody>();
    }

    public void MovementUpdate(Vector3 input)
    {
        if(input == Vector3.zero) return;

        _direction = _myTransform.forward * input.z + _myTransform.right * input.x;
        _direction.Normalize();

        PerformMovement();
    }

    private void PerformMovement()
    {
        _rb.AddForce(_direction * movementSpeed);
        
        Vector3 velocity = new Vector3(_rb.velocity.x, 0, _rb.velocity.z);
        velocity = Vector3.ClampMagnitude(velocity, maxSpeed);

        _rb.velocity = velocity + Vector3.up * _rb.velocity.y;
    }

}
