using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameMenu : MonoBehaviour
{
    public GameObject theMenu;
    public GameObject[] windows; // array of available menu windows

    public GameObject[] statsButtons; // buttons for status window

    private CharStats[] playerStats; // stores player stats

    public Text[] nameText; // stores all player names
    public Text[] hpText; // stores all player hp values
    public Text[] mpText; // stores mp for each player
    public Text[] lvlText; // stores level for each player
    public Text[] expText; // stores experience for each player
    public Slider[] expSlider; // stores position of exp slider for each character
    public Image[] charImage; // stores character images
    public GameObject[] charStatHolder; // holds player object

    // text variables to update stats screen
    public Text statName, statHP, statMP, statStr, statDef, statWpnEqpd, statWpnPwr, statArmEqpd, statArmrPwr, statExp;
    public Image statImage;

    [Header("Item Button Variables")]
    public ItemButton[] itemButtons;
    public string selectedItem;
    public Item activeItem;
    public Text itemName;
    public Text itemDescription;
    public Text useButtonText;

    public GameObject itemCharChoiceMenu;
    public Text[] itemCharChoiceNames;

    public static GameMenu instance;

    public Text goldText; // updates text for amount of gold

    [Header("Additional Variables")]
    public bool canOpen = true; // SP: added to control whether menu may open or not, specifically to keep it from opening when shop menu is open
    public string mainMenuName;

    // Start is called before the first frame update
    void Start()
    {
        instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Fire2") && canOpen) // SP: added canOpen to avoid menu opening when user is in a shop menu 
        {
            if (theMenu.activeInHierarchy)
            {
                // theMenu.SetActive(false);
                // GameManager.instance.gameMenuOpen = false;

                // close menu 
                CloseMenu();
            }
            else
            {
                theMenu.SetActive(true);
                UpdateMainStats();
                GameManager.instance.gameMenuOpen = true;
            }
        }
    }

    public void UpdateMainStats()
    {
        playerStats = GameManager.instance.playerStats;

        for (int i = 0; i < playerStats.Length; i++)
        {
            if (playerStats[i].gameObject.activeInHierarchy)
            {
                charStatHolder[i].SetActive(true);

                // fill in player stats
                nameText[i].text = playerStats[i].charName;
                hpText[i].text = "HP: " + playerStats[i].currentHP + "/" + playerStats[i].maxHP;
                mpText[i].text = "MP: " + playerStats[i].currentMP + "/" + playerStats[i].maxMP;
                lvlText[i].text = "Lvl: " + playerStats[i].playerLevel;
                expText[i].text = "" + playerStats[i].currentEXP + "/" + playerStats[i].expToNextLevel[playerStats[i].playerLevel];
                expSlider[i].maxValue = playerStats[i].expToNextLevel[playerStats[i].playerLevel];
                expSlider[i].value = playerStats[i].currentEXP;
                charImage[i].sprite = playerStats[i].charImage;
            }
            else
            {
                charStatHolder[i].SetActive(false);
            }
        }

        // update gold amount text
        goldText.text = GameManager.instance.currentGold.ToString() + "g";
    }

    public void ToggleWindow(int windowNumber)
    {
        // update stats on opening window
        UpdateMainStats();

        // figure out which window should be opened and open window
        for (int i = 0; i < windows.Length; i++)
        {
            if (i == windowNumber)
            {
                Debug.Log(windowNumber);
                windows[i].SetActive(!windows[i].activeInHierarchy);
            } else
            {
                windows[i].SetActive(false);
            }
        }

        // close character choice window associated with "use" button
        itemCharChoiceMenu.SetActive(false);
    }


    public void CloseMenu()
    {
        // ensure none of the secondary menu windows are open (like items window)
        for (int i = 0; i < windows.Length; i++)
        {
            windows[i].SetActive(false);
        }

        // close main menu
        theMenu.SetActive(false);

        GameManager.instance.gameMenuOpen = false;

        // close character choice window associated with "use" button
        itemCharChoiceMenu.SetActive(false);
    }

    public void OpenStats()
    {
        // update stats on opening window
        UpdateMainStats();
        // update the information that is shown to first character when opening stats page
        StatusChar(0);
        // loop through buttons to see which has been pressed
        for (int i = 0; i < statsButtons.Length; i++)
        {
            // turn off inactive windows
            statsButtons[i].SetActive(playerStats[i].gameObject.activeInHierarchy);
            statsButtons[i].GetComponentInChildren<Text>().text = playerStats[i].charName;
            Debug.Log("Show Character names on Button");
        }
    }

    // Fill in stat window with character stats
    public void StatusChar(int selected)
    {
        statName.text = playerStats[selected].charName;
        statHP.text = "" + playerStats[selected].currentHP + "/" + playerStats[selected].maxHP;
        statMP.text = "" + playerStats[selected].currentMP + "/" + playerStats[selected].maxMP;
        statStr.text = playerStats[selected].strength.ToString();
        statDef.text = playerStats[selected].defense.ToString();

        // only show equipped weapon if one exists - otherwise leave "none" placeholder
        if (playerStats[selected].equippedWpn != "")
        {
            statWpnEqpd.text = playerStats[selected].equippedWpn;
        }
        statWpnPwr.text = playerStats[selected].wpnPwr.ToString(); // show weapon power even if there is no weapon so power is "0"

        // only show equipped armour if one exists - otherwise leave "none" placeholder
        if (playerStats[selected].equippedArmr != "")
        {
            statArmEqpd.text = playerStats[selected].equippedArmr;// show weapon power even if there is no armour so power is "0"
        }
        statArmrPwr.text = playerStats[selected].armrPwr.ToString();
        statExp.text = (playerStats[selected].expToNextLevel[playerStats[selected].playerLevel] - playerStats[selected].currentEXP).ToString();
        statImage.sprite = playerStats[selected].charImage;
    }

    // shows items in inventory menu
    public void ShowItems()
    {
        // sort items in item menu to show existing items before empty slots
        GameManager.instance.SortItems();

        for (int i = 0; i < itemButtons.Length; i++)
        {
            itemButtons[i].buttonValue = i;

            if (GameManager.instance.itemsHeld[i] != "")
            {
                itemButtons[i].buttonImage.gameObject.SetActive(true);
                itemButtons[i].buttonImage.sprite = GameManager.instance.GetItemDetails(GameManager.instance.itemsHeld[i]).itemSprite;
                itemButtons[i].amountText.text = GameManager.instance.numberOfItems[i].ToString();
            }
            else
            {
                itemButtons[i].buttonImage.gameObject.SetActive(false);
                itemButtons[i].amountText.text = "";
            }
        }
    }
    public void SelectItem(Item newItem)
    {
        activeItem = newItem;

        // check type of item
        if (activeItem.isItem)
        {
            useButtonText.text = "Use";
        }

        if (activeItem.isWeapon || activeItem.isArmour)
        {
            useButtonText.text = "Equip";
        }

        itemName.text = activeItem.itemName;
        itemDescription.text = activeItem.description;
    }

    // use remove item class in GameManager to discard items with discard button
    public void DiscardItem()
    {
        if(activeItem != null)
        {
            GameManager.instance.RemoveItem(activeItem.itemName);
        }
    }

    // when using an item (with use button), bring up menu to choose which character to use it on
    public void OpenItemCharChoice()
    {
        itemCharChoiceMenu.SetActive(true);

        for(int i = 0; i < itemCharChoiceNames.Length; i++)
        {
            itemCharChoiceNames[i].text = GameManager.instance.playerStats[i].charName;
            itemCharChoiceNames[i].transform.parent.gameObject.SetActive(GameManager.instance.playerStats[i].gameObject.activeInHierarchy);
        }
    }

    // close choosing a character menu
    public void CloseItemCharChoice()
    {
        itemCharChoiceMenu.SetActive(false);
    }

    public void UseItem(int selectChar)
    {
        activeItem.Use(selectChar);
        CloseItemCharChoice();
    }

    // Save game by calling save functions in GameManager and QuestManager
    public void SaveGame()
    {
        GameManager.instance.SaveData();
        QuestManager.instance.SaveQuestData();
    }

    public void QuitGame()
    {
        SceneManager.LoadScene(mainMenuName);

        Destroy(GameManager.instance.gameObject);
        Destroy(PlayerController.instance.gameObject);
        Destroy(AudioManager.instance.gameObject);
        Destroy(gameObject);
    }
}
