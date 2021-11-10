using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Shop : MonoBehaviour
{
    public static Shop instance;

    public GameObject shopMenu;
    public GameObject buyMenu;
    public GameObject sellMenu;

    public Text goldText;

    public string[] itemsForSale;

    public ItemButton[] buyItemButtons;
    public ItemButton[] sellItemButtons;

    public Item selectedItem;
    public Text buyItemName;
    public Text buyItemDescription;
    public Text buyItemValue;

    public Text sellItemName;
    public Text sellItemDescription;
    public Text sellItemValue;
    public int sellItemQty; // SP: added to try and resolve selling issues

    // Start is called before the first frame update
    void Start()
    {
        instance = this;    
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.K) && !shopMenu.activeInHierarchy)
        {
            OpenShop();
        }
    }

    public void OpenShop()
    {
        shopMenu.SetActive(true);
        OpenBuyMenu();

        GameManager.instance.shopActive = true;

        goldText.text = GameManager.instance.currentGold.ToString() + "g";
    }

    public void CloseShop()
    {
        shopMenu.SetActive(false);
        GameManager.instance.shopActive = false;
    }

    public void OpenBuyMenu()
    {
        // Set first buy item as selected when opening menu
        buyItemButtons[0].Press();

        // open buy and close sell
        buyMenu.SetActive(true);
        sellMenu.SetActive(false);

        // loop through shop buttons and display shopkeeper items and amounts
        for (int i = 0; i < buyItemButtons.Length; i++)
        {
            buyItemButtons[i].buttonValue = i;

            if (itemsForSale[i] != "")
            {
                buyItemButtons[i].buttonImage.gameObject.SetActive(true);
                buyItemButtons[i].buttonImage.sprite = GameManager.instance.GetItemDetails(itemsForSale[i]).itemSprite;
                buyItemButtons[i].amountText.text = ""; // shop can have unlimited items to buy
            }
            else
            {
                buyItemButtons[i].buttonImage.gameObject.SetActive(false);
                buyItemButtons[i].amountText.text = "";
            }
        }
    }

    public void OpenSellMenu()
    {
        // Set first sell item as selected when opening menu
        buyItemButtons[0].Press();

        // open sell and close buy
        sellMenu.SetActive(true);
        buyMenu.SetActive(false);

        ShowSellItems();
    }

    private void ShowSellItems()
    {
        // should already be sorted but just to be certain
        GameManager.instance.SortItems();

        // show player inventory in sell menu
        for (int i = 0; i < sellItemButtons.Length; i++)
        {
            sellItemButtons[i].buttonValue = i;

            if (GameManager.instance.itemsHeld[i] != "")
            {
                sellItemButtons[i].buttonImage.gameObject.SetActive(true);
                sellItemButtons[i].buttonImage.sprite = GameManager.instance.GetItemDetails(GameManager.instance.itemsHeld[i]).itemSprite; // show player items
                sellItemButtons[i].amountText.text = GameManager.instance.numberOfItems[i].ToString(); // show players item quantities
            }
            else
            {
                sellItemButtons[i].buttonImage.gameObject.SetActive(false);
                sellItemButtons[i].amountText.text = "";
            }
        }
    }

    public void SelectBuyItem(Item buyItem)
    {
        if (buyItem != null)
        {
            selectedItem = buyItem;
            buyItemName.text = selectedItem.itemName;
            buyItemDescription.text = selectedItem.description;
            buyItemValue.text = "Value: " + selectedItem.value + "g";
        }
    }

    public void SelectSellItem(Item sellItem)
    {
        if (sellItem != null)
        {
            selectedItem = sellItem;
            sellItemName.text = selectedItem.itemName;
            sellItemDescription.text = selectedItem.description;
            sellItemValue.text = "Value: " + Mathf.FloorToInt(selectedItem.value * .5f).ToString() + "g";
        }
    }

    // when buy is pressed
    public void BuyItem()
    {
        if (selectedItem != null)
        {
            // if player has enough gold to purchase selected item
            if (GameManager.instance.currentGold >= selectedItem.value)
            {
                GameManager.instance.currentGold -= selectedItem.value; // remove gold from players gold
                GameManager.instance.AddItem(selectedItem.itemName); // add selected item to inventory 
            }
        }
        // update players gold 
        goldText.text = GameManager.instance.currentGold.ToString() + "g";

        // sort player inventory items
        GameManager.instance.SortItems();
    }

    // when sell is pressed
    public void SellItem()
    {
        if (selectedItem != null)
        {
            GameManager.instance.RemoveItem(selectedItem.itemName); // remove item from user inventory
            GameManager.instance.currentGold += Mathf.FloorToInt(selectedItem.value * .5f); // add gold value to users gold

            // *** SP: added code portion to fix issue with selectedItem never being reset and unlimited gold (Lecture 69/70)
            List<string> tempList = new List<string>(GameManager.instance.itemsHeld);
            if(!tempList.Contains(selectedItem.itemName))
            {
                selectedItem = null;
                sellItemName.text = "";
                sellItemDescription.text = "";
                sellItemValue.text = "";
            }
        }
        // update players gold 
        goldText.text = GameManager.instance.currentGold.ToString() + "g";

        ShowSellItems();
    }
}
