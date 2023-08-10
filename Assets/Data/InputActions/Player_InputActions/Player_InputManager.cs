using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player_InputManager : MonoBehaviour
{
    public event Action OnStartCharging;
    public event Action OnStopCharging;
    public event Action OnFire;
    public event Action OnInteract;
    public event Action OnCancel;
    public event Action OnScroll;
    public event Action OnSwap;

    public Vector2 MoveInput { get; private set; }
    public Vector2 MouseInput { get; private set; }
    public bool InspectInput { get; private set; }

    Player_InputActions _inputActions;

    private void Awake()
    {
        _inputActions = new Player_InputActions();
    }

    private void OnEnable()
    {
        _inputActions.Enable();

        _inputActions.FirstPersonCharacter_ActionMap.Move.performed += Move;
        _inputActions.FirstPersonCharacter_ActionMap.Move.canceled += Move;

        _inputActions.FirstPersonCharacter_ActionMap.Look.performed += Look;
        _inputActions.FirstPersonCharacter_ActionMap.Look.canceled += Look;

        _inputActions.FirstPersonCharacter_ActionMap.Charge.performed += StartCharging;
        _inputActions.FirstPersonCharacter_ActionMap.Charge.canceled += StopCharging;

        _inputActions.FirstPersonCharacter_ActionMap.Fire.performed += Fire;

        _inputActions.FirstPersonCharacter_ActionMap.Interact.performed += Interact;

        _inputActions.FirstPersonCharacter_ActionMap.Inspect.performed += Inspect;

        _inputActions.FirstPersonCharacter_ActionMap.Scroll.performed += Scroll;
        _inputActions.FirstPersonCharacter_ActionMap.Swap.performed += Swap;
    }

    private void OnDisable()
    {
        _inputActions.FirstPersonCharacter_ActionMap.Move.performed -= Move;
        _inputActions.FirstPersonCharacter_ActionMap.Move.canceled -= Move;

        _inputActions.FirstPersonCharacter_ActionMap.Look.performed += Look;
        _inputActions.FirstPersonCharacter_ActionMap.Look.canceled += Look;

        _inputActions.FirstPersonCharacter_ActionMap.Charge.performed -= StartCharging;
        _inputActions.FirstPersonCharacter_ActionMap.Charge.canceled -= StopCharging;

        _inputActions.FirstPersonCharacter_ActionMap.Fire.performed -= Fire;

        _inputActions.FirstPersonCharacter_ActionMap.Interact.performed -= Interact;

        _inputActions.FirstPersonCharacter_ActionMap.Interact.performed -= Interact;

        _inputActions.FirstPersonCharacter_ActionMap.Scroll.performed -= Scroll;
        _inputActions.FirstPersonCharacter_ActionMap.Swap.performed -= Swap;

        _inputActions.Disable();
    }

    private void Move(InputAction.CallbackContext ctx)
    {
        MoveInput = ctx.ReadValue<Vector2>();
    }

    private void Look(InputAction.CallbackContext ctx)
    {
        MouseInput = ctx.ReadValue<Vector2>();
    }

    private void StartCharging(InputAction.CallbackContext ctx)
    {
        OnStartCharging?.Invoke();
    }

    private void StopCharging(InputAction.CallbackContext ctx)
    {
        OnStopCharging?.Invoke();
    }

    private void Fire(InputAction.CallbackContext ctx)
    {
        OnFire?.Invoke();
    }

    private void Interact(InputAction.CallbackContext ctx)
    {
        OnInteract?.Invoke();
    }

    private void Inspect(InputAction.CallbackContext ctx)
    {
        InspectInput = ctx.ReadValue<float>() > 0.1f;
    }

    private void Scroll(InputAction.CallbackContext ctx)
    {
        OnScroll?.Invoke();
    }

    private void Swap(InputAction.CallbackContext ctx)
    {
        OnSwap?.Invoke();
    }
}