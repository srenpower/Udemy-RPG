using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestObjectActivator : MonoBehaviour
{
    public GameObject objectToActivate;

    public string[] questsToCheck;

    public bool mustCompleteAll; // if all quests must be complete to activate/deactivate - otherwise any one of the quests completed will pass

    [Header("Quest Complete Checks")]
    public bool activeIfComplete;
    public bool deactivateIfComplete;

    [Header("Quest Incomplete Checks")]
    public bool activateIfIncomplete;
    public bool deactivateIfIncomplete;
    

    private bool allComplete = false; // used to indicate whether all quests are complete when it is required
    private bool oneComplete = false;
    private bool activatorObjectiveComplete; // used to stop update checks if this activator has been accomplished

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
            if (!activatorObjectiveComplete)
            {
                CheckCompletion();
            }
    }

    public void CheckCompletion()
    {
            // Check if all or at least one of the required quests have been completed
            for (int i = 0; i < questsToCheck.Length; i++)
            {
                if (QuestManager.instance.CheckIfComplete(questsToCheck[i]))
                {
                    allComplete = true;
                    oneComplete = true;
                    if (mustCompleteAll)
                    {
                        for (int j = 0; j < questsToCheck.Length; j++)
                        {
                            if (!QuestManager.instance.CheckIfComplete(questsToCheck[j]))
                            {
                                allComplete = false;
                            }
                        }
                    }
                }
                else
                {
                    allComplete = false;
                }
            }
            // if all quests must be completed and they have been completed
            // if not all quests must be completed and at least one of them have been completed
            // if quest is supposed to be incomplete and it is incomplete
            if ((mustCompleteAll && allComplete) || (!mustCompleteAll && oneComplete) || ((activateIfIncomplete || deactivateIfIncomplete) && !allComplete))
            {
                CompleteQuestObjective();
            }
    }
    
    public void CompleteQuestObjective()
    {
        if (activeIfComplete || activateIfIncomplete)
        {
            objectToActivate.SetActive(true);
            activatorObjectiveComplete = true;
        }
        else if(deactivateIfIncomplete)
        {
            objectToActivate.SetActive(!deactivateIfIncomplete);
            activatorObjectiveComplete = true;
        }
        else if(deactivateIfComplete)
        {
            // I'm not convinced we do anything with this but I don't think it's doing any harm by staying
            if(objectToActivate.GetComponent<GetItem>())
            {
                if(!objectToActivate.GetComponent<GetItem>().DidRewardItem)
                {
                    return;
                }
            }
            objectToActivate.SetActive(!deactivateIfComplete);
            activatorObjectiveComplete = true;
        }
    }
}
