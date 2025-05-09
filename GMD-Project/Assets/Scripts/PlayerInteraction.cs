using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInteraction : MonoBehaviour
{
    public float interactionDistance;
    public GameObject interactionText;
    public LayerMask interactionLayers;
    private DoorInteraction nearbyDoor;

    // This is called by the Input System when "Interact" (E key) is pressed
    void OnInteract()
    {
        Debug.Log("Interact Pressed");

        if (nearbyDoor != null)
        {
            nearbyDoor.ToggleDoor();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Entered trigger with " + other.name);

        DoorInteraction door = other.GetComponentInParent<DoorInteraction>();
        if (door != null)
        {
            Debug.Log("Found door script on " + door.name);
            nearbyDoor = door;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        DoorInteraction door = other.GetComponentInParent<DoorInteraction>();
        if (door != null && door == nearbyDoor)
        {
            nearbyDoor = null;
        }
    }

    void Update()
    {
        //RaycastHit variable which will collect information from objects 
        RaycastHit hit;

        //If the raycast hits something,
        if(Physics.Raycast(transform.position, transform.forward, out hit, interactionDistance, interactionLayers))
        {
            //If the object it hit contrains the letter script,
            if (hit.collider.gameObject.GetComponent<Letter>())
            {
                //The interaction text will enable
                interactionText.SetActive(true);

                //If the E key is pressed,
                if (Input.GetKeyDown(KeyCode.E))
                {
                    //The letter component is accessed and the letter will open or close
                    hit.collider.gameObject.GetComponent<Letter>().openCloseLetter();
                }
            }
            //else, the interaction text is set false.
            else
            {
                interactionText.SetActive(false);
            }
        }
        //else, the interaction text is set false.
        else
        {
            interactionText.SetActive(false);
        }
    }

    
}



