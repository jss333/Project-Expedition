using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactor : MonoBehaviour
{
    [SerializeField] private GameObject uiObject;
    [SerializeField] private List<IInteractable> currentInteractables = new List<IInteractable>();

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.F))
        {
            if(currentInteractables.Count > 0 )
            {
                for(int i = 0; i < currentInteractables.Count; i++)
                {
                    currentInteractables[i].Interact();
                }
            }
        }
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        IInteractable interactable = collision.GetComponent<IInteractable>();

        if(interactable != null)
        {
            if(!currentInteractables.Contains(interactable))
            {
                currentInteractables.Add(interactable);

                ControlUI();
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        IInteractable interactable = collision.GetComponent<IInteractable>();

        if (interactable != null)
        {
            if (currentInteractables.Contains(interactable))
            {
                currentInteractables.Remove(interactable);

                ControlUI();
            }
        }
    }

    private void ControlUI()
    {
        if (currentInteractables.Count > 0)
        {
            uiObject.SetActive(true);
        }
        else
        {
            uiObject.SetActive(false);
        }
    }
}
