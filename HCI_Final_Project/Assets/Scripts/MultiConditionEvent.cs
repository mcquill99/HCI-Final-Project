using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using NaughtyAttributes;
public class MultiConditionEvent : MonoBehaviour
{
    [Tooltip("List of Game Conditions that all must be met before triggering an event.")]
    [BoxGroup("Settings")][ReorderableList]public List<GameCondition> conditionList;
    [Tooltip("UnityEvent called when all conditions have been set to 'True'.")]
    [BoxGroup("Events")]public UnityEvent onConditionsMetEvent;
    
    public bool conditionsMet = false;
    private int conditionListSize;
    // Start is called before the first frame update
    void Start()
    {
        conditionListSize = conditionList.Count;
    }

    public void flipCondition(int index){
        if(index < 0 || index > conditionListSize){
            Debug.LogWarning("Hey, you provided an invalid condition index: " + index + ". Allowable indexes are between 0 and " + conditionListSize + ".");
            return;
        }
        conditionList[index].setTo(true);
        checkIfAllMet();
    }

    void checkIfAllMet(){
        for(int index = 0; index < conditionListSize; index++){
            if(!conditionList[index].isMet()){
                return;
            }
        }
        this.onConditionsMet();
        this.enabled = false;
    }

    public void onConditionsMet(){
        onConditionsMetEvent.Invoke();
        Debug.Log("ALL CONDITIONS MET");
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}

[System.Serializable]
public class GameCondition{
    [Tooltip("Whether or not this current in-game condition has been met.")]
    public bool conditionMet = false;

    public void setTo(bool isMet){
        conditionMet = isMet;
    }

    public bool isMet(){
        return conditionMet;
    }
}
