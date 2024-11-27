using FMODUnity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;


public class FallingPlatform : HazardPlatform
{
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private bool isVanishing;
    [SerializeField] private GameObject visual;
    [SerializeField] private EventReference fallingSoundEvent;
    float maxTime;
    float timer;
    Color color;
    bool isFading;

    private void Update()
    {
        if (isFading)
        {
            if(timer <  maxTime)
            {
                timer += Time.deltaTime;
                color.a = 1 - timer;
                spriteRenderer.color = color;
            }
            else
            {
                spriteRenderer.enabled = false;
                visual.SetActive(false);
                isFading = false;
            } 
        } 
    }

    protected override void ActivateAction()
    {
        base.ActivateAction();
        Debug.Log("falling Action");
        //AudioManagerNoMixers.Singleton.PlaySFXByName("FallingPlatform");
        AudioManagerNoMixers.Singleton.PlayOneShot(fallingSoundEvent, this.transform.position);

        spriteRenderer.enabled = false;
        color = Vector4.one;
        maxTime = 0.2f;
        timer = 0f; 
        visual.SetActive(true);
        isFading = true;

        triggerCollider.enabled = false;
        mainCollider.enabled = false;
        if(!isVanishing)
        {
            gameObject.AddComponent<Rigidbody2D>();
        }
    }

    protected override void ResetToSpwanPoint()
    {
        base.ResetToSpwanPoint();

        if(!isVanishing)
        {
            Destroy(gameObject.GetComponent<Rigidbody2D>());
        }

        spriteRenderer.enabled = true;
        visual.SetActive(false);
        mainCollider.enabled = true;
        spriteRenderer.enabled = true;
        spriteRenderer.color = Vector4.one;
    }
}
