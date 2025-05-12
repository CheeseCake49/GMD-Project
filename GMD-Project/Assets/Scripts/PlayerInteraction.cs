using System;
using UnityEditor.Rendering.LookDev;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInteraction : MonoBehaviour
{
    public float interactionDistance;
    public GameObject interactionText;
    public LayerMask interactionLayers;
    public Transform cameraPosition;

    private IInteractable currentTarget;
    
    private void OnInteract()
    {
        if (currentTarget != null)
        {
            currentTarget.Interact();
        }
    }

    private void Update()
    {
        //RaycastHit variable which will collect information from objects 
        RaycastHit hit;

        //If the raycast hits something,
        if (Physics.Raycast(cameraPosition.position, cameraPosition.forward, out hit, interactionDistance, interactionLayers))
        {
            IInteractable interactable = hit.collider.GetComponent<IInteractable>();
            if (interactable != null)
            {
                if (interactable != currentTarget)
                {
                    currentTarget = interactable;
                    interactionText.SetActive(true);
                }
                return;
            }
        }

        currentTarget = null;
        interactionText.SetActive(false);
    }
    
}



