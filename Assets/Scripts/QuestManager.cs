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
        instance = this;

        questMarkersComplete = new bool[questMarkerNames.Length];
    }

    // Update is called once per frame
    void Update()
    {

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
    }
    /* SWITCHED SAVING OVER TO GAMEMANAGER
    // loop through all quest markers and decide if they should be stored as true or false
    public void SaveQuestData()
    {
        for(int i = 0; i < questMarkerNames.Length; i++)
        {
            // quest markers are complete
            if(questMarkersComplete[i])
            {
                // Store "QuestMarker" name and set "1" as true
                PlayerPrefs.SetInt("QuestMarker_" + questMarkerNames[i],1);
            }
            else
            {
                // Store "QuestMarker" name and set "0" as false
                PlayerPrefs.SetInt("QuestMarker_" + questMarkerNames[i], 0);
            }
        }
    }

    public void LoadQuestData()
    {
        for(int i = 0; i < questMarkerNames.Length; i++)
        {
            int valueToSet = 0; // set as false by default
            if(PlayerPrefs.HasKey("QuestMarker_" + questMarkerNames[i]))
            {
                // player prefs stores default data (part of unity)
                valueToSet = PlayerPrefs.GetInt("QuestMarker_" + questMarkerNames[i]);
            }

            if(valueToSet == 0)
            {
                questMarkersComplete[i] = false;
            }
            else
            {
                questMarkersComplete[i] = true;
            }
        }
    }
    */
}
