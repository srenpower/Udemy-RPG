using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestManager : MonoBehaviour
{
    // in unity questMarkerNames and questMarkersComplete must be same length
    public string[] questMarkerNames; 
    public bool[] questMarkersComplete;

    public static QuestManager instance;

    // Start is called before the first frame update
    void Start()
    {
<<<<<<< HEAD
<<<<<<< HEAD
        instance = this;

        questMarkersComplete = new bool[questMarkerNames.Length];
=======
        instance = this;    
>>>>>>> 878d4dade1d7376ca2088274fcd743251a189584
=======
        instance = this;    
>>>>>>> fb1402345eb28c0e56d5ae0f46e90108b56f6c3c
    }

    // Update is called once per frame
    void Update()
    {
<<<<<<< HEAD
<<<<<<< HEAD
        if(Input.GetKeyDown(KeyCode.Q))
        {
            Debug.Log(CheckIfComplete("quest test"));
            MarkQuestComplete("quest test");
            MarkQuestIncomplete("fight the demon");
        }
    }

    public int GetQuestNumber(string questToFind)
    {
        for(int i = 0; i < questMarkerNames.Length; i++)
        {
            if(questMarkerNames[i] == questToFind)
            {
                return i;
            }
        }

        // if quest is not found - throw error - 0 gets returned
        Debug.LogError("Quest " + questToFind + " does not exist");

        // quest 0 is always a blank quest for this reason - considered invalid test
        return 0;
    }

    public bool CheckIfComplete(string questToCheck)
    {
        if(GetQuestNumber(questToCheck) != 0)
        {
            return questMarkersComplete[GetQuestNumber(questToCheck)];
        }

        return false;
    }

    public void MarkQuestComplete(string questToMark)
    {
        questMarkersComplete[GetQuestNumber(questToMark)] = true;

        UpdateLocalQuestObjects();
    }

    public void MarkQuestIncomplete(string questToMark)
    {
        questMarkersComplete[GetQuestNumber(questToMark)] = false;

        UpdateLocalQuestObjects();
    }

    public void UpdateLocalQuestObjects()
    {
        QuestObjectActivator[] questObjects = FindObjectsOfType<QuestObjectActivator>();

        if(questObjects.Length > 0)
        {
            for(int i = 0; i < questObjects.Length; i++)
            {
                questObjects[i].CheckCompletion();
            }
        }
=======
        
>>>>>>> 878d4dade1d7376ca2088274fcd743251a189584
=======
        
>>>>>>> fb1402345eb28c0e56d5ae0f46e90108b56f6c3c
    }
}
