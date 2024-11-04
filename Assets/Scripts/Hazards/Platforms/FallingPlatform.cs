using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;


public class FallingPlatform : HazardPlatform
{
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private bool isVanishing;
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
                //Debug.Log(1 - timer);
                spriteRenderer.color = color;
            }
            else
            {
                isFading = false;
            }
        }
    }

    protected override void ActivateAction()
    {
        base.ActivateAction();
        Debug.Log("falling Action");

        color = Vector4.one;
        maxTime = 1f;
        timer = 0f;
        isFading = true;
  

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

        mainCollider.enabled = true;
        spriteRenderer.enabled = true;
        spriteRenderer.color = Vector4.one;
    }
}
