using ProjectUtils.Helpers;
using UnityEngine;

public class PlayerController : CharacterMovement
{
    [Header("Bindings")]
    [SerializeField] private FluidGun fluidGun;
    private Vector3 _input;
    private PlayerInteract _playerInteract;
    
    public static PlayerController Instance { get; private set; }
    
    protected override void Awake()
    {
        base.Awake();

        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        
        _playerInteract = GetComponent<PlayerInteract>();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Update()
    {
        if(Time.timeScale <= 0) return;
        
        _input = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));

        if (Input.GetMouseButton(0))
        {
            fluidGun.ChargeDrop();
        }
        if (Input.GetMouseButtonUp(0) && fluidGun.currentDrop != null)
        {
            fluidGun.currentDrop.emitter.Stop();
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
