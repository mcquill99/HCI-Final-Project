using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static TimerSystem;

public class SplitText : MonoBehaviour
{
    private bool isRunning = true;
    public GameObject splitsText;
    private float timerTime;

    private static string filePath;

    public GameObject totalTime;
    // Start is called before the first frame update
    void Start()
    {
        filePath = Application.persistentDataPath + "/splits.json";
        
    }

    // Update is called once per frame
    void Update()
    {
        if(isRunning){
            timerTime = TimerSystem.getTimerTime();
            totalTime.GetComponent<Text>().text = timerTime.ToString("n2");

        }
    }

    /*public void loadSplits(SplitList splits){
        foreach(Split split in SplitList.splits){
            splitsText.GetComponent<Text>().text += split.name;
        }
    }*/
}
