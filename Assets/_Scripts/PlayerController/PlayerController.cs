using ProjectUtils.Helpers;
using System;
using FMODUnity;
using UnityEngine;

public class PlayerController : CharacterMovement
{
    public event Action OnHasDied;

    [Header("Bindings")]
    [SerializeField] private FluidGun fluidGun;
    private Vector3 _input;
    private PlayerInteract _playerInteract;

    private bool _isDead;
    
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

    private void Start()
    {
        if (OxygenController.Instance != null)
        {
            OxygenController.Instance.OnOxygenFinished += OnDead;
        }
    }

    private void OnDestroy()
    {
        if (OxygenController.Instance != null)
        {
            OxygenController.Instance.OnOxygenFinished -= OnDead;
        }
    }

    private void Update()
    {
        if(Time.timeScale <= 0) return;
        if (_isDead) return;
        
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
        if (_isDead) return;
        MovementUpdate(_input);
    }

    void OnDead()
    {
        _isDead = true;
        OnHasDied?.Invoke();
    }
}
