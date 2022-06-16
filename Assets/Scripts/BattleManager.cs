using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

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

    public GameObject targetMenu;
    public BattleTargetButton[] targetButtons;

    public GameObject magicMenu;
    public BattleMagicSelect[] magicButtons;

    public BattleNotification battleNotice;

    public int chanceToFlee = 40;
    private bool fleeing = false;

    public string gameOverScene;

    public bool cannotFlee;

    private static readonly FontStyle CHARACTER_TURN_STYLE = FontStyle.Bold;
    private static readonly FontStyle CHARACTER_IDLE_STYLE = FontStyle.Normal;
    private static readonly Color CHARACTER_TURN_COLOUR = Color.yellow;
    private static readonly Color CHARACTER_IDLE_COLOUR = Color.white;

    // Extras
    public bool markQuestComplete;
    public string questToMark;
    public bool completeWithFlee;
    public bool deactivateOnceComplete;
    public bool deactivateOnFlee;
    private BattleStarter activeBattleStarter;

    [Header("Rewards Variables")]
    public int rewardXP;
    public string[] rewardItems;

    // Start is called before the first frame update
    void Start()
    {
        instance = this;
        DontDestroyOnLoad(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        /*
        // for testing purposes
        if(Input.GetKeyDown(KeyCode.T))
        {
            BattleStart(new string[] { "Eyeball" }, false); // create new array to send to function
        }
        */
        // if battle scene is open
        if (battleActive)
        {
            UpdateBattle();

            // if we're in turn mode
            if (turnWaiting)
            {
                // if it is a players turn
                if (activeBattlers[currentTurn].isPlayer)
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
            if (Input.GetKeyDown(KeyCode.N))
            {
                NextTurn();
            }
        }
    }

    public void BattleStart(string[] enemiesToSpawn, bool setCannotFlee, BattleStarter battleStarter)
    {
        if (!battleActive)
        {
            Debug.Assert(activeBattleStarter == null, "trying to start a battle when we already have one");
            activeBattleStarter = battleStarter;
            cannotFlee = setCannotFlee;
            battleActive = true; // battle is now active

            GameManager.instance.battleActive = true; // set battle to active in GameManager to halt menus and player movement

            transform.position = new Vector3(Camera.main.transform.position.x, Camera.main.transform.position.y, transform.position.z); // set battle scene to display in front of camera   
            battleScene.SetActive(true); // display battle scene

            AudioManager.instance.PlayBGM(0); // set battle music

            for (int i = 0; i < playerPositions.Length; i++)
            {
                // check if player is active 
                if (GameManager.instance.playerStats[i].gameObject.activeInHierarchy)
                {
                    for (int j = 0; j < playerPrefabs.Length; j++)
                    {
                        // find player prefab that matches name in playerStats
                        if (playerPrefabs[j].charName == GameManager.instance.playerStats[i].charName)
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
                            activeBattlers[i].magDef = thePlayer.magDef;
                            activeBattlers[i].hasDied = thePlayer.isDead;
                        }
                    }
                }
            }

            UpdateUIStats();

            for (int i = 0; i < enemiesToSpawn.Length; i++)
            {
                if (enemiesToSpawn[i] != "")
                {
                    for (int j = 0; j < enemyPrefabs.Length; j++)
                    {
                        if (enemyPrefabs[j].charName == enemiesToSpawn[i])
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
        if (currentTurn >= activeBattlers.Count)
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

        for (int i = 0; i < activeBattlers.Count; i++)
        {
            // hp is less than 0, so player is dead
            if (activeBattlers[i].currentHP < 0)
            {
                // set hp to zero
                activeBattlers[i].currentHP = 0;
            }
            if (activeBattlers[i].currentHP == 0)
            {
                // Handle dead battler
                    if (activeBattlers[i].isPlayer)
                {
                    activeBattlers[i].theSprite.sprite = activeBattlers[i].deadSprite;
                    // set player to *dead* so that they can't be given hp and must have rejuvinate 
                    GameManager.instance.playerStats[i].isDead = true; // sets player in game manager to dead
                    activeBattlers[i].hasDied = true; // sets active battler to dead
                }
                else
                {
                    activeBattlers[i].EnemyFade();
                }
            }
            else
            {
                // set all players or enemies dead to false based on living player/enemy
                if (activeBattlers[i].isPlayer)
                {
                    allPlayersDead = false;
                    activeBattlers[i].theSprite.sprite = activeBattlers[i].aliveSprite;
                }
                else
                {
                    allEnemiesDead = false;
                }
            }
        }

        // if all enemies or all players are dead
        if (allEnemiesDead || allPlayersDead)
        {
            if (allEnemiesDead)
            {
                Debug.Log("Enemies are dead!");
                // end battle in victory
                StartCoroutine(EndBattleCo());
            }
            else
            {
                // end battle in failure
                StartCoroutine(GameOverCo());
            }

            // used before full victory state was implemented
            /*battleScene.SetActive(false);
            GameManager.instance.battleActive = false;
            battleActive = false;*/
        }
        // some players and some enemies are still alive (but at least one of each is still alive
        else
        {
            // if battler in turn order is dead increment turn without player getting turn
            while (activeBattlers[currentTurn].currentHP == 0)
            {
                currentTurn++;
                if (currentTurn >= activeBattlers.Count)
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
        for (int i = 0; i < activeBattlers.Count; i++)
        {
            if (activeBattlers[i].isPlayer && activeBattlers[i].currentHP > 0)
            {
                players.Add(i); // add living players
            }
        }
        // pick random player target for enemy to attack
        int selectedTarget = players[Random.Range(0, players.Count)];

        int selectAttack = Random.Range(0, activeBattlers[currentTurn].movesAvailable.Length);
        int movePower = 0; // amount of damage a chosen move does
        bool isMagic = false;
        for (int i = 0; i < movesList.Length; i++)
        {
            if (movesList[i].moveName == activeBattlers[currentTurn].movesAvailable[selectAttack])
            {
                Instantiate(movesList[i].theEffect, activeBattlers[selectedTarget].transform.position, activeBattlers[selectedTarget].transform.rotation);
                movePower = movesList[i].movePower;
                isMagic = movesList[i].isSpell;
                Debug.Log(activeBattlers[currentTurn].charName + " is using " + movesList[i].moveName); 
            }
        }

        Instantiate(enemyAttackEffect, activeBattlers[currentTurn].transform.position, activeBattlers[currentTurn].transform.rotation);

        DealDamage(selectedTarget, movePower, isMagic);
    }

    public void DealDamage(int target, int movePower, bool isMagic)
    {
        float atkPwr = activeBattlers[currentTurn].strength + activeBattlers[currentTurn].wpnPwr;
        float defPwr = activeBattlers[target].defense + activeBattlers[target].armrPwr;
        float magPwr = ((activeBattlers[currentTurn].maxMP) * .20f); // magic power 33% of maxMP
        float magDef = ((activeBattlers[target].defense) * .33f) + activeBattlers[target].magDef; // magic defense provided by armour
        float damageCalc;
        int damageToGive;
        
        // regular attack uses regular attack and defense power
        if(!isMagic)
        {
            damageCalc = (atkPwr / defPwr) * movePower * Random.Range(.9f, 1.1f);
            damageToGive = Mathf.RoundToInt(damageCalc);
        }
        // magic attack uses magic power and defense
        else
        {
            damageCalc = (magPwr / magDef) * movePower * Random.Range(.9f, 1.1f);
            damageToGive = Mathf.RoundToInt(damageCalc);
        }
        // damageCalc = magic power 30% * movePower / .33% reg defense + armMagicDefense

        Debug.Log(activeBattlers[currentTurn].charName + " is dealing " + damageCalc + "(" + damageToGive + ") damage to " + activeBattlers[target].charName);

        activeBattlers[target].currentHP -= damageToGive;

        Instantiate(theDamageNumber, activeBattlers[target].transform.position, activeBattlers[target].transform.rotation).SetDamage(damageToGive);

        UpdateUIStats();
    }

    public void UpdateUIStats()
    {
        for (int i = 0; i < playerName.Length; i++)
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

                    if (i == currentTurn)
                    {
                        playerName[i].fontStyle = CHARACTER_TURN_STYLE;
                        playerName[i].color = CHARACTER_TURN_COLOUR;
                    }
                    else
                    {
                        playerName[i].fontStyle = CHARACTER_IDLE_STYLE;
                        playerName[i].color = CHARACTER_IDLE_COLOUR; ;
                    }
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
    public void PlayerAttack(string moveName, int selectedTarget)
    {
        int movePower = 0; // amount of damage a chosen move does
        bool isMagic = false;
        for (int i = 0; i < movesList.Length; i++)
        {
            if (movesList[i].moveName == moveName)
            {
                Instantiate(movesList[i].theEffect, activeBattlers[selectedTarget].transform.position, activeBattlers[selectedTarget].transform.rotation);
                movePower = movesList[i].movePower;
                isMagic = movesList[i].isSpell;
            }
        }

        Instantiate(enemyAttackEffect, activeBattlers[currentTurn].transform.position, activeBattlers[currentTurn].transform.rotation);

        DealDamage(selectedTarget, movePower, isMagic);

        uiButtonsHolder.SetActive(false); // disable buttons so they can't be double pressed
        targetMenu.SetActive(false);

        NextTurn();
    }

    public void OpenTargetMenu(string moveName)
    {
        targetMenu.SetActive(true);

        List<int> Enemies = new List<int>();

        for (int i = 0; i < activeBattlers.Count; i++)
        {
            // if activebattler is an enemy (not player)
            if (!activeBattlers[i].isPlayer)
            {
                Enemies.Add(i); // add to list
            }
        }

        // sort through target buttons and set 
        for (int i = 0; i < targetButtons.Length; i++)
        {
            if (Enemies.Count > i && activeBattlers[Enemies[i]].currentHP > 0)
            {
                targetButtons[i].gameObject.SetActive(true);

                targetButtons[i].moveName = moveName;
                targetButtons[i].activeBattlerTarget = Enemies[i];
                targetButtons[i].targetName.text = activeBattlers[Enemies[i]].charName;
            }
            else
            {
                targetButtons[i].gameObject.SetActive(false);
            }
        }
    }

    public void CloseTargetMenu()
    {
        targetMenu.SetActive(false);
    }

    public void OpenMagicMenu()
    {
        magicMenu.SetActive(true); // open magic menu

        // for each magic button assign a spell until all player spells are shown
        for (int i = 0; i < magicButtons.Length; i++)
        {
            if (activeBattlers[currentTurn].movesAvailable.Length > i)
            {
                magicButtons[i].gameObject.SetActive(true); // set button as active

                magicButtons[i].spellName = activeBattlers[currentTurn].movesAvailable[i];
                magicButtons[i].nameText.text = magicButtons[i].spellName;

                for (int j = 0; j < movesList.Length; j++)
                {
                    if (movesList[j].moveName == magicButtons[i].spellName)
                    {
                        magicButtons[i].spellCost = movesList[j].moveCost;
                        magicButtons[i].costText.text = magicButtons[i].spellCost.ToString();
                    }
                }
            }
            else
            {
                magicButtons[i].gameObject.SetActive(false);
            }
        }
    }

    public void CloseMagicMenu()
    {
        magicMenu.SetActive(false);
    }

    public void Flee()
    {
        if (cannotFlee)
        {
            battleNotice.theText.text = "Cannot flee this battle!";
            battleNotice.Activate();
        }
        else
        {
            int fleeSuccess = Random.Range(0, 100);

            if (fleeSuccess < chanceToFlee)
            {
                // end the battle
                //battleActive = false;
                //battleScene.SetActive(false);
                fleeing = true;
                StartCoroutine(EndBattleCo());
            }
            else
            {
                NextTurn();
                battleNotice.theText.text = "Couldn't escape!";
                battleNotice.Activate();
            }
        }
    }

    public IEnumerator EndBattleCo()
    {
        battleActive = false;

        yield return new WaitForSeconds(.5f);

        UIFade.instance.FadeToBlack();

        yield return new WaitForSeconds(1.5f);

        battleScene.SetActive(false);
        uiButtonsHolder.SetActive(false);
        targetMenu.SetActive(false);
        magicMenu.SetActive(false);

        for (int i = 0; i < activeBattlers.Count; i++)
        {
            if (activeBattlers[i].isPlayer)
            {
                for (int j = 0; j < GameManager.instance.playerStats.Length; j++)
                {
                    if (activeBattlers[i].charName == GameManager.instance.playerStats[j].charName)
                    {
                        if (activeBattlers[i].currentHP <= 0)
                        {
                            activeBattlers[i].currentHP = 10;
                            activeBattlers[i].hasDied = false;
                            GameManager.instance.playerStats[j].isDead = false;
                        }
                        GameManager.instance.playerStats[j].currentHP = activeBattlers[i].currentHP;
                        GameManager.instance.playerStats[j].currentMP = activeBattlers[i].currentMP;
                    }
                }
            }

            Destroy(activeBattlers[i].gameObject);
        }

        // checks whether player is fleeing or is allowed to complete quest objective by running away
        if ((markQuestComplete && !fleeing) || (markQuestComplete && completeWithFlee))
        {
            QuestManager.instance.MarkQuestComplete(questToMark);
        }

        // checks if player fulfilled requirements (beat enemies or flee is a valid win condition to deactivate) 
        if ((deactivateOnceComplete && !fleeing) || (deactivateOnceComplete && completeWithFlee))
        {
            // set game object to inactive
            activeBattleStarter.gameObject.SetActive(false);
        }

        if ((deactivateOnFlee && fleeing))
        {
            // set game object to inactive
            activeBattleStarter.gameObject.SetActive(false);
        }

        activeBattleStarter = null;
        UIFade.instance.FadeFromBlack();
        activeBattlers.Clear();
        currentTurn = 0;

        if(fleeing)
        {
            GameManager.instance.battleActive = false;
            fleeing = false;
        }
        else
        {
            BattleRewards.instance.OpenRewardScreen(rewardXP, rewardItems);
        }

        AudioManager.instance.PlayBGM(FindObjectOfType<CameraController>().musicToPlay);
    }

    public IEnumerator GameOverCo()
    {
        battleActive = false;
        battleScene.SetActive(false);

        UIFade.instance.FadeToBlack();

        yield return new WaitForSeconds(1.5f);

        SceneManager.LoadScene(gameOverScene);
    }
}
