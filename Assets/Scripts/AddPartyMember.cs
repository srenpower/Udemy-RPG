using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddPartyMember : MonoBehaviour
{
    public bool autoAdd;
    private bool isInTrigger;
    private bool isInParty = false;

    [SerializeField]
    private int playerIndex;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!autoAdd)
        {
            if (Input.GetButtonDown("Fire1") && isInTrigger && !isInParty)
            {
                //GameMenu.instance.gameNotification.Activate(buttonOn);
                //GameMenu.instance.gameNotification.theText.text = message;
                Debug.Log("Activate Player");
                GameManager.instance.playerStats[playerIndex].gameObject.SetActive(true);
                isInParty = true;
            }
        }
        else
        {
            if (isInTrigger && !isInParty)
            {
                Debug.Log("Activate Player");
                GameManager.instance.playerStats[playerIndex].gameObject.SetActive(true);
                isInParty = true;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "Player")
        {
            isInTrigger = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            isInTrigger = false;
        }
    }
}
