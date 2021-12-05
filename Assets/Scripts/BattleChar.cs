using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleChar : MonoBehaviour
{
    public bool isPlayer;
    public string[] movesAvailable; // determines what moves a character can do while fighting

    public string charName;
    public int currentHP, maxHP, currentMP, maxMP, strength, defense, wpnPwr, armrPwr;
    public bool hasDied;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

}
