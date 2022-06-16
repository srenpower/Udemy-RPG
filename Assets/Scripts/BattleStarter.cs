using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleStarter : MonoBehaviour
{
    public BattleType[] potentialBattles;

    public bool activateOnEnter;
    public bool activateOnStay;
    public bool activateOnExit;

    private bool inArea;
    public float timeBetweenBattles;
    private float betweenBattleCounter;

    public bool deactivateOnceComplete; // deactivates if battle is won or fleeing is considered a win condition
    public bool deactivateOnFlee; // deactivate when player flees battle - will not complete quest without checking complete with flee

    public bool cannotFlee;
    public bool completeWithFlee; //Fleeing still allows you to progress 

    public bool shouldCompleteQuest;
    public string questToComplete;

    public int customChanceToFlee;

    // Start is called before the first frame update
    void Start()
    {
        betweenBattleCounter = Random.Range(timeBetweenBattles * .5f, timeBetweenBattles * 1.5f);
    }

    // Update is called once per frame
    void Update()
    {
        if(inArea && PlayerController.instance.canMove)
        {
            if(Input.GetAxisRaw("Horizontal") != 0 || Input.GetAxisRaw("Vertical") != 0)
            {
                betweenBattleCounter -= Time.deltaTime;
            }

            if(betweenBattleCounter <= 0)
            {
                betweenBattleCounter = Random.Range(timeBetweenBattles * .5f, timeBetweenBattles * 1.5f);

                StartCoroutine(StartBattleCo());
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "Player")
        {
            if (activateOnEnter)
            {
                StartCoroutine(StartBattleCo());
            }
            else
            {
                inArea = true;
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            if (activateOnExit)
            {
                StartCoroutine(StartBattleCo());
            }
            else
            {
                inArea = false;
            }
        }
    }

    public IEnumerator StartBattleCo()
    {
        PlayerController.instance.canMove = false;
        yield return new WaitForSeconds(.2f);

        UIFade.instance.FadeToBlack();
        GameManager.instance.battleActive = true;

        int selectedBattle = Random.Range(0, potentialBattles.Length);

        BattleManager.instance.rewardItems = potentialBattles[selectedBattle].rewardItems;
        BattleManager.instance.rewardXP = potentialBattles[selectedBattle].rewardXP;

        yield return new WaitForSeconds(1.5f);

        BattleManager.instance.BattleStart(potentialBattles[selectedBattle].enemies, cannotFlee, this);
        UIFade.instance.FadeFromBlack();

        BattleManager.instance.markQuestComplete = shouldCompleteQuest;
        BattleManager.instance.questToMark = questToComplete;
        BattleManager.instance.deactivateOnceComplete = deactivateOnceComplete;
        BattleManager.instance.deactivateOnFlee = deactivateOnFlee;
        BattleManager.instance.completeWithFlee = completeWithFlee;
        if(customChanceToFlee != 0)
        {
            BattleManager.instance.chanceToFlee = customChanceToFlee;
        }
    }
}
