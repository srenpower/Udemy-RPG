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
    public bool affectHP, affectMP, affectStr, affectLife; // does the object affect a stat

    [Header("Weapon/Armour Details")]
    public int weaponStrength;
    public int armourStrength;
    public int magicDefense;

    private string noItemEquipped = "None Equipped";

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
                if (!selectedChar.hasDied)
                {
                    selectedChar.currentHP += amountToChange;

                    if (selectedChar.currentHP > selectedChar.maxHP)
                    {
                        selectedChar.currentHP = selectedChar.maxHP;
                    }

                    // have to update out of battle character HP because that's where hp is tracked when performing magic attack
                    inGameChar.currentHP += amountToChange;

                    if (inGameChar.currentHP > inGameChar.maxHP)
                    {
                        inGameChar.currentHP = inGameChar.maxHP;
                    }
                    GameManager.instance.RemoveItem(itemName);
                }
                else
                {
                    /* notification */
                    BattleManager.instance.battleNotice.theText.text = "Cannot use on dead character";
                    BattleManager.instance.battleNotice.Activate();
                }
            }

            if (affectMP)
            {
                if (!selectedChar.hasDied)
                {
                    selectedChar.currentMP += amountToChange;

                    if (selectedChar.currentMP > selectedChar.maxMP)
                    {
                        selectedChar.currentMP = selectedChar.maxMP;
                    }

                    // have to update out of battle character MP because that's where mp is tracked when performing magic attack
                    inGameChar.currentMP += amountToChange;

                    if (inGameChar.currentMP > inGameChar.maxMP)
                    {
                        inGameChar.currentMP = inGameChar.maxMP;
                    }

                    GameManager.instance.RemoveItem(itemName);
                }
                else
                {
                    /* notification */
                    BattleManager.instance.battleNotice.theText.text = "Cannot use on dead character";
                    BattleManager.instance.battleNotice.Activate();
                }
            }

            if(affectLife)
            {
                if (selectedChar.hasDied)
                {
                    selectedChar.currentHP += selectedChar.maxHP;
                    selectedChar.hasDied = false;
                    BattleManager.instance.activeBattlers[charToUseOn].hasDied = false; // set battle character back to alive
                    GameManager.instance.playerStats[charToUseOn].isDead = false; // set out of battle character to alive
                    GameManager.instance.RemoveItem(itemName);
                }
                else
                {
                    /* notification */
                    BattleManager.instance.battleNotice.theText.text = "Cannot use on living character";
                    BattleManager.instance.battleNotice.Activate();
                }
            }

            // seems like we didn't implement any sort of strength potion but there's no harm in leaving this in case we do
            if (affectStr)
            {
                selectedChar.strength += amountToChange;
                GameManager.instance.RemoveItem(itemName);
            }
        }

        if (isWeapon)
        {
            if (selectedChar.equippedWpn != noItemEquipped)
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
            GameManager.instance.RemoveItem(itemName);
        }

        if (isArmour)
        {
            if (selectedChar.equippedArmr != noItemEquipped)
            {
               //GameManager.instance.AddItem(selectedChar.equippedArmr); // add current item equipped back into battle inventory
                GameManager.instance.AddItem(inGameChar.equippedArmr); // add current item equppied back into regular inventory
            }
            // change battle items
            selectedChar.equippedArmr = itemName;
            // change in-game items
            selectedChar.armrPwr = armourStrength;
            selectedChar.magDef = magicDefense;

            inGameChar.equippedArmr = itemName;
            inGameChar.armrPwr = armourStrength;
            inGameChar.magDef = magicDefense;
            GameManager.instance.RemoveItem(itemName); 
        }
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
                GameManager.instance.RemoveItem(itemName);
            }

            if (affectMP)
            {
                selectedChar.currentMP += amountToChange;

                if (selectedChar.currentMP > selectedChar.maxMP)
                {
                    selectedChar.currentMP = selectedChar.maxMP;
                }
                GameManager.instance.RemoveItem(itemName);
            }

            // this situation shouldn't come up (shouldn't be able to go to zero health outside of battle)
            if (affectLife)
            {
                if (selectedChar.isDead)
                {
                    selectedChar.currentHP += selectedChar.maxHP;
                    selectedChar.isDead = false;
                    GameManager.instance.playerStats[charToUseOn].isDead = false;
                    GameManager.instance.RemoveItem(itemName);
                }
                else
                {
                    /* notification */
                    BattleManager.instance.battleNotice.theText.text = "Cannot use on living character";
                    BattleManager.instance.battleNotice.Activate(); ;
                }
            }

            if (affectStr)
            {
                selectedChar.strength += amountToChange;
                GameManager.instance.RemoveItem(itemName);
            }
        }

        if (isWeapon)
        {
            if (selectedChar.equippedWpn != noItemEquipped)
            {
                GameManager.instance.AddItem(selectedChar.equippedWpn);
            }

            selectedChar.equippedWpn = itemName;
            selectedChar.wpnPwr = weaponStrength;
            GameManager.instance.RemoveItem(itemName);
        }

        if (isArmour)
        {
            if (selectedChar.equippedArmr != noItemEquipped)
            {
                GameManager.instance.AddItem(selectedChar.equippedArmr);
            }

            selectedChar.equippedArmr = itemName;
            selectedChar.armrPwr = armourStrength;
            GameManager.instance.RemoveItem(itemName);
        }
    }
}   
