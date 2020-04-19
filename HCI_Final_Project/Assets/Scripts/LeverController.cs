using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using NaughtyAttributes;

public class LeverController : MonoBehaviour
{
    [Tooltip("Transform to animate rotation")]
    [Required][BoxGroup("Settings")] public Transform pivot;

    [Tooltip("Values to animate pivot local rotation between")]
    [BoxGroup("Settings")] [MinMaxSlider(-180, 180)]public Vector2 onOffAngleRange = new Vector2(-45, 45);
    
    [Tooltip("Time in seconds for animation")]
    [BoxGroup("Settings")] public float switchDuration = 0.5f;
    
    [Tooltip("Easing curve for animation of pivot")]
    [BoxGroup("Settings")] public AnimationCurve leverEasingCurve;
    [Space]

    [Tooltip("UnityEvent called when lever is interacted with. Passes value of isOn as parameter. Delegate onLeverChangedDelegate also executed")]
    [BoxGroup("Events")] public BoolUnityEvent onLeverChangedEvent;
    [BoxGroup("Events")] public BoolDelegate onLeverChangedDelegate;

    [Tooltip("UnityEvent called when lever is turned on. Delegate onLeverOnDelegate also executed")]
    [BoxGroup("Events")] public UnityEvent onLeverOnEvent;
    [BoxGroup("Events")] public VoidDelegate onLeverOnDelegate;

    [Tooltip("UnityEvent called when lever is turned off. Delegate onLeverOffDelegate also executed")]
    [BoxGroup("Events")] public UnityEvent onLeverOffEvent;
    [BoxGroup("Events")] public VoidDelegate onLeverOffDelegate;


    [Tooltip("Status of lever")]
    [BoxGroup("DEBUG")][ReadOnly]public bool isOn = false;
    
    [Tooltip("Is lever currently animating")]
    [BoxGroup("DEBUG")][ReadOnly]public bool isAnimating;
    private Interactable interactable;
    private bool isGoalOn = false;
    private float flipTimestamp;
    private Quaternion onRotation;
    private Quaternion offRotation;
    
    void Start()
    {
        interactable = GetComponent<Interactable>();
        interactable.onInteractDelegate += onInteract;

        onRotation = Quaternion.Euler(0, 0, onOffAngleRange.x);
        offRotation = Quaternion.Euler(0, 0, onOffAngleRange.y);
    }

    void Update()
    {
        float progress = 1f - (Mathf.Clamp(flipTimestamp - Time.time, 0, switchDuration) / switchDuration);
        
        pivot.localRotation = Quaternion.Lerp(isGoalOn ? offRotation : onRotation, isGoalOn ? onRotation : offRotation, leverEasingCurve.Evaluate(progress));

        if(flipTimestamp < Time.time && isAnimating) {
            isOn = isGoalOn;
            isAnimating = false;

            onLeverChangedEvent.Invoke(isOn);
            if(onLeverChangedDelegate != null) {
                onLeverChangedDelegate(isOn);
            }

            if(isOn) {
                onLeverOnEvent.Invoke();
                if(onLeverOnDelegate != null) {
                    onLeverOnDelegate();
                }
            } else {
                onLeverOffEvent.Invoke();
                if(onLeverOffDelegate != null) {
                    onLeverOffDelegate();
                }
            }
        }
    }

    public void onInteract() {
        isGoalOn = !isGoalOn;
        isAnimating = true;
        flipTimestamp = Time.time + switchDuration;
    }
}
