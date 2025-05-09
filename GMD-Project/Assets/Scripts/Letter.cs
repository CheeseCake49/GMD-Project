using UnityEngine;

public class Letter : MonoBehaviour
{
    public GameObject letterUI;

    bool toggle;

    public Renderer letterMesh;
    public PlayerController player;


    public void openCloseLetter()
    {
        //Toggle will equal to the opposite of what it currently equals to.
        toggle = !toggle;

        //If toggle equals false, that means the player is putting down the letter.
        if(toggle == false)
        {
            letterUI.SetActive(false);
            letterMesh.enabled = true;
            player.enabled = true;
        }

        //If toggle equals true, that means the player is picking up the letter.
        if (toggle == true)
        {
            letterUI.SetActive(true);
            letterMesh.enabled = false;
            player.enabled = false;
        }
    }
}
