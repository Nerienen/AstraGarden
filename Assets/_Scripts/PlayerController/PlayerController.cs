using System;
using FMODUnity;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : CharacterMovement
{
    public event Action OnHasDied;

    [Header("Bindings")]
    [SerializeField] private FluidGun fluidGun;
    [SerializeField] private Player_InputManager inputManager;

    private Vector3 _input;
    private PlayerInteract _playerInteract;

    private bool _isDead;

    private StudioEventEmitter _emitter;

    bool _isCharging;
    bool _isFiring;
    
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

        _emitter = GetComponent<StudioEventEmitter>();
       // _emitter.Play();
    }

    private void OnEnable()
    {
        inputManager.OnStartCharging += OnStartCharging;
        inputManager.OnStopCharging += OnStopCharging;
        inputManager.OnFire += OnFire;
    }

    private void OnDisable()
    {
        inputManager.OnStartCharging -= OnStartCharging;
        inputManager.OnStopCharging -= OnStopCharging;
        inputManager.OnFire -= OnFire;
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
        if (Time.timeScale <= 0)
        {
            //_emitter.EventInstance.setPaused(true);
            return;
        }
       // else _emitter.EventInstance.setPaused(false);
        if (_isDead) return;
        
        //_emitter.EventInstance.setParameterByName("Oxygen", OxygenController.Instance.CurrentAmount/OxygenController.Instance.MaxAmount);
        _input = new Vector3(inputManager.MoveInput.x, 0, inputManager.MoveInput.y);
        
        if (_isCharging)
        {
            if(fluidGun.CurrentDrop == null)fluidGun.InstantiateDrop();
            fluidGun.ChargeDrop();
        }
       
        if (_isFiring)
        {
            _isFiring = false;
            fluidGun.ShootDrop();
        }
    }
    
    private void FixedUpdate()
    {
        if (_isDead ) return;
        MovementUpdate(_input);
    }

    void OnDead()
    {
        _isDead = true;
        OnHasDied?.Invoke();
    }

    #region Input system implementation
    void OnStartCharging()
    {
        if (Time.timeScale <= 0 || _isDead) return;
        _isCharging = true;
    }

    void OnStopCharging()
    {
        if (Time.timeScale <= 0 || _isDead) return;

        _isCharging = false;

        if (fluidGun.CurrentDrop)
        {
            fluidGun.CurrentDrop.emitterCharge.Stop();
        }
    }

    void OnFire()
    {
        if (Time.timeScale <= 0 || _isDead || fluidGun.CurrentDrop == null) return;

        _isFiring = true;
    }
    #endregion
}
