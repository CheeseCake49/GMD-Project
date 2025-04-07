using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInteraction : MonoBehaviour
{
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

    
}



