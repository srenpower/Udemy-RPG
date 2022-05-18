using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NotificationActivator : MonoBehaviour
{
    public string message;
    public bool isTrigger;
    public bool deactivateAfterUse; // message only shown once
    public bool buttonOn;
    private bool inZone = false;
    private bool doNotActivate = false; // keeps notification from activating repeatedly
   // public GameNotification gameNotice;
    

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!isTrigger && inZone && !doNotActivate)
        {
            if (Input.GetButtonDown("Fire1"))
            {
                GameMenu.instance.gameNotification.Activate(buttonOn);
                GameMenu.instance.gameNotification.theText.text = message;

                if(deactivateAfterUse)
                {
                    doNotActivate = true;
                }
            }
        }
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player" && !doNotActivate)
        {
            inZone = true;
            if (isTrigger)
            {
                GameMenu.instance.gameNotification.Activate(buttonOn);
                GameMenu.instance.gameNotification.theText.text = message;
                if (deactivateAfterUse)
                {
                    doNotActivate = true;
                }
            }
        }
    }
    private void OnTriggerExit2D(Collider2D other)
    {
        if(other.tag == "Player")
        {
            inZone = false;
        }
    }
}
