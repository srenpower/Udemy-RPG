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
       // battleItemMenu.name = battleItemMenu.name + UnityEngine.Random.RandomRange(0.0f, 100000.0f).ToString();
        instance = this;   
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // open battle item menu
    public static void OpenMenu()
    {
        Debug.Log("Open Battle Items Menu " + instance.battleItemMenu.name);
        instance.battleItemMenu.gameObject.SetActive(true);
        instance.battleItemMenu.SetActive(true);
        ShowItems();
    }

    public static void CloseMenu()
    {
        Debug.Log("Close Battle Items Menu" + instance.battleItemMenu.name);
        instance.battleItemMenu.SetActive(false);
    }

    public static void ShowItems()
    {
        // should already be sorted but just to be certain
        GameManager.instance.SortItems();

        // show player inventory in sell menu
        for (int i = 0; i < instance.itemButtons.Length; i++)
        {
            instance.itemButtons[i].buttonValue = i;

            if (GameManager.instance.itemsHeld[i] != "")
            {
                instance.itemButtons[i].buttonImage.gameObject.SetActive(true);
                instance.itemButtons[i].buttonImage.sprite = GameManager.instance.GetItemDetails(GameManager.instance.itemsHeld[i]).itemSprite; // show player items
                instance.itemButtons[i].amountText.text = GameManager.instance.numberOfItems[i].ToString(); // show players item quantities
            }
            else
            {
                instance.itemButtons[i].buttonImage.gameObject.SetActive(false);
                instance.itemButtons[i].amountText.text = "";
            }
        }

        if(GameManager.instance.itemsHeld.Length > 0 && GameManager.instance.itemsHeld[0] != "")
        {
            instance.selectedItem = GameManager.instance.GetItemDetails(GameManager.instance.itemsHeld[0]);
            SelectItem(instance.selectedItem);
        }
    }

    public static void SelectItem(Item battleItem)
    {
        if (instance.selectedItem != null)
        {
            instance.selectedItem = battleItem;
            instance.itemName.text = instance.selectedItem.itemName;
            instance.itemDescription.text = instance.selectedItem.description;
        }
    }

    public static void UseItem(int selectChar)
    {
        if (instance.selectedItem != null)
        {
            // usebattle in battle to update the correct instance of the player
            instance.selectedItem.UseBattle(selectChar);
            GameMenu.instance.CloseItemCharChoice();

            BattleManager.instance.UpdateUIStats();
            GameManager.instance.SortItems();
        }
    }
}
