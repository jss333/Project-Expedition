using FMODUnity;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CyclingElectricPlatform : MonoBehaviour
{
    [Header("Refs")]
    [SerializeField] private TextMeshProUGUI timerText;
    [SerializeField] private GameObject timerHolder;
    [SerializeField] private PlatformElectricCaster electricityCaster;
    [SerializeField] private EventReference triggeredEventSound;

    [Header("Settings")]
    [SerializeField] private float timeToStartCycling;
    [SerializeField] private float timeToShowElectricity;
    [SerializeField] private float timeToStayElectrified;
    [SerializeField] private int damage;

    float timer;
    bool spawned;

    bool started;

    private void FixedUpdate()
    {
        timer += Time.deltaTime;


        if (timer > timeToStartCycling && !started)
        {
            started = true;
            timer = 0;
        }

        if (!started)
        {
            return;
        }

        if (timer < timeToShowElectricity)
        {
            timerHolder.SetActive(true);

            float t = timeToShowElectricity - timer;
            timerText.text = ((int)t).ToString();
        }

        if (timer > timeToShowElectricity)
        {
            //Show Electricity
            if(!spawned)
            {
                Debug.Log("Spawned");
                AudioManagerNoMixers.Singleton.PlayOneShot(triggeredEventSound, this.transform.position);
                PlatformElectricCaster platformElectricCaster = Instantiate(electricityCaster, transform);
                platformElectricCaster.Configure(this, damage, 0.25f, timeToStayElectrified);
                spawned = true;
            }
        }

        /*if (timer > timeToShowElectricity + vanishingTime)
        {
            //Update Vanishing
        }

        if (timer > timeToStayElectrified + timeToShowElectricity + vanishingTime)
        {
            //Remove Electricity
            timer = 0;


        }  */
    }

    public void ResetTimer()
    {
        spawned = false;
        timer = 0; 
    }

}
