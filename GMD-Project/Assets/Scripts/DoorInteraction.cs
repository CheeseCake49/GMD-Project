using UnityEngine;

public class DoorInteraction : MonoBehaviour, IInteractable
{
    public Transform hinge; 
    public float openAngle = 90f;
    public float openSpeed = 2f;
    public bool isOpen = false;

    private Quaternion closedRotation;
    private Quaternion openRotation;

    void Start()
    {
        if (hinge == null)
        {
            hinge = transform; 
        }

        closedRotation = hinge.localRotation;
        openRotation = closedRotation * Quaternion.Euler(0, openAngle, 0);
    }

    void Update()
    {
        Quaternion targetRotation = isOpen ? openRotation : closedRotation;
        hinge.localRotation = Quaternion.Lerp(hinge.localRotation, targetRotation, Time.deltaTime * openSpeed);
    }

    public void ToggleDoor()
    {
        isOpen = !isOpen;
    }

    public void Interact()
    {
        ToggleDoor();
    }
}
