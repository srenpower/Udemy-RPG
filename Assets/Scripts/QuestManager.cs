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
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
