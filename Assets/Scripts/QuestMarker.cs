using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestMarker : MonoBehaviour
{
    public string questToMark; // set quest the zone/marker is attached to
    public bool markComplete; // bool to mark quest complete
    public bool markIncomplete;

    public bool markOnEnter; // should quest marker trigger complete as soon as player enters zone
    private bool canMark; // sets whether item has been marked already or not

    public bool deactivateOnMarking;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (canMark && Input.GetButtonDown("Fire1"))
        {
            canMark = false;
            MarkQuest();
        }
    }

    public void MarkQuest()
    {
        if(markComplete)
        {
            QuestManager.instance.MarkQuestComplete(questToMark);
        }
        else if(markIncomplete)
        {
            QuestManager.instance.MarkQuestIncomplete(questToMark);
        }

        gameObject.SetActive(!deactivateOnMarking); // set game object to inactive if deactivateOnMarking is true
    }

    // when entering quest zone
    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "Player")
        {
            if (markOnEnter)
            {
                MarkQuest();
            }
            else
            {
                canMark = true;
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            canMark = false;
        }
    }
}

