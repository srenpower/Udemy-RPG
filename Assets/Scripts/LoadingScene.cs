using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadingScene : MonoBehaviour
{
    public float waitToLoad;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(waitToLoad > 0)
        {
            waitToLoad -= Time.deltaTime;
            if(waitToLoad <= 0)
            {
                SceneManager.LoadScene(PlayerPrefs.GetString("Save_One_Scene")); // need to change when switching to a 3 save system

                GameManager.instance.LoadData("Save_One");
                //QuestManager.instance.LoadQuestData();
            }
        }
    }
}
