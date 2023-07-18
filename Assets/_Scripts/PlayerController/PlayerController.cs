using ProjectUtils.Helpers;
using UnityEngine;

public class PlayerController : CharacterMovement
{
    [Header("Bindings")]
    [SerializeField] private FluidGun fluidGun;
    private Vector3 _input;
    
    protected override void Awake()
    {
        base.Awake();
        
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Update()
    {
        _input = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));

        if (Input.GetMouseButton(0))
        {
            fluidGun.ChargeDrop();
        }

        if (Input.GetMouseButton(1) && fluidGun.currentDrop != null)
        {
           fluidGun.ShootDrop();
        }
    }
    
    private void FixedUpdate()
    {
        MovementUpdate(_input);
    }
}
