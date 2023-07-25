using System;
using ProjectUtils.Helpers;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private float sensitivityX;
    [SerializeField] private float sensitivityY;
    private float _x, _y;
    
    private bool _isMainCamera;
    private Transform _player;
    private float _playerInitialRotation;

    private void Start()
    {
        _player = transform.parent;
        _playerInitialRotation = _player.localRotation.eulerAngles.y;
        _isMainCamera = Helpers.Camera == GetComponent<Camera>();
    }

    private void LateUpdate()
    {
        _x -= Input.GetAxisRaw("Mouse Y") * sensitivityY * Time.deltaTime;
        _y += Input.GetAxisRaw("Mouse X") * sensitivityX * Time.deltaTime;

        _x = Mathf.Clamp(_x, -85f, 90f);
        
        transform.localRotation = Quaternion.Euler(_x, 0f, 0f);
        if(_isMainCamera)_player.localRotation = Quaternion.Euler(0f, _playerInitialRotation+_y, 0f);
    }
    
}
