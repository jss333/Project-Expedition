using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace AbilitySystem
{
    public class AbilityUIItem : MonoBehaviour
    {
        [SerializeField] private Image icon;
        [SerializeField] private Image dimmedImage;
        [SerializeField] private GameObject highLightImageGameObject;

        public void InitializeUI(AbilitySo abilitySo)
        {
            highLightImageGameObject.SetActive(false);
            icon.sprite = abilitySo.AbilityIcon; 
            icon.color = abilitySo.IconColor;
        }

        public void ShowHighLight()
        {
            highLightImageGameObject.SetActive(true);
        }

        public void HideHighLight()
        {
            highLightImageGameObject.SetActive(false);
        }

        public void UpdateCooldownVisual(AbilitySo abilitySo)
        {
            if(abilitySo.InCoolDown())
            {
                dimmedImage.gameObject.SetActive(true);
                dimmedImage.fillAmount = 1 - (float)(abilitySo.CoolDownTime / abilitySo.MaxCoolDownTime);
            }
            else
            {
                dimmedImage.gameObject.SetActive(false);
            }
        }
    }
}