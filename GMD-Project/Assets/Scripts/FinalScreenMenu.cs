using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class FinalScreenMenu : MonoBehaviour
{

    [SerializeField] private Canvas canvasUI;
    [SerializeField] private Canvas deletingProgress;
    [SerializeField] private Button deleteButton;

    void Start()
    {
        canvasUI.gameObject.SetActive(false);
        deletingProgress.gameObject.SetActive(false);
    }
    
    public void OpenMenu()
    {
        canvasUI.gameObject.SetActive(true);
        deletingProgress.gameObject.SetActive(false);
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(deleteButton.gameObject);
    }

    public void DeleteFile()
    {
        StartCoroutine(CrashGame());
    }

    private IEnumerator CrashGame()
    {
        deletingProgress.gameObject.SetActive(true);

        yield return new WaitForSeconds(5);
        
        Application.Quit();
        Debug.Log("Quitting Game");
    }

}
