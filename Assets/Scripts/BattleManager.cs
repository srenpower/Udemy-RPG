using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleManager : MonoBehaviour
{
    public static BattleManager instance;

    public bool battleActive;

    public GameObject battleScene;

    public Transform[] playerPositions; // stores position for players to be placed in the scene
    public Transform[] enemyPositions; // stores positiosn for enemies to be placed in the scene

    public BattleChar[] playerPrefabs; // store what players should be placed in battle
    public BattleChar[] enemyPrefabs; // store what enemies should be placed in battle

    // Start is called before the first frame update
    void Start()
    {
        instance = this;
        DontDestroyOnLoad(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.T))
        {
            BattleStart(new string[] { "Eyeball", "Spider" }); // create new array to send to function
        }
    }

    public void BattleStart(string[] enemiesToSpawn)
    {
        if(!battleActive)
        {
            battleActive = true; // battle is now active

            GameManager.instance.battleActive = true; // set battle to active in GameManager to halt menus and player movement

            battleScene.SetActive(true);
        }
    }
}
