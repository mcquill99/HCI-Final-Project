using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using NaughtyAttributes;
public class DoorController : MonoBehaviour
{
    [Tooltip("Sections of the door to animate")]
    [BoxGroup("Settings")][ReorderableList]public List<DoorPiece> doorPieces;
    
    [Tooltip("NavMeshObstacle to enable/disable when door is 3/4 open")]
    [BoxGroup("Settings")]public GameObject navMeshObstacle;
    
    [Tooltip("Duration of door animation")]
    [BoxGroup("Settings")]public float doorAnimationDuration = 1f;
    
    [Tooltip("Animation curve for easing door animation")]
    [BoxGroup("Settings")]public AnimationCurve doorEasingCurve;
    
    [Tooltip("Does the door open automatically based on proximity")]
    [BoxGroup("Settings")]public bool isAutoDoor;
    [BoxGroup("Settings")][ShowIf("isAutoDoor")]public TriggerEvents autoDoorTrigger;

    [Tooltip("Sound to play when door animates")]
    [BoxGroup("Settings")]public AudioSource doorSound;

    [Space]
    [Tooltip("UnityEvent called when door is opened or closed. Passes parameter containing status of door. Delegate onDoorChangedDelegate also executed")]
    [BoxGroup("Events")] public BoolUnityEvent onDoorChangedEvent;
    [BoxGroup("Events")] public BoolDelegate onDoorChangedDelegate;

    [Tooltip("UnityEvent called when door is opened. Delegate onDoorOpenDelegate also executed")]
    [BoxGroup("Events")] public UnityEvent onDoorOpenEvent;
    [BoxGroup("Events")] public VoidDelegate onDoorOpenDelegate;

    [Tooltip("UnityEvent called when door is closed. Delegate onDoorClosedDelegate also executed")]
    [BoxGroup("Events")] public UnityEvent onDoorClosedEvent;
    [BoxGroup("Events")] public VoidDelegate onDoorClosedDelegate;

    private bool isOpen = false;
    private bool isGoalOpen = false;
    private bool isAnimating = false;
    private float doorAnimTimestamp;

    void Start() {
        setIsAutoDoor(isAutoDoor);
    }

    void Update()
    {
        float progress = 1f - (Mathf.Clamp(doorAnimTimestamp - Time.time, 0, doorAnimationDuration) / doorAnimationDuration);
        
        foreach(DoorPiece piece in doorPieces) {
            piece.doorTransform.position = Vector3.Lerp(isGoalOpen ? piece.closedTransform.position : piece.openTransform.position, isGoalOpen ? piece.openTransform.position : piece.closedTransform.position, doorEasingCurve.Evaluate(progress));
            piece.doorTransform.rotation = Quaternion.Lerp(isGoalOpen ? piece.closedTransform.rotation : piece.openTransform.rotation, isGoalOpen ? piece.openTransform.rotation : piece.closedTransform.rotation, doorEasingCurve.Evaluate(progress));
        }

        if(navMeshObstacle)
            navMeshObstacle.SetActive(isGoalOpen ? progress < 0.75f : progress > 0.25f);

        
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
        if(doorSound != null){
            AudioSource.PlayClipAtPoint(doorSound.clip, transform.position);
        }
        setDoorState(true);
    }

    public void closeDoor() {
        setDoorState(false);
    }

    public void toggleDoor() {
        setDoorState(!isOpen);
    }

    public void setIsAutoDoor(bool val) {
        isAutoDoor = val;
        if(isAutoDoor) {
            autoDoorTrigger.onTriggerEnterDelegate += openDoor;
            autoDoorTrigger.onTriggerExitDelegate += closeDoor;
        } else {
            autoDoorTrigger.onTriggerEnterDelegate -= openDoor;
            autoDoorTrigger.onTriggerExitDelegate -= closeDoor;
        }
    }

    public void setDoorState(bool isOpen) {
        if(isOpen != this.isOpen) {
            isAnimating = true;
            isGoalOpen = isOpen;
            doorAnimTimestamp = Time.time + doorAnimationDuration;
        } else {
            isAnimating = true;
            isGoalOpen = isOpen;
            doorAnimTimestamp = Time.time + (doorAnimationDuration - (doorAnimTimestamp - Time.time));
        }
    }

}

[System.Serializable]
public class DoorPiece {
    [Tooltip("Transform for doorTransform to animate to when door is open. Uses Position and Rotation only.")]
    [Required][BoxGroup("References")] public Transform openTransform;
    
    [Tooltip("Transform for doorTransform to animate to when door is closed. Uses Position and Rotation only.")]
    [Required][BoxGroup("References")] public Transform closedTransform;

    [Tooltip("Transform to animate")]
    [Required][BoxGroup("References")] public Transform doorTransform;
}