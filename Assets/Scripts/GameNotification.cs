using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameNotification : MonoBehaviour
{
    public Text theText;
    public Button closeButton;
    private bool buttonOn;

    public float awakeTime;
    private float awakeCounter;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (!buttonOn)
        {
            if (awakeCounter > 0)
            {
                awakeCounter -= Time.deltaTime;
                if (awakeCounter <= 0)
                {
                    gameObject.SetActive(false);
                }
            }
        }
    }

    public void Activate(bool closeButtonOn)
    {
        buttonOn = closeButtonOn;
        gameObject.SetActive(true);
        if (buttonOn)
        {
            closeButton.gameObject.SetActive(true);
        }
        else
        {
            awakeCounter = awakeTime;
            closeButton.gameObject.SetActive(false);
        }
    }

    public void CloseNotification()
    {
        gameObject.SetActive(false);
        Debug.Log("Button Pressed");
    }
}

