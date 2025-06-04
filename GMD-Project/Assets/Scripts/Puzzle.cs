using UnityEngine;

public class Puzzle : MonoBehaviour, IInteractable
{
    public GameObject puzzleUI;
    public Renderer puzzleRenderer;
    public PlayerMovement player;
    public CameraController camera;

    private bool isOpen;

    public void Interact()
    {
        isOpen = !isOpen;

        if (isOpen)
        {
            puzzleUI.SetActive(true);
            puzzleRenderer.enabled = false;
            player.enabled = false;
            camera.enabled = false;
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
        else
        {
            puzzleUI.SetActive(false);
            puzzleRenderer.enabled = true;
            player.enabled = true;
            camera.enabled = true;
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }
}

