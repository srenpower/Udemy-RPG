using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public CharStats[] playerStats; // holds player stats

    public bool gameMenuOpen;
    public bool dialogActive;
    public bool fadingBetweenAreas;
    public bool shopActive;
    public bool battleActive;

    [Header("Inventory Variables")]
    public string[] itemsHeld; // what items are in inventory
    public int[] numberOfItems; // how many items in inventory
    public Item[] referenceItems; // how to get info about item from inventory
   
    public int currentGold;
    private string saveState;

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
        if(gameMenuOpen || dialogActive || fadingBetweenAreas || shopActive || battleActive)
        {
            // if true stop player movement
            PlayerController.instance.canMove = false;
        }
        else
        {
            // none of these things are true so player can move
            PlayerController.instance.canMove = true;
        }

        // SP: added to check if shop menu is open to stop game menu from opening in the background
        if(shopActive || battleActive)
        {
            GameMenu.instance.canOpen = false;
        }
        else
        {
            GameMenu.instance.canOpen = true;
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

                    i = referenceItems.Length; // goes to end of loop because item name exists in reference items
                }
            }

            // if item exists add it to the posisition and increment the number of items
            if(itemExists)
            {
                itemsHeld[newItemPosition] = itemToAdd;
                numberOfItems[newItemPosition]++;
            }
            else if(itemToAdd == "Gold Coin")
            {

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
                Debug.Log("Item should be fully removed");
            }

            // re-sort items in case an item has been fully removed
            if (GameMenu.instance.theMenu.activeInHierarchy)
            {
                GameMenu.instance.ShowItems();
            }
            else
            {
                BattleItems.instance.ShowItems();
            }
        }
        else
        {
            Debug.LogError("Couldn't find " + itemToRemove);
        }
    }

    public void SaveData(int saveNum)
    {
        if(saveNum == 1)
        {
            saveState = "Save_One";
        }
        else if(saveNum == 2)
        {
            saveState = "Save_Two";
        }
        else if(saveNum == 3)
        {
            saveState = "Save_Three";
        }
        else
        {
            saveState = "Persistent_Save";
            Debug.Log("Save persistent state");
        }
        PlayerPrefs.SetString(saveState, saveState);

        if (saveState != "Persistent_Save")
        {
            Debug.Log("Not used during persistent state");
            // store current scene state
            PlayerPrefs.SetString(saveState + "_Scene", SceneManager.GetActiveScene().name);
            // store player coordinates
            PlayerPrefs.SetFloat(saveState + "_" + "Player_Position_x", PlayerController.instance.transform.position.x);
            PlayerPrefs.SetFloat(saveState + "_" + "Player_Position_y", PlayerController.instance.transform.position.y);
        }

        // save character info
        for(int i = 0; i < playerStats.Length; i++)
        {
            if(playerStats[i].gameObject.activeInHierarchy)
            {
                // if player is active set tag as Player_playername_active and "1" for true
                PlayerPrefs.SetInt(saveState + "_" + "Player_" + playerStats[i].charName + "_active", 1);
            }
            else
            {
                PlayerPrefs.SetInt(saveState + "_" + "Player_" + playerStats[i].charName + "_active", 0);

            }

            // store other character info
            PlayerPrefs.SetInt(saveState + "_" + "Player_" + playerStats[i].charName + "_Level", playerStats[i].playerLevel);
            PlayerPrefs.SetInt(saveState + "_" + "Player_" + playerStats[i].charName + "_CurrentExp", playerStats[i].currentEXP);
            PlayerPrefs.SetInt(saveState + "_" + "Player_" + playerStats[i].charName + "_CurrentHP", playerStats[i].currentHP);
            PlayerPrefs.SetInt(saveState + "_" + "Player_" + playerStats[i].charName + "_MaxHP", playerStats[i].maxHP);
            PlayerPrefs.SetInt(saveState + "_" + "Player_" + playerStats[i].charName + "_CurrentMP", playerStats[i].currentMP);
            PlayerPrefs.SetInt(saveState + "_" + "Player_" + playerStats[i].charName + "_MaxMP", playerStats[i].maxMP);
            PlayerPrefs.SetInt(saveState + "_" + "Player_" + playerStats[i].charName + "_Strength", playerStats[i].strength);
            PlayerPrefs.SetInt(saveState + "_" + "Player_" + playerStats[i].charName + "_Defense", playerStats[i].defense);
            PlayerPrefs.SetInt(saveState + "_" + "Player_" + playerStats[i].charName + "_WpnPwr", playerStats[i].wpnPwr);
            PlayerPrefs.SetInt(saveState + "_" + "Player_" + playerStats[i].charName + "_ArmrPwr", playerStats[i].armrPwr);
            PlayerPrefs.SetString(saveState + "_" + "Player_" + playerStats[i].charName + "_EquippedWpn", playerStats[i].equippedWpn);
            PlayerPrefs.SetString(saveState + "_" + "Player_" + playerStats[i].charName + "_EquippedArmr", playerStats[i].equippedArmr);
        }

        // store inventory data
        for(int i = 0; i < itemsHeld.Length; i++)
        {
            PlayerPrefs.SetString(saveState + "_" + "ItemInInventory_" + i, itemsHeld[i]);
            PlayerPrefs.SetInt(saveState + "_" + "ItemAmount_" + i, numberOfItems[i]);
        }

        // Store quest data
        for(int i = 0; i < QuestManager.instance.questMarkerNames.Length; i++)
        {
            //PlayerPrefs.SetString(saveState + "_" + "QuestName_" + i, QuestManager.instance.questMarkerNames[i]);
            if (QuestManager.instance.questMarkersComplete[i])
            {
                PlayerPrefs.SetInt(saveState + "_" + "QuestCompletion_" + i, 1);
            }
            else
            {
                PlayerPrefs.SetInt(saveState + "_" + "QuestCompletion_" + i, 0);
            }
        }

        // Store pickup item data
        for (int i = 0; i < PickupManager.instance.pickupItemNames.Length; i++)
        {
            if (PickupManager.instance.itemsPickedUp[i])
            {
                PlayerPrefs.SetInt(saveState + "_" + "PickupItems_" + i, 1);
            }
            else
            {
                PlayerPrefs.SetInt(saveState + "_" + "PickupItems_" + i, 0);
            }
        }
    }

    public void LoadData(string saveName)
    {
        saveState = saveName;
        Debug.Log("Save State = " + saveState);
        if (PlayerPrefs.GetString(saveState) != "Persistent_Save")
        {
            Debug.Log("Set Player Position");
            Debug.Log("x-position: " + PlayerPrefs.GetFloat(saveState + "_" + "Player_Position_x") + ", y-position: " + PlayerPrefs.GetFloat(saveState + "_" + "Player_Position_y"));
            // set player scene
            //Scene sceneName = (PlayerPrefs.GetString(saveState + "_Scene");
            //SceneManager.SetActiveScene(PlayerPrefs.GetString(saveState + "_Scene");
            // set player position in scene based on loaded game state
            PlayerController.instance.transform.position = new Vector3(PlayerPrefs.GetFloat(saveState + "_" + "Player_Position_x"), PlayerPrefs.GetFloat(saveState + "_" + "Player_Position_y"));
        }
        Debug.Log("Persistent x-position: " + PlayerPrefs.GetFloat(saveState + "_" + "Player_Position_x") + ", y-position: " + PlayerPrefs.GetFloat(saveState + "_" + "Player_Position_y"));
        for (int i = 0; i < playerStats.Length; i++)
        {
            if(PlayerPrefs.GetInt(saveState + "_" + "Player_" + playerStats[i].charName + "_active") == 0)
            {
                playerStats[i].gameObject.SetActive(false);
            }
            else
            {
                playerStats[i].gameObject.SetActive(true);
            }

            // set other character info
            playerStats[i].playerLevel = PlayerPrefs.GetInt(saveState + "_" + "Player_" + playerStats[i].charName + "_Level");
            playerStats[i].currentEXP = PlayerPrefs.GetInt(saveState + "_" + "Player_" + playerStats[i].charName + "_CurrentExp");
            playerStats[i].currentHP = PlayerPrefs.GetInt(saveState + "_" + "Player_" + playerStats[i].charName + "_CurrentHP");
            playerStats[i].maxHP = PlayerPrefs.GetInt(saveState + "_" + "Player_" + playerStats[i].charName + "_MaxHP");
            playerStats[i].currentMP = PlayerPrefs.GetInt(saveState + "_" + "Player_" + playerStats[i].charName + "_CurrentMP");
            playerStats[i].maxMP = PlayerPrefs.GetInt(saveState + "_" + "Player_" + playerStats[i].charName + "_MaxMP");
            playerStats[i].strength = PlayerPrefs.GetInt(saveState + "_" + "Player_" + playerStats[i].charName + "_Strength");
            playerStats[i].defense = PlayerPrefs.GetInt(saveState + "_" + "Player_" + playerStats[i].charName + "_Defense");
            playerStats[i].wpnPwr = PlayerPrefs.GetInt(saveState + "_" + "Player_" + playerStats[i].charName + "_WpnPwr");
            playerStats[i].armrPwr = PlayerPrefs.GetInt(saveState + "_" + "Player_" + playerStats[i].charName + "_ArmrPwr");
            playerStats[i].equippedWpn = PlayerPrefs.GetString(saveState + "_" + "Player_" + playerStats[i].charName + "_EquippedWpn");
            playerStats[i].equippedArmr = PlayerPrefs.GetString(saveState + "_" + "Player_" + playerStats[i].charName + "_EquippedArmr");
        }

        // set saved items
        for(int i = 0; i < itemsHeld.Length; i++)
        {
            itemsHeld[i] = PlayerPrefs.GetString(saveState + "_" + "ItemInInventory_" + i);
            numberOfItems[i] = PlayerPrefs.GetInt(saveState + "_" + "ItemAmount_" + i);
        }

        // set quest data
        for(int i = 0; i < QuestManager.instance.questMarkerNames.Length; i++)
        {
            //PlayerPrefs.SetString(saveState + "_" + "QuestName_" + i, QuestManager.instance.questMarkerNames[i]);
            if (PlayerPrefs.GetInt(saveState + "_" + "QuestCompletion_" + i) == 1)
            {
                QuestManager.instance.questMarkersComplete[i] = true;
                Debug.Log("Quest " + i + " Complete");
            }
            else
            {
                QuestManager.instance.questMarkersComplete[i] = false;
            }
        }

        // set pickup item data
        for (int i = 0; i < PickupManager.instance.pickupItemNames.Length; i++)
        {
            if (PlayerPrefs.GetInt(saveState + "_" + "PickupItems_" + i) == 1)
            {
                PickupManager.instance.itemsPickedUp[i] = true;
            }
            else
            {
                PickupManager.instance.itemsPickedUp[i] = false;
            }
        }
    }
}
