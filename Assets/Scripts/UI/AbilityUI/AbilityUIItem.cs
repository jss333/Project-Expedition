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

        private int uiObjectId;

        public void InitializeUI(AbilitySo abilitySo, int id)
        {
            highLightImageGameObject.SetActive(false);
            icon.sprite = abilitySo.AbilityIcon; 
            icon.color = abilitySo.IconColor;

            SetId(id);
        }

        public void ShowHighLight()
        {
            highLightImageGameObject.SetActive(true);
        }

        public void HideHighLight()
        {
            highLightImageGameObject.SetActive(false);
        }

        private void SetId(int index)
        {
            uiObjectId = index;
        }

        public void UpdateVisual(AbilitySo abilitySo)
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