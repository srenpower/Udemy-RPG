using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public CharStats[] playerStats; // holds player stats

    public bool gameMenuOpen;
    public bool dialogActive;
    public bool fadingBetweenAreas;
    public bool shopActive;

    [Header("Inventory Variables")]
    public string[] itemsHeld; // what items are in inventory
    public int[] numberOfItems; // how many items in inventory
    public Item[] referenceItems; // how to get info about item from inventory

    public int currentGold;

    // Start is called before the first frame update
    void Start()
    {
        instance = this;

        DontDestroyOnLoad(gameObject);

        SortItems();
    }

    // Update is called once per frame
    void Update()
    {
        // check on update if game menu or dialog box is open, or if we are switching between scenes
        if(gameMenuOpen || dialogActive || fadingBetweenAreas || shopActive)
        {
            // if true stop player movement
            PlayerController.instance.canMove = false;
        }
        else
        {
            // none of these things are true so player can move
            PlayerController.instance.canMove = true;
        }

        // GOOD FOR TESTING
        if(Input.GetKeyDown(KeyCode.J))
        {

        }
    }

    public Item GetItemDetails(string itemToGrab)
    {
        for(int i = 0; i < referenceItems.Length; i++)
        {
            if(referenceItems[i].itemName == itemToGrab)
            {
                return referenceItems[i];
            }
        }
        return null; // if there is no item returned
    }

    // sort item slots by active and inactive
    public void SortItems()
    {
        bool itemAfterSpace = true; // set initially to true to enter while loop

        while (itemAfterSpace)
        {
            itemAfterSpace = false; // set item after space to false after once in while loop
            // loop through item slots in array
            for (int i = 0; i < itemsHeld.Length - 1; i++)
            {
                // if there is no item in this position move the next item into this position
                if (itemsHeld[i] == "")
                {
                    // move next item in array to this position
                    itemsHeld[i] = itemsHeld[i + 1];
                    itemsHeld[i + 1] = "";

                    // move quantity of next item in array to this position
                    numberOfItems[i] = numberOfItems[i + 1];
                    numberOfItems[i + 1] = 0;

                    // if position now has item set item after space back to true
                    if(itemsHeld[i] != "")
                    {
                        itemAfterSpace = true;
                    }
                }
            }
        }
    }
    
    public void AddItem(string itemToAdd)
    {
        int newItemPosition = 0;
        bool foundSpace = false;

        for(int i = 0; i < itemsHeld.Length; i++)
        {
            // check if itemToAdd is an existing item in the inventory, _or_ if there is a blank space to hold it
            if (itemsHeld[i] == "" || itemsHeld[i] == itemToAdd)
            {
                newItemPosition = i; // store position of existing item or blank space
                i = itemsHeld.Length; // set i to itemsHeld length to stop looping
                foundSpace = true; // set foundSpace to true
            }
        }

        // either item already exists in inventory or a blank is available to add item
        if(foundSpace)
        {
            bool itemExists = false; // reset to false

            // find out if item exists in inventory and set bool
            for(int i = 0; i < referenceItems.Length; i++)
            {
                if(referenceItems[i].itemName == itemToAdd)
                {
                    itemExists = true;

                    i = referenceItems.Length;
                }
            }
            // if item exists add it to the posisition and increment the number of items
            if(itemExists)
            {
                itemsHeld[newItemPosition] = itemToAdd;
                numberOfItems[newItemPosition]++;
            }
            else
            {
                Debug.LogError(itemToAdd + " Does Not Exist!!");
            }
        }

        GameMenu.instance.ShowItems();
    }

    public void RemoveItem(string itemToRemove)
    {
        bool foundItem = false;
        int itemPosition = 0;

        for(int i = 0; i < itemsHeld.Length; i++)
        {
            if(itemsHeld[i] == itemToRemove)
            {
                foundItem = true;
                itemPosition = i;

                i = itemsHeld.Length;
            }
        }

        // if item exists in inventory
        if(foundItem)
        {
            // decrement item
            numberOfItems[itemPosition]--;

            // check if decrementing the item quantity results in the item having a zero quantity
            if(numberOfItems[itemPosition] <= 0)
            {
                // if true remove item entirely from inventory
                itemsHeld[itemPosition] = "";
            }

            // re-sort items in case an item has been fully removed
            GameMenu.instance.ShowItems();
        }
        else
        {
            Debug.LogError("Couldn't find " + itemToRemove);
        }
    }
}
