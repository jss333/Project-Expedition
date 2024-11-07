using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombStickyController : MonoBehaviour
{
    [Header("Parameters")]
    private float bombSpeed;
    [SerializeField] private float durationInAir;
    [SerializeField] private float durationAfterStick;
    [SerializeField] private int bombExplosionDamage = 300;
    [SerializeField] private float bombExplosionRadius = 1f;
    [SerializeField] private AnimationCurve bombSpeedCurve;
    [SerializeField] private float speedMultiplier = 20;

    [Header("References")]
    SpriteRenderer spriteRenderer;
    Animator animator;

    [Header("Variables")]
    private Vector2 spawnLocation;
    private Vector2 mouseLocation;
    private bool sticking = false;
    private bool hasExploded = false;
    private Vector2 explodeLocation;

    private float timeSinceSpawn = 0;
    private float timeSinceSticking = 0;
    private float mouseDistance;
    private float sampleSpeed;

    private GameObject stuckToObject;
    private Vector3 offsetStickPostion;

    private GameObject player;
    void Start()
    {
        spawnLocation = transform.position;
        mouseLocation = Input.mousePosition;
        player = FindAnyObjectByType<PlayerMovement>().gameObject;
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();

        mouseDistance = Vector2.Distance(spawnLocation, mouseLocation);
        sampleSpeed = bombSpeedCurve.Evaluate(mouseDistance / 2203);
        bombSpeed = sampleSpeed * speedMultiplier;


        ThrowBomb();
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
    }
    private void Explode()
    {
        //code a radius for the explosion damage
        if (bombExplosionRadius > 0)
        {
            //check radius
            var hitEnemies = Physics2D.OverlapCircleAll(transform.position, bombExplosionRadius);

            //for each in radius of explosion deal damage
            foreach (var hit in hitEnemies)
            {
                if (hit.tag == "Boss")
                {
                    var closestPoint = hit.ClosestPoint(transform.position);
                    var distance = Vector3.Distance(closestPoint, transform.position);

                    var damagePercentCalc = Mathf.InverseLerp(bombExplosionRadius, 0, distance);

                    hit.GetComponent<BossController>().TakeDamage((int)(damagePercentCalc * bombExplosionDamage));
                }
                else if (hit.tag == "Minion")
                {
                    var closestPoint = hit.ClosestPoint(transform.position);
                    var distance = Vector3.Distance(closestPoint, transform.position);

                    var damagePercentCalc = Mathf.InverseLerp(bombExplosionRadius, 0, distance);

                    if (hit.GetComponent<MinionController>() != null)
                    {
                        hit.GetComponent<MinionController>().TakeDamage((int)(damagePercentCalc * bombExplosionDamage));
                    }
                    else if (hit.GetComponent<MinionTesting>() != null)
                    {
                        hit.GetComponent<MinionTesting>().TakeDamage((int)(damagePercentCalc * bombExplosionDamage));
                    }
                }
            }
        }
    }

    private void ThrowBomb()
    {
        GetComponent<Rigidbody2D>().AddForce((Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position).normalized * bombSpeed, ForceMode2D.Impulse);
    }
    private void DurationLogic()
    {
        //while in air
        if (timeSinceSpawn < durationInAir && !sticking)
        {
            timeSinceSpawn += Time.deltaTime;
        }
        //after sticking to something
        else if (timeSinceSticking < durationAfterStick && sticking)
        {
            timeSinceSticking += Time.deltaTime;
            if (stuckToObject != null)
            {
                transform.position = stuckToObject.transform.position - offsetStickPostion;
            }
            else
            {
                sticking = false;
            }
            
        }
        //after stick countdown
        else if (timeSinceSticking >= durationAfterStick)
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
        float scale = 1;
        while (scale < 2)
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
        animator.SetTrigger("Death");
    }
}
