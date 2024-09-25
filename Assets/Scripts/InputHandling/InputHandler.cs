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
        OnAbilityActivate?.Invoke();
    }
}
