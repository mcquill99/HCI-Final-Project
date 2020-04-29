using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;
using System.IO;
using System;
using UnityEngine.UI;


public class TimerSystem : MonoBehaviour
{ 
    private static float timerStartTime; // initial time
    private static bool isRunning; // toggles updating

    private static float currentTime = 0.00F; //placeholder to start timer

    public int splitIndex = 0; //split iterator

    private List<float> currentRun = new List<float>(); //current splits
    public SplitList bestSplits; // list of all splits

    private static string filePath; //file path to save to

    public GameObject totalTime; //text object to display time to complete
    public GameObject SplitNamesUI; //names of the splits that appear on screen
    public GameObject SplitTimesUI; //times for each split that appear on screen
    public GameObject differenceInTimesUI; //displayes + or - in advantage

    public static float getTimerTime()
    {
        if(isRunning){
            currentTime = Time.time - timerStartTime;
        }
        return currentTime;;
    }

    // starts the timer
    public void startTimer()
    {
        timerStartTime = Time.time;
        isRunning = true;
    }

    // splits each section
    public void splitTimer()
    {
        currentTime = getTimerTime();
        float bestTime = float.Parse(bestSplits.splits[splitIndex].time);
        currentRun[splitIndex] = currentTime;

        float difference = currentTime - bestTime;

        if(bestTime == 0.00F || difference < 0 )
        {
            if(bestTime != 0.00F)
            {
                differenceInTimesUI.GetComponent<Text>().text = differenceInTimesUI.GetComponent<Text>().text + "<color=green> " + difference.ToString("n2") + "</color>\n"; 
            }
            bestSplits.splits[splitIndex].time = currentTime.ToString("n2");

        }
        else
        {
             differenceInTimesUI.GetComponent<Text>().text = differenceInTimesUI.GetComponent<Text>().text + "<color=red> +" + difference.ToString("n2") + "</color>\n";
        }
        splitIndex++;
        
        updateSplitsUI();
    }

    // stops the timer, prints out all splits recorded
    public void stopTimer(){
        isRunning = false;

        foreach(Split split in bestSplits.splits)
        {
            print(split.toString());
        }

        saveJSON();

    }

    public void updateSplitsUI(){
        SplitTimesUI.GetComponent<Text>().text = "";

        foreach(float split in currentRun)
        {
            SplitTimesUI.GetComponent<Text>().text = SplitTimesUI.GetComponent<Text>().text + split.ToString("n2") + "\n";
        }
    }


    // Start is called before the first frame update
    void Start()
    {
        filePath = Application.persistentDataPath + "/splits.json";
        
        
        if(File.Exists(filePath))
        {
            loadJSON();

        }

        foreach(Split split in bestSplits.splits)
        {
            SplitNamesUI.GetComponent<Text>().text = SplitNamesUI.GetComponent<Text>().text + split.name + "\n";
            SplitTimesUI.GetComponent<Text>().text = SplitTimesUI.GetComponent<Text>().text + split.time + "\n";
            currentRun.Add(float.Parse(split.time));
            
        }


    }

    // Update is called once per frame
    void Update()
    {
        if(isRunning){
            totalTime.GetComponent<Text>().text = getTimerTime().ToString("n2");
        }
    }

    public void saveJSON(){
        var toSave = JsonUtility.ToJson(bestSplits);
        File.WriteAllText(filePath, toSave);
    }

    public void loadJSON(){
        var inputString = File.ReadAllText(filePath);
        bestSplits = JsonUtility.FromJson<SplitList>(inputString);

        print("old splits");
        foreach(Split split in bestSplits.splits)
        {
            print(split.toString());
        }
    }

}




// class for splits in the timer
[Serializable]
public class Split{
    public string name; // name of the split
    public string time; // how long the split took

    // returns a string version of the object
    public string toString()
    { 
        return name + " : " + time;
    }
}

[Serializable]
public class SplitList{
    [ReorderableList]
    public List<Split> splits;
}

