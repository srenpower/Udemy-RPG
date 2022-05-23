using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreditActivator : MonoBehaviour
{
    private bool inZone = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (inZone)
        {
            if (Input.GetButtonDown("Fire1"))
            {
                GameMenu.instance.creditWindow.SetActive(inZone);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            inZone = true;
        }
    }
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            inZone = false;
        }
    }

    public void CloseWindow()
    {
        gameObject.SetActive(false);
    }
}
