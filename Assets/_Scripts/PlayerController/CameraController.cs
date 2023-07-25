using System;
using ProjectUtils.Helpers;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public float sensitivityX;
    public float sensitivityY;
    private float _x, _y;
    
    private bool _isMainCamera;
    private Transform _player;
    private float _playerInitialRotation;

    private void Start()
    {
        _player = transform.parent;
        _playerInitialRotation = _player.localRotation.eulerAngles.y;
        _isMainCamera = Helpers.Camera == GetComponent<Camera>();

        sensitivityX = PlayerPrefs.GetFloat("SensX", 100);
        sensitivityY = PlayerPrefs.GetFloat("SensY", 100);
        Helpers.Camera.fieldOfView =  PlayerPrefs.GetFloat("Fov", 70);
    }

    private void Update()
    {
        if (!_isMainCamera)
        {
            transform.localRotation = Helpers.Camera.transform.localRotation;
            return;
        }
        
        _x -= Input.GetAxisRaw("Mouse Y") * sensitivityY * Time.deltaTime;
        _y += Input.GetAxisRaw("Mouse X") * sensitivityX * Time.deltaTime;

        _x = Mathf.Clamp(_x, -85f, 90f);
        
        transform.localRotation = Quaternion.Euler(_x, 0f, 0f);
        _player.localRotation = Quaternion.Euler(0f, _playerInitialRotation+_y, 0f);
    }

    public void SetSensX(float value)
    {
        sensitivityX = value;
        PlayerPrefs.SetFloat("SensX", value);
    }

    public void SetSensY(float value)
    {
        sensitivityY = value;
        PlayerPrefs.SetFloat("SensY", value);
    }

    public void SetFov(float value)
    {
        Helpers.Camera.fieldOfView = value;
        PlayerPrefs.SetFloat("Fov", value);
    }
    
}
