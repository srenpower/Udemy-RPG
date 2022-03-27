using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetItem : MonoBehaviour
{
    private bool canPickup;
    public string[] rewardItems;
    public bool autoReceive;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!autoReceive)
        {
            if (canPickup && Input.GetButtonDown("Fire1"))
            {
                AddItems();
            }
        }
        else
        {
            if (canPickup)
            {
                Debug.Log("CAN PICKUP");
                AddItems();
                autoReceive = false;
                canPickup = false;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            canPickup = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "Player" && !autoReceive)
        {
            Debug.Log("SHOULD NOT REACH");
            canPickup = false;
        }
    }

    public void AddItems()
    {
        for (int i = 0; i < rewardItems.Length; i++)
        {
            GameManager.instance.AddItem(rewardItems[i]);
        }
    }
}
