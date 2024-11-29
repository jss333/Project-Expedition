using FMODUnity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombStickyController : MonoBehaviour
{
    [Header("Parameters")]
    private float bombSpeed;
    [SerializeField] private float duration =4;
    [SerializeField] private int bombExplosionDamage = 300;
    [SerializeField] private float bombExplosionRadius = 1.5f;
    [SerializeField] private AnimationCurve bombSpeedCurve;
    [SerializeField] private AnimationCurve beepingSpeedCurve;
    [SerializeField] private float speedMultiplier = 20;

    [Header("References")]
    SpriteRenderer spriteRenderer;
    Animator animator;

    [Header("SFX")]
    [SerializeField] private EventReference bombExplosionSFX;
    [SerializeField] private EventReference bombLaunchSFX;
    [SerializeField] private EventReference bombBeepSFX;
    [SerializeField] private EventReference bombAttachSFX;

    [Header("Variables")]
    private Vector2 spawnLocation;
    private Vector2 mouseLocation;
    private bool sticking = false;
    private bool hasExploded = false;
    private Vector2 explodeLocation;

    private float stickAudioCooldown = 2;
    private bool stickAudioCooldownActive = false;
    private float stickAudioTimeStamp;
    private bool isBeeping = false;

    private float timeSinceSpawn = 0;
    private float timeSinceSticking = 0;
    private float mouseDistance;
    private float sampleSpeed;

    private GameObject stuckToObject;
    private Vector3 offsetStickPostion;

    void Start()
    {
        spawnLocation = transform.position;
        mouseLocation = Input.mousePosition;
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();

        mouseDistance = Vector2.Distance(spawnLocation, mouseLocation);
        sampleSpeed = bombSpeedCurve.Evaluate(mouseDistance / 2203);
        bombSpeed = sampleSpeed * speedMultiplier;


        ThrowBomb();
        isBeeping = true;
    }

    private void Update()
    {
        //alive duration
        if(hasExploded)
        {
            ExplodeLogic();
        }
        else
        {
            DurationLogic();
        }
        BeepingLogic();
    }

    public void InitializeBombSettings(float durationInAir, int explosionDamage, float explosionRadius, float speedMuliplier = 20)
    {
        this.duration = durationInAir;
        this.bombExplosionDamage = explosionDamage;
        this.bombExplosionRadius = explosionRadius;
        this.speedMultiplier = speedMuliplier;
    }

    private void Explode()
    {
        //code a radius for the explosion damage
        if (bombExplosionRadius > 0)
        {
            //check radius
            var hitEnemies = Physics2D.OverlapCircleAll(transform.position, bombExplosionRadius);
            PlayExplodeAudio();
            

            //for each in radius of explosion deal damage
            foreach (var hit in hitEnemies)
            {
                if (hit.GetComponent<PlayerHealthComponent>() != null)
                {
                    var closestPoint = hit.ClosestPoint(transform.position);
                    var distance = Vector3.Distance(closestPoint, transform.position);

                    var damagePercentCalc = Mathf.InverseLerp(bombExplosionRadius, 0, distance);
                    hit.GetComponent<HealthComponent>().TakeDamage((int)(damagePercentCalc * bombExplosionDamage) / 7);
                }
                else if (hit.GetComponent<HealthComponent>() != null)
                {
                    var closestPoint = hit.ClosestPoint(transform.position);
                    var distance = Vector3.Distance(closestPoint, transform.position);

                    var damagePercentCalc = Mathf.InverseLerp(bombExplosionRadius, 0, distance);
                    hit.GetComponent<HealthComponent>().TakeDamage((int)(damagePercentCalc * bombExplosionDamage));
                }

            }
        }
    }

    private void ThrowBomb()
    {
        GetComponent<Rigidbody2D>().AddForce((Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position).normalized * bombSpeed, ForceMode2D.Impulse);
        PlayLaunchAudio();
    }
    private void DurationLogic()
    {
        //while in air
        if (timeSinceSpawn < duration)
        {
            GetComponent<Rigidbody2D>().freezeRotation = false;
            timeSinceSpawn += Time.deltaTime;
        }
        //after sticking to something
        if (timeSinceSpawn < duration && sticking)
        {
            GetComponent<Rigidbody2D>().freezeRotation = true;
            if (stuckToObject != null)
            {
                transform.position = stuckToObject.transform.position - offsetStickPostion;
            }
            else
            {
                sticking = false;
            }
            
        }
        //explosion countdown
        if (timeSinceSpawn >= duration)
        {
            explodeLocation = transform.position;
            hasExploded = true;
            TriggerDeathAnim();
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision != null && !sticking)
        {
            stuckToObject = collision.gameObject;
            offsetStickPostion = (stuckToObject.transform.position - transform.position);
            StickingEffects();
        }
    }
    private void StickingEffects()
    {
        //add in effects later
        sticking = true;
        //add a cooldown to the sticking sound effect
        if (!stickAudioCooldownActive)
        {
            stickAudioCooldownActive = true;
            stickAudioTimeStamp = stickAudioCooldown + Time.deltaTime;
            PlayBombStickAudio();
        }
        else if(stickAudioTimeStamp < Time.deltaTime)
        {
            stickAudioCooldownActive = false;
        }
        
    }
    private void ExplodeLogic()
    {
        transform.position = explodeLocation;
        //animator.SetTrigger("Explode");
    }
    private IEnumerator FadeOut()
    {
        float alphaVal = spriteRenderer.color.a;
        Color tmp = spriteRenderer.color;

        while (spriteRenderer.color.a < 1)
        {
            alphaVal += 0.01f;
            tmp.a = alphaVal;
            spriteRenderer.color = tmp;

            yield return new WaitForSeconds(0.01f); // update interval
        }
    }
    private IEnumerator Grow()
    {
        float scale = 1f;
        while (scale < bombExplosionRadius || scale > bombExplosionRadius)
        {
            if(bombExplosionRadius > 1)
            {
                scale += 0.05f;
            }
            else if(bombExplosionRadius < 1)
            {
                scale -= 0.05f;
            }
            transform.localScale = new Vector3(scale, scale, scale);
            yield return new WaitForSeconds(0.05f);
        }
    }

    public void growInSize()
    {
        StartCoroutine(Grow());
    }
    public void EndExplodeEffects()
    {
        StartCoroutine(FadeOut());
    }
    public void Destroy()
    {
        Destroy(gameObject);
    }
    private void TriggerDeathAnim()
    {
        Debug.Log("Sticky Explode");
        animator.SetTrigger("Explode");
        isBeeping = false;
        animator.speed = 1;
    }
    public void PlayExplodeAudio()
    {
        AudioManagerNoMixers.Singleton.PlayOneShot(bombExplosionSFX, this.transform.position);
    }
    public void PlayLaunchAudio()
    {
        AudioManagerNoMixers.Singleton.PlayOneShot(bombLaunchSFX, this.transform.position);
    }
    public void PlayBeepAudio()
    {
        AudioManagerNoMixers.Singleton.PlayOneShot(bombBeepSFX, this.transform.position);
    }
    public void PlayBombStickAudio()
    {
        AudioManagerNoMixers.Singleton.PlayOneShot(bombAttachSFX, this.transform.position);
    }
    public void BeepingLogic()
    {
        if (isBeeping)
        {
            animator.speed = beepingSpeedCurve.Evaluate((timeSinceSpawn + timeSinceSticking) / (duration));
            //Debug.Log(animator.speed);
        }
    }
}
