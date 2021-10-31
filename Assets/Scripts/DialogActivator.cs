using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogActivator : MonoBehaviour
{
    public string[] lines; // store lines for dialog

    public bool isPerson = true; // set default that dialog objects are people  

    private bool canActivate; // determines if character for dialog can be activated

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(canActivate && Input.GetButtonDown("Fire1") && !DialogManager.instance.dialogBox.activeInHierarchy)
        {
            DialogManager.instance.ShowDialog(lines, isPerson);
        }
    }
    
    // normally (Collider2D collision) we change to other because "collision" is just a name and makes it confusing because Collision is an object in unity and leaves room for error
    private void OnTriggerEnter2D(Collider2D other)
    {
        // check if collided with by player
        if(other.tag == "Player")
        {
            canActivate = true;
        }
    }
    // when player leaves collider do not activate
    private void OnTriggerExit2D(Collider2D other)
    {
        // check if collided with by player
        if (other.tag == "Player")
        {
            canActivate = false;
        }
    }
}
