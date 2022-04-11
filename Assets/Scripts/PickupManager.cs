using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupManager : MonoBehaviour
{
    public string[] pickupItemNames;
    public bool[] itemsPickedUp;

    public static PickupManager instance;

    // Start is called before the first frame update
    void Start()
    {
        instance = this;

        itemsPickedUp = new bool[pickupItemNames.Length];
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public int GetItemNumber(string itemToFind)
    {
        for(int i = 0; i < pickupItemNames.Length; i++)
        {
            if(pickupItemNames[i] == itemToFind)
            {
                return i;
            }
        }

        // if pickup item name not found - throw error - 0 gets returned
        Debug.LogError("Pickup item " + itemToFind + " does not exist");

        // item name 0 is always a blank item for this reason - considered invalid
        return 0;
    }

    // check if item has been picked up - return true or false
    public bool CheckIfPickedUp(string itemToCheck)
    {
        if(GetItemNumber(itemToCheck) != 0)
        {
            return itemsPickedUp[GetItemNumber(itemToCheck)];
        }

        return false;
    }

    // set pickup item as picked up or not (true/false)
    public void PickupItemComplete(string itemToMark)
    {
        itemsPickedUp[GetItemNumber(itemToMark)] = true;

        UpdateLocalPickupItems();
    }

    // allows an item to be incomplete (could be used to make a disabled item appear on cue)
    public void PickupItemIncomplete(string itemToMark)
    {
        itemsPickedUp[GetItemNumber(itemToMark)] = false;

        UpdateLocalPickupItems();
    }
   
    // update local pickup items when moving between scenes
    public void UpdateLocalPickupItems()
    {
        PickupItemActivator[] pickupItems = FindObjectsOfType<PickupItemActivator>();

        if(pickupItems.Length > 0)
        {
            for(int i = 0; i < pickupItems.Length; i++)
            {
                pickupItems[i].CheckCompletion();
            }
        }
    }
}
