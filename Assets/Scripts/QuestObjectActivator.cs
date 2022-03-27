using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestObjectActivator : MonoBehaviour
{
    public GameObject objectToActivate;

    public string[] questsToCheck;

    public bool mustCompleteAll; // if all quests must be complete to activate/deactivate - otherwise any one of the quests completed will pass
    public bool activeIfComplete;
    public bool deactivateIfComplete;

    private bool allComplete = false; // used to indicate whether all quests are complete when it is required
    private bool oneComplete = false;
    private bool questObjectiveComplete; // used to stop update checks if this activator has been accomplished

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(!questObjectiveComplete)
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
            }
            else
            {
                allComplete = false;
            }
        }
        // if all quests must be completed and they have been completed
        if (mustCompleteAll && allComplete)
        {
            CompleteQuestObjective();
        }
        // if not all quests must be completed and at least one of them have been completed
        if(!mustCompleteAll && oneComplete)
        {
            CompleteQuestObjective();
        }
    }
    
    public void CompleteQuestObjective()
    { 
        if (activeIfComplete)
        {
            objectToActivate.SetActive(activeIfComplete);
            questObjectiveComplete = true;
        }
        else
        {
            objectToActivate.SetActive(!deactivateIfComplete);
            questObjectiveComplete = true;
        }
    }
}
