using UnityEngine;

public class FinalScreenActivator : MonoBehaviour, IInteractable
{
    [SerializeField] private FinalScreenMenu menu;
    public void Interact()
    {
        menu.OpenMenu();
    }
}
