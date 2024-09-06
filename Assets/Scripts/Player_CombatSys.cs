using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class B3RTCombat : MonoBehaviour
{
    [Header("References")]
    public GameObject proj;
    public GameObject pointer;
    private RandomPitchAudioSource audioSource;

    [Header("Input Controls")]
    public InputAction fireInput;

    [Header("Parameters")]
    public float shotCooldownSec = 0.2f;
    private float timeOfLastShot;
    public AudioClip shotSFX;
   

    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<RandomPitchAudioSource>();
        fireInput.Enable();
        timeOfLastShot = Time.time - shotCooldownSec;
    }


    // Update is called once per frame
    void Update()
    {
        HandleFireInput();
        RotateSpriteTowardsPointer();
    }


    private void HandleFireInput()
    {
        bool canFireAgain = Time.time - timeOfLastShot >= shotCooldownSec;
        if (fireInput.IsPressed() && canFireAgain)
        {
            timeOfLastShot = Time.time;
            LaunchProjectile();
            audioSource.PlayAudioWithRandomPitch(shotSFX);
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
