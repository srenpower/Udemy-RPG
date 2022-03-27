using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BattleMagicSelect : MonoBehaviour
{

    public string spellName;
    public int spellCost;
    public Text nameText;
    public Text costText;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Press()
    {
        // check if player has mp to perform spell - perform spell if true
        if (BattleManager.instance.activeBattlers[BattleManager.instance.currentTurn].currentMP >= spellCost)
        {
            BattleManager.instance.magicMenu.SetActive(false); // close magic menu
            BattleManager.instance.OpenTargetMenu(spellName); // open target menu to choose target of spell
            BattleManager.instance.activeBattlers[BattleManager.instance.currentTurn].currentMP -= spellCost; // decrement mp by spell cost
        }
        else
        {
            // let player know there is not enough MP
            BattleManager.instance.battleNotice.theText.text = "Not Enough MP!";
            BattleManager.instance.battleNotice.Activate();
            BattleManager.instance.magicMenu.SetActive(false);
        }
    }
}
