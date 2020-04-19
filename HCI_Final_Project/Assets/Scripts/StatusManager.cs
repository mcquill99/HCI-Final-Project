using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;
public class StatusManager : MonoBehaviour
{
    [ReadOnly]public HealthController healthController;
    [ReadOnly]public MovementController movementController;
    
    [ReadOnly]public List<Status> statuses;

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