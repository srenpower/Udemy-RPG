using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveBetweenScenes : MonoBehaviour
{
    public int saveNum;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void CallSaveState(int saveNum)
    {
        GameManager.instance.SaveData(saveNum);
    }
}
