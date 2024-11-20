using Platformer.Mechanics;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BombThrowController : MonoBehaviour
{
    [Header("Parameters")]
    private float bombSpeed;
    [SerializeField] private float duration;
    [SerializeField] private int bombExplosionDamage = 300;
    [SerializeField] private float bombExplosionRadius = 2f;
    [SerializeField] private AnimationCurve bombSpeedCurve;
    [SerializeField] private AnimationCurve beepingSpeedCurve;

    private Animator animator;

    [Header("Variables")]
    private Vector2 spawnLocation;
    private Vector2 mouseLocation;
    private Vector2 explodeLocation;

    private float timeSinceSpawn = 0;
    private float mouseDistance;
    private float sampleSpeed;
    private bool isBeeping = false;

    private bool hasExploded = false;

    private GameObject player;
    void Start()
    {
        spawnLocation = transform.position;
        mouseLocation = Input.mousePosition;
        player = FindAnyObjectByType<PlayerMovement>().gameObject;
        animator = GetComponent<Animator>();

        mouseDistance = Vector2.Distance(spawnLocation, mouseLocation);
        sampleSpeed = bombSpeedCurve.Evaluate(mouseDistance/2203);

        bombSpeed = sampleSpeed * 20;

        ThrowBomb();
        
    }

    private void Update()
    {
        ExplodeLogic();
        BeepingLogic();
    }
    private void Explode()
    {
        GetComponent<Rigidbody2D>().freezeRotation = true;
        //code a radius for the explosion damage
        if (bombExplosionRadius > 0)
        {
            //check radius
            var hitEnemies = Physics2D.OverlapCircleAll(transform.position, bombExplosionRadius);
            PlayExplodeAudio();

            //for each in radius of explosion deal damage
            foreach(var hit in hitEnemies)
            {
                if(hit.GetComponent<PlayerHealthComponent>() != null)
                {
                    var closestPoint = hit.ClosestPoint(transform.position);
                    var distance = Vector3.Distance(closestPoint, transform.position);

                    var damagePercentCalc = Mathf.InverseLerp(bombExplosionRadius, 0, distance);
                    hit.GetComponent<HealthComponent>().TakeDamage((int)(damagePercentCalc * bombExplosionDamage)/7);
                }
                else if (hit.GetComponent<HealthComponent>() != null)
                {
                    var closestPoint = hit.ClosestPoint(transform.position);
                    var distance = Vector3.Distance(closestPoint, transform.position);

                    var damagePercentCalc = Mathf.InverseLerp(bombExplosionRadius, 0, distance);
                    hit.GetComponent<HealthComponent>().TakeDamage((int)(damagePercentCalc * bombExplosionDamage));
                }
                
            }
            //Gizmos.DrawSphere(transform.position, bombExplosionRadius);
        }
    }

    private void ThrowBomb()
    {
        GetComponent<Rigidbody2D>().AddForce((Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position).normalized * bombSpeed, ForceMode2D.Impulse);
        PlayLaunchAudio();
        isBeeping = true;
    }
    public void Destroy()
    {
        Destroy(gameObject);
    }

    public void enlargeExplosion()
    {
        transform.localScale = Vector3.one * bombExplosionRadius;
    }

    private void ExplodeLogic()
    {
        //alive duration
        if (timeSinceSpawn < duration)
        {
            timeSinceSpawn += Time.deltaTime;
        }
        else if (!hasExploded)
        {
            hasExploded = true;
            explodeLocation = transform.position;
            GetComponent<Animator>().SetTrigger("Explode");
            isBeeping = false;
            PlayExplodeAudio();
        }
        if (hasExploded)
        {
            transform.position = explodeLocation;
        }
    }
    private IEnumerator Grow()
    {
        float scale = 1;
        while (scale < bombExplosionRadius)
        {
            scale += 0.05f;
            transform.localScale = new Vector3(scale, scale, scale);
            yield return new WaitForSeconds(0.05f);
        }
    }

    public void growInSize()
    {
        StartCoroutine(Grow()); 
    }
    public void PlayExplodeAudio()
    {
        AudioManagerNoMixers.Singleton.PlaySFXByName("BombExplosion");
    }
    public void PlayLaunchAudio()
    {
        AudioManagerNoMixers.Singleton.PlaySFXByName("BombLaunch");
    }
    public void PlayBeepAudio()
    {
        AudioManagerNoMixers.Singleton.PlaySFXByName("BombBeep");
    }
    public void BeepingLogic()
    {
        if (isBeeping)
        {
            animator.speed = beepingSpeedCurve.Evaluate(timeSinceSpawn / duration);
            //Debug.Log(animator.speed);
        }
        else if (!isBeeping)
        {
            animator.speed = 1;
        }
    }
}
