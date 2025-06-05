using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    public GameObject menuUI;
    public Button startButton;
    public bool isPaused = true;

    void Start()
    {
        OnToggleMenu();
    }

    public void StartGame()
    {
        ResumeGame();
    }
    
    public void ExitGame()
    {
        Debug.Log("Exiting game...");
        Application.Quit();
    }

    private void OnToggleMenu()
    {
        if (isPaused)
            ResumeGame();
        else
            PauseGame();
    }

    public void PauseGame()
    {
        Debug.Log("Pausing game");
        Time.timeScale = 0f;
        menuUI.SetActive(false);
        menuUI.SetActive(true);
        isPaused = true;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(startButton.gameObject);
    }

    public void ResumeGame()
    {
        Debug.Log("Resuming game");
        Time.timeScale = 1f;
        menuUI.SetActive(false);
        isPaused = false;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
}