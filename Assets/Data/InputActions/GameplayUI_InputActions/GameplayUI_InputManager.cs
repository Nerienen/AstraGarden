using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameplayUI_InputManager : MonoBehaviour
{
    public event Action OnCancel;

    GameplayUI_InputActions _inputActions;

    private void Awake()
    {
        _inputActions = new GameplayUI_InputActions();
    }

    private void OnEnable()
    {
        _inputActions.Enable();

        _inputActions.GameplayUI_ActionMap.Cancel.performed += Cancel;
    }

    private void OnDisable()
    {
        _inputActions.GameplayUI_ActionMap.Cancel.performed -= Cancel;

        _inputActions.Disable();
    }

    private void Cancel(InputAction.CallbackContext ctx)
    {
        OnCancel?.Invoke();
    }
}
