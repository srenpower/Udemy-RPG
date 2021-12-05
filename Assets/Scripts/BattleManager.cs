using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BattleManager : MonoBehaviour
{
    public static BattleManager instance;

    public bool battleActive;

    public GameObject battleScene;

    public Transform[] playerPositions; // stores position for players to be placed in the scene
    public Transform[] enemyPositions; // stores positiosn for enemies to be placed in the scene

    public BattleChar[] playerPrefabs; // store what players should be placed in battle
    public BattleChar[] enemyPrefabs; // store what enemies should be placed in battle

    public List<BattleChar> activeBattlers = new List<BattleChar>();

    public int currentTurn;
    public bool turnWaiting;

    public GameObject uiButtonsHolder;

    public BattleMove[] movesList; // serialized object creates list of variables in battlemanager for each attack type
    public GameObject enemyAttackEffect; // white effect behind enemy when it attacks

    public DamageNumber theDamageNumber; // number displayed above character to show amount of damage taken

    [Header("UI Stat Elements")]
    public Text[] playerName;
    public Text[] playerHP;
    public Text[] playerMP; 

    // Start is called before the first frame update
    void Start()
    {
        instance = this;
        DontDestroyOnLoad(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        // for testing purposes
        if(Input.GetKeyDown(KeyCode.T))
        {
            BattleStart(new string[] { "Eyeball", "Spider", "Skeleton" }); // create new array to send to function
        }

        // if battle scene is open
        if(battleActive)
        {
            // if we're in turn mode
            if(turnWaiting)
            {
                // if it is a players turn
                if(activeBattlers[currentTurn].isPlayer)
                {
                    uiButtonsHolder.SetActive(true); // show option buttons for player turn
                }
                else
                {
                    uiButtonsHolder.SetActive(false); // hide option buttons for players turn

                    // Enemy should attack
                    StartCoroutine(EnemyMoveCo());
                }
            }
            // for testing purposes
            if(Input.GetKeyDown(KeyCode.N))
            {
                NextTurn();
            }
        }

        UpdateBattle();
    }

    public void BattleStart(string[] enemiesToSpawn)
    {
        if(!battleActive)
        {
            battleActive = true; // battle is now active

            GameManager.instance.battleActive = true; // set battle to active in GameManager to halt menus and player movement

            transform.position = new Vector3(Camera.main.transform.position.x, Camera.main.transform.position.y, transform.position.z); // set battle scene to display in front of camera   
            battleScene.SetActive(true); // display battle scene

            AudioManager.instance.PlayBGM(0); // set battle music

            for(int i = 0; i < playerPositions.Length; i++)
            {
                // check if player is active 
                if(GameManager.instance.playerStats[i].gameObject.activeInHierarchy)
                {
                    for(int j = 0; j < playerPrefabs.Length; j++)
                    {
                        // find player prefab that matches name in playerStats
                        if(playerPrefabs[j].charName == GameManager.instance.playerStats[i].charName)
                        {
                            Transform tempTest;
                            // show player in battle scene
                            BattleChar newPlayer = Instantiate(playerPrefabs[j], playerPositions[i].position, playerPositions[i].rotation);
                            newPlayer.transform.parent = playerPositions[i];
                            activeBattlers.Add(newPlayer);

                            CharStats thePlayer = GameManager.instance.playerStats[i]; // creating a local copy of the CharStats object so we don't need to call the GameManager each time
                            activeBattlers[i].isPlayer = true;
                            activeBattlers[i].currentHP = thePlayer.currentHP;
                            activeBattlers[i].maxHP = thePlayer.maxHP;
                            activeBattlers[i].currentMP = thePlayer.currentMP;
                            activeBattlers[i].maxMP = thePlayer.maxMP;
                            activeBattlers[i].strength = thePlayer.strength;
                            activeBattlers[i].defense = thePlayer.defense;
                            activeBattlers[i].wpnPwr = thePlayer.wpnPwr;
                            activeBattlers[i].armrPwr = thePlayer.armrPwr;
                        }
                    }
                }
            }

            for(int i = 0; i < enemiesToSpawn.Length; i++)
            {
                if(enemiesToSpawn[i] != "")
                {
                    for(int j = 0; j < enemyPrefabs.Length; j++)
                    {
                        if(enemyPrefabs[j].charName == enemiesToSpawn[i])
                        {
                            BattleChar newEnemy = Instantiate(enemyPrefabs[j], enemyPositions[i].position, enemyPositions[i].rotation);
                            newEnemy.transform.parent = enemyPositions[i];
                            activeBattlers.Add(newEnemy);
                        }
                    }
                }
            }
            turnWaiting = true;
            currentTurn = Random.Range(0, activeBattlers.Count);    
        }
    }

    // Increment turn and reset to start of active battlers if we're at the end of the list
    public void NextTurn()
    {
        currentTurn++;
        if(currentTurn >= activeBattlers.Count)
        {
            currentTurn = 0;
        }

        turnWaiting = true;

        UpdateBattle();
        UpdateUIStats();
    }

    public void UpdateBattle()
    {
        bool allEnemiesDead = true;
        bool allPlayersDead = true;

        for(int i = 0; i < activeBattlers.Count; i++)
        {
            // hp is less than 0, so player is dead
            if(activeBattlers[i].currentHP < 0)
            {
                // set hp to zero
                activeBattlers[i].currentHP = 0;
            }
            if(activeBattlers[i].currentHP ==0)
            {
                // Placeholder: Handle dead battler
            }
            else
            {
                // set all players or enemies dead to false based on living player/enemy
                if(activeBattlers[i].isPlayer)
                {
                    allPlayersDead = false;
                }
                else
                {
                    allEnemiesDead = false;
                }
            }
        }
        
        // if all enemies or all players are dead
        if(allEnemiesDead || allPlayersDead)
        {
            if(allEnemiesDead)
            {
                // end battle in victory
            }
            else
            {
                // end battle in failure
            }

            battleScene.SetActive(false);
            GameManager.instance.battleActive = false;
            battleActive = false;
        }
        // some players and some enemies are still alive (but at least one of each is still alive
        else
        {
            // if battler in turn order is dead increment turn without player getting turn
            while(activeBattlers[currentTurn].currentHP == 0)
            {
                currentTurn++;
                if(currentTurn >= activeBattlers.Count)
                {
                    currentTurn = 0;
                }
            }
        }
    }
    // creates pause between enemy attacks
    public IEnumerator EnemyMoveCo()
    {
        turnWaiting = false;
        yield return new WaitForSeconds(1f);
        EnemyyAttack();
        yield return new WaitForSeconds(1f);
        NextTurn();
    }
    public void EnemyyAttack()
    {
        List<int> players = new List<int>(); // create new list of players (not enemies)
        for(int i = 0; i < activeBattlers.Count; i++)
        {
            if(activeBattlers[i].isPlayer && activeBattlers[i].currentHP > 0)
            {
                players.Add(i); // add living players
            }
        }
        // pick random player target for enemy to attack
        int selectedTarget = players[Random.Range(0, players.Count)];

        int selectAttack = Random.Range(0, activeBattlers[currentTurn].movesAvailable.Length);
        int movePower = 0; // amount of damage a chosen move does
        for(int i = 0; i < movesList.Length; i++)
        {
            if(movesList[i].moveName == activeBattlers[currentTurn].movesAvailable[selectAttack])
            {
                Instantiate(movesList[i].theEffect, activeBattlers[selectedTarget].transform.position, activeBattlers[selectedTarget].transform.rotation);
                movePower = movesList[i].movePower;
            }
        }

        Instantiate(enemyAttackEffect, activeBattlers[currentTurn].transform.position, activeBattlers[currentTurn].transform.rotation);

        DealDamage(selectedTarget, movePower);
    }

    public void DealDamage(int target, int movePower)
    {
        float atkPwr = activeBattlers[currentTurn].strength + activeBattlers[currentTurn].wpnPwr;
        float defPwr = activeBattlers[target].defense + activeBattlers[target].armrPwr;

        float damageCalc = (atkPwr / defPwr) * movePower * Random.Range(.9f, 1.1f);
        int damageToGive = Mathf.RoundToInt(damageCalc); // 

        Debug.Log(activeBattlers[currentTurn].charName + " is dealing " + damageCalc + "(" + damageToGive + ") damage to " + activeBattlers[target].charName);

        activeBattlers[target].currentHP -= damageToGive;

        Instantiate(theDamageNumber, activeBattlers[target].transform.position, activeBattlers[target].transform.rotation).SetDamage(damageToGive);

        UpdateUIStats();
    }

    public void UpdateUIStats()
    {
        for(int i = 0; i < playerName.Length; i++)
        {
            if (activeBattlers.Count > i)
            {
                if (activeBattlers[i].isPlayer)
                {
                    BattleChar playerData = activeBattlers[i];

                    playerName[i].gameObject.SetActive(true);
                    playerName[i].text = playerData.charName;
                    playerHP[i].text = Mathf.Clamp(playerData.currentHP, 0, int.MaxValue) + "/" + playerData.maxHP;
                    playerMP[i].text = Mathf.Clamp(playerData.currentMP, 0, int.MaxValue) + "/" + playerData.maxMP;
                }
                else
                {
                    playerName[i].gameObject.SetActive(false);
                }
            }
            else
            {
                playerName[i].gameObject.SetActive(false);
            }
        }
    }
    
    // function which is called when player attacks
    public void PlayerAttack(string moveName /*,int selectedTarget*/)
    {
        int movePower = 0; // amount of damage a chosen move does
        for (int i = 0; i < movesList.Length; i++)
        {
            if (movesList[i].moveName == moveName)
            {
               // Instantiate(movesList[i].theEffect, activeBattlers[selectedTarget].transform.position, activeBattlers[selectedTarget].transform.rotation);
                movePower = movesList[i].movePower;
            }
        }

        Instantiate(enemyAttackEffect, activeBattlers[currentTurn].transform.position, activeBattlers[currentTurn].transform.rotation);

       // DealDamage(selectedTarget, movePower);
    }
}
