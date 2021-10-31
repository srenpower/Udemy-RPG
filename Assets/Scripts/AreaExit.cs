using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AreaExit : MonoBehaviour
{
    // Variables
    public string areaToLoad;

    public string areaTransitionName;

    public AreaEntrance theEntrance;

    public float waitToLoad = 1f; 
    private bool shouldLoadAfterFade;

    // Start is called before the first frame update
    void Start()
    {
        theEntrance.transitionName = areaTransitionName;
    }

    // Update is called once per frame
    void Update()
    {
        if(shouldLoadAfterFade)
        {
            waitToLoad -= Time.deltaTime;
            if(waitToLoad <= 0)
            {
                shouldLoadAfterFade = false;
                SceneManager.LoadScene(areaToLoad);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "Player")
        {
            //SceneManager.LoadScene(areaToLoad);
            shouldLoadAfterFade = true; // when hitting an exit we change bool to load after fade to true
            GameManager.instance.fadingBetweenAreas = true; // stop player movement while fading between scenes is occuring 
            UIFade.instance.FadeToBlack(); // call fade from UIFade instance

            PlayerController.instance.areaTransitionName = areaTransitionName;
        }
    }
}
