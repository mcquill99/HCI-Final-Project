using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;
public class StatusManager : MonoBehaviour
{
    [Tooltip("Reference to HealthController for health statuses")]
    [BoxGroup("DEBUG")][ReadOnly]public HealthController healthController;

    [Tooltip("Reference to movementController for speed statuses")]
    [BoxGroup("DEBUG")][ReadOnly]public MovementController movementController;
    
    [Tooltip("List of current active statuses")]
    [BoxGroup("DEBUG")][ReadOnly]public List<Status> statuses;

    void Start()
    {
        statuses = new List<Status>();
        healthController = GetComponent<HealthControllerReferencer>().healthController;
        movementController = GetComponent<MovementController>();
    }

    void Update()
    {
        
        for(int i = statuses.Count - 1; i >= 0; i--) {
            Status s = statuses[i];
            if(s.duration + s.getTimeRecieved() < Time.time) {
                statuses.Remove(s);
            }
        }

        for(int i = statuses.Count - 1; i >= 0; i--) {
            Status s = statuses[i];
            s.EvaluateStatus(this);
        }

    }

    public void addStatus(Status status) {
        statuses.Add(status);
    }
}

[System.Serializable]
public abstract class Status {
    [Tooltip("Duration in seconds that the status lasts")]
    public float duration;
    private float timeRecieved;
    public void initStatus() {
        timeRecieved = Time.time;
    }
    public abstract void EvaluateStatus(StatusManager manager);
    public float getTimeRecieved() {
        return timeRecieved;
    }

    public abstract Status copyStatus();
}