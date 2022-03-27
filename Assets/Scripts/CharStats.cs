using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharStats : MonoBehaviour
{
    private static readonly System.Random rand = new System.Random();

    public string charName;
    public int playerLevel = 1;
    public int currentEXP;

    // variables for level up system 
    public int[] expToNextLevel; // how much exp required at each level
    public int maxLevel = 20;
    public int baseEXP = 200;


    // other stat variables
    public int currentHP = 0; // health points
    public int maxHP = 100; // default HP 30
    public int currentMP; // magic points
    public int maxMP = 30; // default MP 30
    public int[] mpLvlBonus;
    public int strength; // attack strength
    public int defense;
    public int wpnPwr; // weapon power
    public int armrPwr; // armour power
    public string equippedWpn;
    public string equippedArmr;
    public Sprite charImage;

    // Start is called before the first frame update
    void Start()
    {
        // calculate experience required to reach each level
        expToNextLevel = new int[maxLevel]; // instantiate level exp array to max level size
        mpLvlBonus = new int[maxLevel]; // instantiate array for mp level bonuses

        expToNextLevel[1] = baseEXP;
        currentMP = maxMP;

        // calculate experience required to level up at each level
        for(int i = 2; i < maxLevel; i++)
        {
            if (i < 20)
            {
                expToNextLevel[i] = (int)(expToNextLevel[i - 1] * 1.35);
            }
            else
            {
                expToNextLevel[i] = (int)(expToNextLevel[i - 1] * 1.05);
            }
        }

        int newMP = 1; // used for calculating mp for random levelups
        // calculate MP jumps to increase at quasi-random levels
        for (int i = 2; i < maxLevel; i++)
        {
            i += rand.Next(2,5); // save randomly generated level
            if(i >= maxLevel)
            {
                i = maxLevel-1;
            }

            newMP += rand.Next(4, 8); // assign random amount of MP 
            mpLvlBonus[i] = newMP;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.K))
        {
            AddExp(500);
        }
    }

    // add experience
    public void AddExp(int expToAdd)
    {
        currentEXP += expToAdd;

        if(playerLevel < maxLevel)
        {
            if(currentEXP >= expToNextLevel[playerLevel])
            {
                currentEXP -= expToNextLevel[playerLevel];

                playerLevel++;

                // determine whether to add to str or def on level up based on odd or even level number
                if (playerLevel % 2 == 0) // even number
                {
                    strength++;
                }
                else
                {
                    defense++;
                }

                // add to maxHP
                maxHP = (int)(maxHP * 1.05f);

                // max out current HP on levelup
                currentHP = maxHP;

                // set new mp
                maxMP += mpLvlBonus[playerLevel];
                currentMP = maxMP;
            }
        }
        else
        {
            currentEXP = 0;
        }
    }
}
