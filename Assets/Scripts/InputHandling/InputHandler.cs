using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using static Controls;

public class InputHandler : MonoBehaviour, IStdActions
{
    public static InputHandler Singletone;
    public Controls controls;
    public Action OnAbilityActivate;
    public Action OnSecondaryAbilityActivate;
    public Action OnCycleForward;
    public Action OnCycleBackward;

    private void Awake()
    {
        if (Singletone != null)
        {
            Destroy(Singletone);
        }
        else
        {
            Singletone = this;
        }
    }

    private void OnEnable()
    {
        if (controls == null)
        {
            controls = new Controls();
            controls.Std.SetCallbacks(this);
        }

        controls.Std.Enable();
    }

    public void OnFire(InputAction.CallbackContext context)
    {
    }

    public void OnMove(InputAction.CallbackContext context)
    {
    }

    public void OnAcitvateAbility(InputAction.CallbackContext context)
    {
        
    }

    public void OnActivateSecondAbility(InputAction.CallbackContext context)
    {
        
    }

    void IStdActions.OnCycleForward(InputAction.CallbackContext context)
    {
        OnCycleForward?.Invoke();
    }

    void IStdActions.OnCycleBackward(InputAction.CallbackContext context)
    {
        OnCycleBackward?.Invoke();
    }

    public void OnUseAbility(InputAction.CallbackContext context)
    {
        OnAbilityActivate?.Invoke();
    }
}
