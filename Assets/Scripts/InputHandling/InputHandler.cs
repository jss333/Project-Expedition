using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using static Controls;

public class InputHandler : MonoBehaviour, IStdActions , IUIActions
{
    public static InputHandler Singleton;
    public Controls controls;
    public Action OnAbilityActivate;
    public Action OnSecondaryAbilityActivate;
    public Action OnCycleForward;
    public Action OnCycleBackward;
    public Action OnJumpDown;
    public Action OnJumpUp;
    public Action OnThrowBomb;

    public Action OnHandlePausingAndResuming;
    public Action OnHandleDebugMenuOpenning;

    public Action OnUIMenuActivated;
    public Action OnUIMenuDeActivated;

    public Action<bool> OnWeaponFire;

    public Action<float> OnPlayerMovementHandle;

    private void Awake()
    {
        if (Singleton != null)
        {
            Destroy(Singleton);
        }
        else
        {
            Singleton = this;
        }

        OnUIMenuActivated += DisablePlayerGameplayInput;
        OnUIMenuDeActivated += EnablePlayerGameplayInput;
    }

    private void OnEnable()
    {
        if (controls == null)
        {
            controls = new Controls();
            controls.Std.SetCallbacks(this);
            controls.UI.SetCallbacks(this);
        }

        controls.Std.Enable();
        controls.UI.Enable();
    }

    private void OnDestroy()
    {
        OnUIMenuActivated -= DisablePlayerGameplayInput;
        OnUIMenuDeActivated -= EnablePlayerGameplayInput;
    }

    private void DisablePlayerGameplayInput()
    {
        controls.Std.Disable();
    }

    private void EnablePlayerGameplayInput()
    {
        controls.Std.Enable();
    }

    public void OnFire(InputAction.CallbackContext context)
    {
        OnWeaponFire?.Invoke(context.phase == InputActionPhase.Performed);
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
        if (context.phase == InputActionPhase.Canceled)
        {
            OnCycleForward?.Invoke();
        }
    }

    void IStdActions.OnCycleBackward(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Canceled)
        {
            OnCycleBackward?.Invoke();
        }
    }

    public void OnUseAbility(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Canceled)
        {
            OnAbilityActivate?.Invoke();
        }
    }

    public void OnPlayerMovement(InputAction.CallbackContext context)
    {
        OnPlayerMovementHandle?.Invoke(context.ReadValue<float>());
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed)
        {
            OnJumpDown?.Invoke();
        }

        if (context.phase == InputActionPhase.Canceled)
        {
            OnJumpUp?.Invoke();
        }
    }

    public void OnPauseMenu(InputAction.CallbackContext context)
    {
        OnHandlePausingAndResuming?.Invoke();
    }

    public void OnDebugMenu(InputAction.CallbackContext context)
    {
        OnHandleDebugMenuOpenning?.Invoke();
    }

    public void OnNewaction(InputAction.CallbackContext context)
    {
    }

    /*public void OnThrowBomb(InputAction.CallbackContext context)
    {
        if(context.phase == InputActionPhase.Performed)
        {
            ThrowBomb?.Invoke();
        }
    }*/
}