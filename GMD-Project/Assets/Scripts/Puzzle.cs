using UnityEngine;

public class Puzzle : MonoBehaviour, IInteractable
{
    public GameObject puzzleUI;
    public Renderer puzzleRenderer;
    public PlayerMovement player; 

    private bool isOpen;

    public void Interact()
    {
        isOpen = !isOpen;

        if (isOpen)
        {
            puzzleUI.SetActive(true);
            puzzleRenderer.enabled = false;
            player.enabled = false;
        }
        else
        {
            puzzleUI.SetActive(false);
            puzzleRenderer.enabled = true;
            player.enabled = true;
        }
    }
}

