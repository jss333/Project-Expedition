using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElectricPlatform : HazardPlatform
{
    [Header("Electric Settings")]
    [SerializeField] private PlatformElectricCaster electricCaster;
    [SerializeField] private int damage = 10;
    [SerializeField] private float timeToCastDamage = 3f;
    [SerializeField] private float maxTimeToStayElectrified;
    float timer;
    bool isElectrified;

    private void Update()
    {
        if (isElectrified)
        {
            if (timer < maxTimeToStayElectrified)
            {
                timer += Time.deltaTime;
            }
            else
            {
                isElectrified = false;
            }
        }
    }

    protected override void ActivateAction()
    {
        base.ActivateAction();
        Debug.Log("Electrified Action");
        AudioManagerNoMixers.Singleton.PlaySFXByName("Electricity");

        isElectrified = true;
        timer = 0;
        ActivateCaster();
    }

    private void ActivateCaster()
    {
        PlatformElectricCaster platformElectricCaster = Instantiate(electricCaster, transform);
        platformElectricCaster.Configure(damage, timeToCastDamage, maxTimeToStayElectrified);

        triggerCollider.enabled = false;
    }

    protected override void ResetToSpwanPoint()
    {
        base.ResetToSpwanPoint();

        triggerCollider.enabled = true;
    }
}
