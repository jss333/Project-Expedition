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
    }
}