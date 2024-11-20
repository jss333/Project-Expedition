using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class MinionTesting : MonoBehaviour
{
    [Header("References - Popup labels")]
    [SerializeField] private PopupLabel damageNumberPopupPrefab;
    [SerializeField] private Transform popupLabelSource;

    private HealthComponent healthComponent;
    [SerializeField] Image healthSlider;
    private int saveOldHealth;
    private void Start()
    {
        healthComponent = this.gameObject.GetComponentInChildren<HealthComponent>();
        saveOldHealth = healthComponent.getCurrentHealth();
        changeHealthBar();
    }
    //Basic properties and component refs...
    public void DestroyThisMinion()
    {
        Destroy(this.gameObject);
    }
    public void changeHealthBar()
    {
        if(saveOldHealth !=  healthComponent.getCurrentHealth())
        {
            MinionTakesDamage(saveOldHealth - healthComponent.getCurrentHealth());
            saveOldHealth = healthComponent.getCurrentHealth();
        }
        //healthSlider.fillAmount = healthComponent.getCurrentHealth()/ (float)(healthComponent.getMaxHealth());
    }
    public void MinionTakesDamage(int damage)
    {
        //quick hits will stack numbers (future)
        PopupLabel dmgNumPopup = Instantiate(damageNumberPopupPrefab, popupLabelSource.position, Quaternion.identity);
        dmgNumPopup.UpdateLabel(damage.ToString());
    }
    

}