using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AreaEntrance : MonoBehaviour
{
    // Variables
    public string transitionName;

    // Start is called before the first frame update
    void Start()
    {
        if(transitionName == PlayerController.instance.areaTransitionName)
        {
            PlayerController.instance.transform.position = transform.position;
        }
        Debug.Log("Arrived in new scene");
        UIFade.instance.FadeFromBlack(); // fade from black in new scene
        GameManager.instance.fadingBetweenAreas = false; // allow player movement once fade is complete 
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
