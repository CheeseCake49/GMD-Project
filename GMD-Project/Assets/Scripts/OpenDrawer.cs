using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenDrawer : MonoBehaviour, IInteractable
{
    public Animator ANI;


    private bool open;

    private bool inReach;


    void Start()
    {

        ANI.SetBool("open", false);
        ANI.SetBool("close", false);

        open = false;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Reach" && !open)
        {
            inReach = true;
        }

        else if (other.gameObject.tag == "Reach" && open)
        {
            inReach = true;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Reach")
        {
            inReach = false;
  
        }
    }



    public void ToggleDrawer()
    {
        if (!open)
        {
            ANI.SetBool("open", true);
            ANI.SetBool("close", false);
            open = true;
        }
        else
        {
            ANI.SetBool("open", false);
            ANI.SetBool("close", true);
            open = false;
        }

    }

        public void Interact()
    {
        ToggleDrawer();
    }
}