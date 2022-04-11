using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupItemActivator : MonoBehaviour
{
    public GameObject itemToDeactivate;

    public string itemToCheck; // unique item name in pickup manager
    public bool deactivateOnPickup = true; // should item be deactivated permanently on pickup

    private bool itemPickedUp = false; // used to stop update checks if item was already picked up
    //private bool shouldDisableItem = false; // should item be permanently disabled in world state

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(!itemPickedUp)
        {
            CheckCompletion();
        }
    }

    public void CheckCompletion()
    {
        // Check if all or at least one of the required quests have been completed
        if (PickupManager.instance.CheckIfPickedUp(itemToCheck))
        {
            DisablePickedUpItem();
        }
    }

    public void DisablePickedUpItem()
    {
        itemToDeactivate.SetActive(!deactivateOnPickup); 
        itemPickedUp = true; // disables update check for this item
    }
}
