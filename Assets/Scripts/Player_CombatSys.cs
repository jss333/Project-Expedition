using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class B3RTCombat : MonoBehaviour
{
    [Header("References")]
    public GameObject proj;
    public GameObject pointer;

    [Header("Input Controls")]
    public InputAction fireInput;

    [Header("Parameters")]
    public float shotCooldownSec = 0.1f;
    private float shotCoolDownTimer = 0;
   

    // Start is called before the first frame update
    void Start()
    {
        fireInput.Enable();
    }


    // Update is called once per frame
    void Update()
    {
        HandleFireInput();
        RotateSpriteTowardsPointer();
    }


    private void HandleFireInput()
    {
        if (shotCoolDownTimer <= 0)
        {
            if (fireInput.IsPressed())
            {
                LaunchProjectile();
                shotCoolDownTimer = shotCooldownSec;
            }
        }
        else
        {
            shotCoolDownTimer -= Time.deltaTime;
        }
    }

    private void LaunchProjectile()
    {
        Instantiate(proj, transform.position, transform.rotation);
    }


    private void RotateSpriteTowardsPointer()
    {
        float angle = Vector2.SignedAngle(transform.right, Vector3.Normalize(pointer.transform.position - this.transform.position));
        this.transform.rotation *= Quaternion.Euler(0, 0, angle);
    }
}
