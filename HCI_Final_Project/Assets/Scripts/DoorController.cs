using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using NaughtyAttributes;
public class DoorController : MonoBehaviour
{
    [BoxGroup("Settings")][ReorderableList]public List<DoorPiece> doorPieces;
    [BoxGroup("Settings")]public float doorAnimationDuration = 1f;
    [BoxGroup("Settings")]public AnimationCurve doorEasingCurve;

    [Space]
    [BoxGroup("Events")] public BoolUnityEvent onDoorChangedEvent;
    [BoxGroup("Events")] public BoolDelegate onDoorChangedDelegate;
    [BoxGroup("Events")] public UnityEvent onDoorOpenEvent;
    [BoxGroup("Events")] public VoidDelegate onDoorOpenDelegate;
    [BoxGroup("Events")] public UnityEvent onDoorClosedEvent;
    [BoxGroup("Events")] public VoidDelegate onDoorClosedDelegate;

    private bool isOpen = false;
    private bool isGoalOpen = false;
    private bool isAnimating = false;
    private float doorAnimTimestamp;

    void Update()
    {
        float progress = 1f - (Mathf.Clamp(doorAnimTimestamp - Time.time, 0, doorAnimationDuration) / doorAnimationDuration);
        
        foreach(DoorPiece piece in doorPieces) {
            piece.doorTransform.position = Vector3.Lerp(isGoalOpen ? piece.closedTransform.position : piece.openTransform.position, isGoalOpen ? piece.openTransform.position : piece.closedTransform.position, doorEasingCurve.Evaluate(progress));
            piece.doorTransform.rotation = Quaternion.Lerp(isGoalOpen ? piece.closedTransform.rotation : piece.openTransform.rotation, isGoalOpen ? piece.openTransform.rotation : piece.closedTransform.rotation, doorEasingCurve.Evaluate(progress));
        }
        
        if(doorAnimTimestamp < Time.time && isAnimating) {
            isOpen = isGoalOpen;
            isAnimating = false;

            onDoorChangedEvent.Invoke(isOpen);
            if(onDoorChangedDelegate != null) {
                onDoorChangedDelegate(isOpen);
            }

            if(isOpen) {
                onDoorOpenEvent.Invoke();
                if(onDoorOpenDelegate != null) {
                    onDoorOpenDelegate();
                }
            } else {
                onDoorClosedEvent.Invoke();
                if(onDoorClosedDelegate != null) {
                    onDoorClosedDelegate();
                }
            }
        }
    }

    public void openDoor() {
        setDoorState(true);
    }

    public void closeDoor() {
        setDoorState(false);
    }

    public void toggleDoor() {
        setDoorState(!isOpen);
    }

    public void setDoorState(bool isOpen) {
        if(isOpen != this.isOpen) {
            isAnimating = true;
            isGoalOpen = isOpen;
            doorAnimTimestamp = Time.time + doorAnimationDuration;
        }
    }

}

[System.Serializable]
public class DoorPiece {
    [BoxGroup("References")] public Transform openTransform;
    [BoxGroup("References")] public Transform closedTransform;
    [BoxGroup("References")] public Transform doorTransform;
}