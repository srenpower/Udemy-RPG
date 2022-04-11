using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupMarker : MonoBehaviour
{
    public string uniqueItemName;
    private bool canMark;
    public bool deactivateOnMarking;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(canMark && Input.GetButtonDown("Fire1"))
        {
            canMark = false;
            MarkItemPickedUp();
        }
    }

    public void MarkItemPickedUp()
    {
        PickupManager.instance.PickupItemComplete(uniqueItemName);
        gameObject.SetActive(!deactivateOnMarking); // set game object to inactive if deactivateOnMarking is true
    }

    // when entering item's collider
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
                canMark = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            canMark = false;
        }
    }
}
