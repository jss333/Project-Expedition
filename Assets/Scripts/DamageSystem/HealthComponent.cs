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
    [SerializeField] MonoFloatEventChannel healthEventChannel;


    public UnityEvent healthChange;
    public UnityEvent death;
    public UnityEvent Immune;

    private bool isImmune = false;

    // [SerializeField] private PopupLabel damageNumberPopupPrefab;
    // [SerializeField] private Transform popupLabelSource;


    private bool isDead = false;

    public bool IsDead 
    { 
        get 
        { 
            return currentHealth <= 0;
        }
        private set 
        {
            isDead = value;
        } 
    }

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
            if(currentHealth > 0)
            {
                currentHealth -= damage;
                healthChange.Invoke();
                PublishHealthPercentage();
            }
            
            if (IsDead)
            {
                Debug.Log("Deathhhhhhhhhhh Eveeeeeeeeeeeeeeeeeeeeeeent");
                death.Invoke();
                isDead = false;

                return;
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
        if(healthEventChannel != null)
        {
            healthEventChannel.Invoke(currentHealth / (float)(maxHealth));
        }

        if (healthChannel != null)
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

    public void InitializeHealth(int maxHealth)
    {
        this.maxHealth = maxHealth;
        this.currentHealth = maxHealth;
    }
}
