using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using NaughtyAttributes;

public class LeverController : MonoBehaviour
{
    [BoxGroup("Settings")] public Transform pivot;
    [BoxGroup("Settings")] [MinMaxSlider(-180, 180)]public Vector2 onOffAngleRange = new Vector2(-45, 45);
    [BoxGroup("Settings")] public float switchDuration = 0.5f;
    [BoxGroup("Settings")] public AnimationCurve leverEasingCurve;
    [Space]
    [BoxGroup("Events")] public BoolUnityEvent onLeverChangedEvent;
    [BoxGroup("Events")] public BoolEvent onLeverChangedDelegate;
    [BoxGroup("Events")] public UnityEvent onLeverOnEvent;
    [BoxGroup("Events")] public VoidEvent onLeverOnDelegate;
    [BoxGroup("Events")] public UnityEvent onLeverOffEvent;
    [BoxGroup("Events")] public VoidEvent onLeverOffDelegate;

    private Interactable interactable;
    private bool isGoalOn = false;
    [ShowNonSerializedField]private bool isOn = false;
    private bool isAnimating;
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
        float progress = Mathf.Clamp(Time.time - flipTimestamp, 0, switchDuration) / switchDuration;
        
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
