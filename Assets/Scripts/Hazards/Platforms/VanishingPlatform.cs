using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VanishingPlatform : MonoBehaviour
{
    [Header("Refs")]
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private Collider2D mainCollider;

    [Header("Vanishing Settings")]
    [SerializeField] private float timeToVanish;
    [SerializeField] private float timeToStayVanished;
    [SerializeField] private float vanishingTime;

    float timer;
    Color color;

    private void FixedUpdate()
    {
        timer += Time.deltaTime;

        if(timer > timeToVanish)
        {
            //Vanish

            float t = (float)(timeToVanish + vanishingTime - timer) / (float)(vanishingTime);
            
            color = Vector4.one;
            color.a = t;
            spriteRenderer.color = color;
        }

        if (timer > timeToVanish + vanishingTime)
        {
            //Update Vanishing
            mainCollider.enabled = false;
        }

        if (timer > timeToStayVanished + timeToVanish + vanishingTime)
        {
            //float t = timer / (timeToStayVanished + timeToVanish + vanishingTime - timer);
            //SHowBack
            timer = 0;
            color.a = 1;
            spriteRenderer.color = color;
            mainCollider.enabled = true;
        }
    }

    private void HideVisual()
    {
        
    }
}
