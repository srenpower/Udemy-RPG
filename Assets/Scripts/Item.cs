using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    [Header("Item Type")]
    public bool isItem; // basic item
    public bool isWeapon;
    public bool isArmour;
    public bool isGold;

    [Header("Item Details")]
    public string itemName;
    public string description;
    public int value; // cost of item
    public Sprite itemSprite; // image of item

    [Header("Item Affects")]
    public int amountToChange; // how much does it alter a stat - hp, mp, health, str, def
    public bool affectHP, affectMP, affectStr; // does the object affect a stat

    [Header("Weapon/Armour Details")]
    public int weaponStrength;
    public int armourStrength;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void UseBattle(int charToUseOn)
    {
        BattleChar selectedChar = BattleManager.instance.activeBattlers[charToUseOn];
        CharStats inGameChar = GameManager.instance.playerStats[charToUseOn];

        if (isItem)
        {
            if (affectHP)
            {
                selectedChar.currentHP += amountToChange;

                if (selectedChar.currentHP > selectedChar.maxHP)
                {
                    selectedChar.currentHP = selectedChar.maxHP;
                }
            }

            if (affectMP)
            {
                selectedChar.currentMP += amountToChange;

                if (selectedChar.currentMP > selectedChar.maxMP)
                {
                    selectedChar.currentMP = selectedChar.maxMP;
                }
            }

            if (affectStr)
            {
                selectedChar.strength += amountToChange;
            }
        }

        // todo: figure out battle time item swapping
        if (isWeapon)
        {
            if (selectedChar.equippedWpn != "")
            {
                //GameManager.instance.AddItem(selectedChar.equippedWpn); // add current item equipped back into battle inventory
                GameManager.instance.AddItem(inGameChar.equippedWpn); // add current item equppied back into regular inventory
            }
            // change battle items
            selectedChar.equippedWpn = itemName;
            selectedChar.wpnPwr = weaponStrength;
            // change in-game items
            inGameChar.equippedWpn = itemName;
            inGameChar.wpnPwr = weaponStrength;
        }

        if (isArmour)
        {
            if (selectedChar.equippedArmr != "")
            {
               //GameManager.instance.AddItem(selectedChar.equippedArmr); // add current item equipped back into battle inventory
                GameManager.instance.AddItem(inGameChar.equippedArmr); // add current item equppied back into regular inventory
            }
            // change battle items
            selectedChar.equippedArmr = itemName;
            // change in-game items
            selectedChar.armrPwr = armourStrength;

            inGameChar.equippedWpn = itemName;
            inGameChar.wpnPwr = armourStrength;
        }
        GameManager.instance.RemoveItem(itemName);
        //BattleItems.instance.ShowItems();
    }


    public void Use(int charToUseOn)
    { 
        CharStats selectedChar = GameManager.instance.playerStats[charToUseOn];

        if (isItem)
        {
            if (affectHP)
            {
                selectedChar.currentHP += amountToChange;

                if (selectedChar.currentHP > selectedChar.maxHP)
                {
                    selectedChar.currentHP = selectedChar.maxHP;
                }
            }

            if (affectMP)
            {
                selectedChar.currentMP += amountToChange;

                if (selectedChar.currentMP > selectedChar.maxMP)
                {
                    selectedChar.currentMP = selectedChar.maxMP;
                }
            }

            if (affectStr)
            {
                selectedChar.strength += amountToChange;
            }
        }

        if (isWeapon)
        {
            if (selectedChar.equippedWpn != "")
            {
                GameManager.instance.AddItem(selectedChar.equippedWpn);
            }

            selectedChar.equippedWpn = itemName;
            selectedChar.wpnPwr = weaponStrength;
        }

        if (isArmour)
        {
            if (selectedChar.equippedArmr != "")
            {
                GameManager.instance.AddItem(selectedChar.equippedArmr);
            }

            selectedChar.equippedArmr = itemName;
            selectedChar.armrPwr = armourStrength;
        }

        GameManager.instance.RemoveItem(itemName);
    }
}   
