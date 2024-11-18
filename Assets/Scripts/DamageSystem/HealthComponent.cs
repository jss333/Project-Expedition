using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
public class HealthComponent : MonoBehaviour
{
    public delegate void HealthChangedHandler(object source, float oldHealth, float newHealth);
    public event HealthChangedHandler OnHealthChanged;
    [SerializeField] private int maxHealth = 1000;
    [SerializeField] FloatEventChannel healthChannel;

    public UnityEvent healthChange;
    public UnityEvent death;

    // [SerializeField] private PopupLabel damageNumberPopupPrefab;
    // [SerializeField] private Transform popupLabelSource;

    private int currentHealth = 0;

    public bool IsDead => currentHealth <= 0;

    private void Awake()
    {
        currentHealth = maxHealth;
    }
    private void Start()
    {
        PublishHealthPercentage(); 
    }
    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        healthChange.Invoke();
        PublishHealthPercentage();
        if(IsDead)
        {
            death.Invoke();
        }
        if(OnHealthChanged != null)
        {
            OnHealthChanged?.Invoke(this, currentHealth + damage, currentHealth);
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

    /*void MinionTakesDamage(int damage)
    {
        if(damageNumberPopupPrefab != null)
        {
            //quick hits will stack numbers (future)
            PopupLabel dmgNumPopup = Instantiate(damageNumberPopupPrefab, popupLabelSource.position, Quaternion.identity);
            dmgNumPopup.UpdateLabel(damage.ToString());
        }
    }*/
}
