using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossHealthComponent : HealthComponent
{
    //idk tailor this to the boss later
    [SerializeField] PopupLabel damageNumberPopupPrefab;
    [SerializeField] PopupLabel immunePopupPrefab;
    [SerializeField] Transform popupLabelSource;

    [SerializeField] private float stopOverflowDamageNumbers = 1f;
    [SerializeField] private float overflowDamageCooldown = 1f;

    private bool damageNumActive = true;

    public override void Start()
    {
        base.Start();
    }
    private void Update()
    {
        if (damageNumActive)
        {
            stopOverflowDamageNumbers -= Time.deltaTime;
            if (stopOverflowDamageNumbers <= -3)
            {
                damageNumActive = false;
            }
        }
    }
    public bool CurrentHealthPercentLessThan(float thresholdPercent)
    {
        return (float)getCurrentHealth() / getMaxHealth() <= thresholdPercent / 100;
    }
    public void SpawnDamageNumberPopupLabel(int damage)
    {
        //quick hits will stack numbers (future)
        PopupLabel dmgNumPopup = Instantiate(damageNumberPopupPrefab, popupLabelSource.position, Quaternion.identity);
        dmgNumPopup.UpdateLabel(damage.ToString());
    }
    public void SpawnImmunePopupLabel()
    {
        Instantiate(immunePopupPrefab, popupLabelSource.position, Quaternion.identity);
        stopOverflowDamageNumbers = overflowDamageCooldown;
    }

    public void TryImmunePopup()
    {
        if (damageNumActive == false)
        {
            damageNumActive = true;
            SpawnImmunePopupLabel();
        }
        else if (damageNumActive && stopOverflowDamageNumbers < 0)
        {
            SpawnImmunePopupLabel();
        }
    }
}
