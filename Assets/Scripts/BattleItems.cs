using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BattleItems : MonoBehaviour
{
    public ItemButton[] itemButtons;

    public Item selectedItem;

    public Text itemName;
    public Text itemDescription;

    public static BattleItems instance;

    public GameObject battleItemMenu;
    // Start is called before the first frame update
    void Start()
    {
        instance = this;   
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // open battle item menu
    public void OpenMenu()
    {
        Debug.Log("Open Battle Items Menu");
        battleItemMenu.SetActive(true);

        ShowItems();
    }

    public void CloseMenu()
    {
        battleItemMenu.SetActive(false);
    }

    public void ShowItems()
    {
        // should already be sorted but just to be certain
        GameManager.instance.SortItems();

        // show player inventory in sell menu
        for (int i = 0; i < itemButtons.Length; i++)
        {
            itemButtons[i].buttonValue = i;

            if (GameManager.instance.itemsHeld[i] != "")
            {
                itemButtons[i].buttonImage.gameObject.SetActive(true);
                itemButtons[i].buttonImage.sprite = GameManager.instance.GetItemDetails(GameManager.instance.itemsHeld[i]).itemSprite; // show player items
                itemButtons[i].amountText.text = GameManager.instance.numberOfItems[i].ToString(); // show players item quantities
            }
            else
            {
                itemButtons[i].buttonImage.gameObject.SetActive(false);
                itemButtons[i].amountText.text = "";
            }
        }

        if(GameManager.instance.itemsHeld.Length > 0 && GameManager.instance.itemsHeld[0] != "")
        {
            selectedItem = GameManager.instance.GetItemDetails(GameManager.instance.itemsHeld[0]);
            SelectItem(selectedItem);
        }
    }

    public void SelectItem(Item battleItem)
    {
        if (selectedItem != null)
        {
            selectedItem = battleItem;
            itemName.text = selectedItem.itemName;
            itemDescription.text = selectedItem.description;
        }
    }

    public void UseItem(int selectChar)
    {
        if (selectedItem != null)
        {
            // usebattle in battle to update the correct instance of the player
            selectedItem.UseBattle(selectChar);
            GameMenu.instance.CloseItemCharChoice();

            BattleManager.instance.UpdateUIStats();
            GameManager.instance.SortItems();
        }
    }
}
