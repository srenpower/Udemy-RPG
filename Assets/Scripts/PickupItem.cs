using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupItem : MonoBehaviour
{

    private bool canPickup;
    public bool isGold;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(canPickup && Input.GetButtonDown("Fire1") && PlayerController.instance.canMove)
        {
            if(isGold)
            {
                GameManager.instance.currentGold += GetComponent<Item>().value;
            }
            GameManager.instance.AddItem(GetComponent<Item>().itemName);
            Destroy(gameObject); 
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "Player")
        {
            canPickup = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            canPickup = false;
        }
    }
}
