using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerBasicShot : MonoBehaviour
{
    [Header("References")]
    public PlayerProjectile projectilePrefab;
    public GameObject pointer;
    [SerializeField] private Transform shootPoint;
    private RandomPitchAudioSource audioSource;

    [Header("Basic Shot Parameters")]
    public int shotDmg = 10;
    public float shotSpeed = 20f;
    public float shotIntervalSec = 0.05f;
    private float timeOfLastShot;
    public AudioClip shotSFX;

    private bool isFired;

    private void Awake()
    {
        InputHandler.Singleton.OnWeaponFire += SetIsFired;
    }

    void Start()
    {
        audioSource = GetComponent<RandomPitchAudioSource>();
        timeOfLastShot = Time.time - shotIntervalSec;
    }

    private void OnDestroy()
    {
        InputHandler.Singleton.OnWeaponFire -= SetIsFired;
    }

    void Update()
    {
        RotateTowardsPointer();
        HandleFireInput();
    }


    private void RotateTowardsPointer()
    {
        float angle = Vector2.SignedAngle(transform.right, Vector3.Normalize(pointer.transform.position - this.transform.position));
        this.transform.rotation *= Quaternion.Euler(0, 0, angle);
    }


    private void HandleFireInput()
    {
        if (IsFired() && CanFireAgain())
        {
            timeOfLastShot = Time.time;
            LaunchProjectile();
            AudioManagerNoMixers.Singleton.PlaySFXByName("PlayerShoots");
        }

        bool CanFireAgain()
        {
            return Time.time - timeOfLastShot >= shotIntervalSec;
        }
    }

    private void SetIsFired(bool isFired)
    {
        this.isFired = isFired; 
    }

    private bool IsFired()
    {
        return this.isFired;
    }


    private void LaunchProjectile()
    {
        PlayerProjectile proj = Instantiate(projectilePrefab, shootPoint.position, shootPoint.rotation);
        proj.SetVelocityAndDamageAmt(shotSpeed, shotDmg);
    }
}
