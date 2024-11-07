using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class VanishingPlatform : MonoBehaviour
{
    [Header("Refs")]
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private Collider2D mainCollider;
    [SerializeField] private TextMeshProUGUI timerText;
    [SerializeField] private GameObject timerHolder;

    [Header("Vanishing Settings")]
    [SerializeField] private float timeToStartCycling;
    [SerializeField] private float timeToVanish;
    [SerializeField] private float timeToStayVanished;
    [SerializeField] private float vanishingTime;

    float timer;
    Color color;

    bool started;

    private void FixedUpdate()
    {
        timer += Time.deltaTime;

        
        if(timer > timeToStartCycling && !started)
        {
            started = true;
            timer = 0;
        }

        if (!started)
        {
            return;
        }

        if (timer < timeToVanish)
        {
            timerHolder.SetActive(true);

            float t = timeToVanish - timer;
            Debug.Log(t);
            timerText.text = ((int)t).ToString();
        }

        if (timer > timeToVanish)
        {
            //Vanish

            timerHolder.SetActive(false);

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
