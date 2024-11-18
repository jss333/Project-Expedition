using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
public class HealthComponent : MonoBehaviour
{
    public delegate void HealthChangedHandler(object source, float oldHealth, float newHealth);
    public event HealthChangedHandler OnHealthChanged;
    [SerializeField] private int maxHealth = 1000;
    [SerializeField] private int currentHealth = 0;
    [SerializeField] FloatEventChannel healthChannel;


    public UnityEvent healthChange;
    public UnityEvent death;
    public UnityEvent Immune;

    private bool isImmune = false;

    // [SerializeField] private PopupLabel damageNumberPopupPrefab;
    // [SerializeField] private Transform popupLabelSource;



    public bool IsDead => currentHealth <= 0;

    public virtual void Awake()
    {
        currentHealth = maxHealth;
    }
    public virtual void Start()
    {
        PublishHealthPercentage(); 
    }
    public virtual void TakeDamage(int damage)
    {
        if(!isImmune)
        {
            currentHealth -= damage;
            healthChange.Invoke();
            PublishHealthPercentage();
            if (IsDead)
            {
                death.Invoke();
            }
            if (OnHealthChanged != null)
            {
                OnHealthChanged?.Invoke(this, currentHealth + damage, currentHealth);
            }
        }
        else if (isImmune)
        {
            Immune.Invoke();
        }
    }
    void PublishHealthPercentage()
    {
        if(healthChannel != null)
        {
            healthChannel.Invoke(currentHealth / (float)(maxHealth));
        }
    }
    public int getCurrentHealth()
    {
        return currentHealth;
    }
    public int getMaxHealth() 
    {
        return maxHealth;
    }

    public bool GetIsImmune()
    {
        return isImmune;
    }

    public void SetIsImmune(bool value)
    {
        isImmune = value;
    }
}
