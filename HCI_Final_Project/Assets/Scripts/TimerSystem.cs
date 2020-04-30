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

    [ReorderableList]public List<Split> splits = new List<Split>(); //current splits
    public SplitList bestSplits; // list of all splits

    private static string filePath; //file path to save to

    public Text totalTime; //text object to display time to complete
    public Text SplitNamesUI; //names of the splits that appear on screen
    public Text SplitTimesUI; //times for each split that appear on screen
    public Text differenceInTimesUI; //displayes + or - in advantage

    public static float getTimerTime()
    {
        if(isRunning){
            currentTime = Time.time - timerStartTime;
        }
        return currentTime;
    }

    // starts the timer
    public void startTimer()
    {
        timerStartTime = Time.time;
        isRunning = true;
        splitIndex = 0;
        differenceInTimesUI.text = "";
    }

    // splits each section
    public void splitTimer(int splitNumber)
    {
        if(!isRunning) {
            Debug.LogWarning("Attempting to start split " + splitNumber + " without timer running.");
            return;
        }
        
        if(splitNumber != splitIndex + 1) {
            Debug.LogWarning("Attempting to start split " + splitNumber + " out of order");
            return;
        }

        if(splitNumber > splits.Count) {
            Debug.LogError("Attempting to start split " + splitNumber + ". This exceeds the current level's split amount of " + splits.Count);
            return;
        }

        splitIndex = splitNumber - 1;

        currentTime = getTimerTime();
        float bestTime = bestSplits.splits[splitIndex].time;
        splits[splitIndex].time = currentTime;

        float difference = currentTime - bestTime;

        if(bestTime == 0.00F || difference < 0 )
        {
            if(bestTime != 0.00F)
            {
                differenceInTimesUI.text = differenceInTimesUI.text + "<color=green> " + difference.ToString("n2") + "</color>\n"; 
            }
            bestSplits.splits[splitIndex].time = currentTime;

        }
        else
        {
             differenceInTimesUI.text = differenceInTimesUI.text + "<color=red> +" + difference.ToString("n2") + "</color>\n";
        }
        splitIndex++;
        
        updateSplitsUI();
    }

    // stops the timer, prints out all splits recorded
    public void stopTimer(){
        if(splitIndex != splits.Count) {
            Debug.LogWarning("Stopping timer before last split started");
        }


        isRunning = false;

        foreach(Split split in bestSplits.splits)
        {
            print(split.toString());
        }

        saveJSON();

    }

    public void updateSplitsUI(){
        SplitTimesUI.text = "";

        foreach(Split split in splits)
        {
            SplitTimesUI.text = SplitTimesUI.text + split.time.ToString("n2") + "\n";
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

        string namesText = "";
        string timesText = "";

        for(int i = 0; i < splits.Count; i++)
        {
            Split s = splits[i];
            namesText = namesText + s.name + "\n";
            timesText = timesText + s.time + "\n";
            // splits.Add(bestSplits.splits[i].time);
            
        }

        differenceInTimesUI.text = "";
        SplitNamesUI.text = namesText;
        SplitTimesUI.text = timesText;
        updateSplitsUI();
    }

    // Update is called once per frame
    void Update()
    {
        if(isRunning){
            totalTime.text = getTimerTime().ToString("n2");
        }
    }

    public void saveJSON(){
        print("Saving splits to: " + filePath);
        var toSave = JsonUtility.ToJson(bestSplits);
        File.WriteAllText(filePath, toSave);
    }

    public void loadJSON(){
        var inputString = File.ReadAllText(filePath);
        bestSplits = JsonUtility.FromJson<SplitList>(inputString);

        print("Loading splits from: " + filePath);
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
    public float time; // how long the split took

    // returns a string version of the object
    public string toString()
    { 
        return name + " : " + time.ToString("n2");
    }
}

[Serializable]
public class SplitList{
    [ReorderableList]
    public List<Split> splits;
}

