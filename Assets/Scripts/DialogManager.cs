using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; // accessing and using textbox UI

public class DialogManager : MonoBehaviour
{
    // Variables
    public Text dialogText;
    public Text nameText;
    public GameObject dialogBox;
    public GameObject nameBox;

    public string[] dialogLines; // string array

    public int currentLine; // determines what line we're on
    private bool justStarted; // used to block the dialog from passing too quickly

    public static DialogManager instance;

    // Start is called before the first frame update
    void Start()
    {
        instance = this;
        dialogBox.SetActive(false);

        // following code was just a test
        // dialogText.text = dialogLines[currentLine]; // finds text element in dialogText object
    }

    // Update is called once per frame
    void Update()
    {
        if(dialogBox.activeInHierarchy)
        {
            if(Input.GetButtonUp("Fire1"))
            // GetButtonUp only activates once button press is complete (button is pressed and then let go of) 
            {
                if (!justStarted)
                {
                    currentLine++; // advance current line

                    // check to see if we are at the end of the dialogLines array
                    if (currentLine >= dialogLines.Length)
                    {
                        // deactivate dialogBox
                        dialogBox.SetActive(false);

                        // reactivate player movement because dialog window has been closed
                        GameManager.instance.dialogActive = false;
                    }
                    else
                    {
                        // Check if new line is a name change
                        CheckIfName();

                        // change text in dialogBox to current line text
                        dialogText.text = dialogLines[currentLine];
                    }
                } else
                {
                    justStarted = false;
                }
            }
        }
    }
    
    // is called on successful collision to show dialog box and text
    public void ShowDialog(string[] newLines, bool isPerson)
    {

        // takes dialog and sets to dialog lines
        dialogLines = newLines;

        // start at first line
        currentLine = 0;

        // Set nameBox active based on isPerson true or false
        nameBox.SetActive(isPerson);

        // check if current line indicates name or name change
        CheckIfName();

        // set dialogText.text to first current line (which will be the first non-name line)
        dialogText.text = dialogLines[currentLine]; 
        
        // make dialog box pop up
        dialogBox.SetActive(true);

        justStarted = true;

        // tell player to stop moving while dialog box is open
        GameManager.instance.dialogActive = true;
    }

    public void CheckIfName()
    {
        if(dialogLines[currentLine].StartsWith("n-"))
        {
            nameText.text = dialogLines[currentLine].Replace("n-", "");
            currentLine++;
        }
    }
}
