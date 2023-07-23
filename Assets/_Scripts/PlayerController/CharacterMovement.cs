using System;
using System.Collections;
using System.Collections.Generic;
using FMODUnity;
using ProjectUtils.Helpers;
using UnityEngine;

public class CharacterMovement : MonoBehaviour
{

    [Header("Movement")] [SerializeField] private float movementSpeed;
    [SerializeField] private float maxSpeed;
    private Vector3 _direction;

    [SerializeField] private float walkSoundTime;
    private float _lastWalkSound;
    
    [SerializeField] private float walkDisplayTime;
    private float _lastWalkDisplay;
    [SerializeField] private float jiggleMagnitude;
    [SerializeField] private AnimationCurve jiggleX;
    [SerializeField] private AnimationCurve jiggleY;
    [SerializeField] private AnimationCurve jiggleZ;
    
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
        if (Time.time - _lastWalkSound >= walkSoundTime)
        {
            _lastWalkSound = Time.time;
            AudioManager.instance.PlayOneShot(FMODEvents.instance.footSteps, transform.position);
        }
        if (Time.time - _lastWalkDisplay >= walkDisplayTime)
        {
            _lastWalkDisplay = Time.time;
            Helpers.Camera.transform.DoJiggle(jiggleMagnitude,jiggleX, jiggleY, jiggleZ, walkDisplayTime-0.1f);
        }

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
