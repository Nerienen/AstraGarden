using System;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private float sensitivityX;
    [SerializeField] private float sensitivityY;
    private float _x, _y;
    
    private Transform _player;

    private void Start()
    {
        _player = transform.parent;
    }

    private void LateUpdate()
    {
        _x -= Input.GetAxisRaw("Mouse Y") * sensitivityY * Time.deltaTime;
        _y += Input.GetAxisRaw("Mouse X") * sensitivityX * Time.deltaTime;

        _x = Mathf.Clamp(_x, -85f, 90f);
        
        transform.localRotation = Quaternion.Euler(_x, 0f, 0f);
        _player.localRotation = Quaternion.Euler(0f, _y, 0f);
    }
    
}