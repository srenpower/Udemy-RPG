using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NotificationActivator : MonoBehaviour
{
    public string message;
    public bool isTrigger;
    public bool buttonOn;
    private bool inZone = false;
   // public GameNotification gameNotice;
    

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!isTrigger && inZone)
        {
            if (Input.GetButtonDown("Fire1"))
            {
                GameMenu.instance.gameNotification.Activate(buttonOn);
                GameMenu.instance.gameNotification.theText.text = message;
            }
        }
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            inZone = true;
            if (isTrigger)
            {
                GameMenu.instance.gameNotification.Activate(buttonOn);
                GameMenu.instance.gameNotification.theText.text = message;
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
