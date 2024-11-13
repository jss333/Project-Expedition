using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MinionTesting : MonoBehaviour
{
    [Header("References - Popup labels")]
    [SerializeField] private PopupLabel damageNumberPopupPrefab;
    [SerializeField] private Transform popupLabelSource;

    [Header("Properties")]
    //Basic properties and component refs...
    private HealthBar healthBar;
    public float maxhealth = 500;
    public float currentHealth;
    public void Start()
    {
        SetUpMinion();
    }
    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 8)
        {
            TakeDamage(collision.gameObject.GetComponent<PlayerProjectile>().damageAmt);
            Destroy(collision.gameObject);
        }
        return;
    }
    public void TakeDamage(int damage)
    {
        AudioManagerNoMixers.Singleton.PlaySFXByName("MinionHit");
        currentHealth -= damage;
        SpawnDamageNumberPopupLabel(damage);
        healthBar.SetHealth((int)currentHealth);
        if (currentHealth <= 0)
        {
            AudioManagerNoMixers.Singleton.PlaySFXByName("MinionDeath");
            DestroyThisMinion();
        }
    }
    private void DestroyThisMinion()
    {
        Destroy(this.gameObject);
    }

    private void SetUpMinion()
    {
        healthBar = GetComponentInChildren<HealthBar>();
        currentHealth = maxhealth;
        healthBar.SetMaxHealth((int)maxhealth);
    }
    private void SpawnDamageNumberPopupLabel(int damage)
    {
        //quick hits will stack numbers (future)
        PopupLabel dmgNumPopup = Instantiate(damageNumberPopupPrefab, popupLabelSource.position, Quaternion.identity);
        dmgNumPopup.UpdateLabel(damage.ToString());
    }
}