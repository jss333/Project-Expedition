using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ShowPointerOnHover : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] Image menuPointerImage;

    public void OnPointerEnter(PointerEventData eventData)
    {
        menuPointerImage.enabled = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        menuPointerImage.enabled = false;
    }
}
