using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using NaughtyAttributes;

public class ButtonController : MonoBehaviour
{
    [Tooltip("Amount of time in seconds for entire animation")]
    [BoxGroup("Settings")] public float pressDuration = 0.5f;
    
    [Tooltip("Transform to animate")]
    [BoxGroup("Settings")][Required] public Transform button;
    
    [Tooltip("Start and end positions of Button Transform's local z position")]
    [BoxGroup("Settings")] [MinMaxSlider(0, 1f)]public Vector2 upDownHeight = new Vector2(0.1f, 0.3f);
    
    [Tooltip("Easing curve used to modify animation")]
    [BoxGroup("Settings")] public AnimationCurve buttonEasingCurve;
    
    [Tooltip("UnityEvent called when button finishes animation. Delegate onButtonPressedDelegate also executed")]
    [BoxGroup("Events")] public UnityEvent onButtonPressedEvent;
    [BoxGroup("Events")] public VoidDelegate onButtonPressedDelegate;
    
    [Tooltip("Whether or not button is currently animating")]
    [BoxGroup("DEBUG")][ReadOnly]public bool isAnimating;
    
    private Interactable interactable;
    private float pressTimeStamp;
    private Vector3 pressedPosition;
    private Vector3 unpressedPosition;



    void Start()
    {
        interactable = GetComponent<Interactable>();
        interactable.onInteractDelegate += onInteract;

        pressedPosition = new Vector3(0, 0, upDownHeight.x);
        unpressedPosition = new Vector3(0, 0, upDownHeight.y);
    }

    void Update()
    {
        float progress = 1f - (Mathf.Clamp(pressTimeStamp - Time.time, 0, pressDuration) / pressDuration);
        
        button.localPosition = Vector3.Lerp(unpressedPosition, pressedPosition, buttonEasingCurve.Evaluate(progress));

        if(pressTimeStamp < Time.time && isAnimating) {
            isAnimating = false;

            onButtonPressedEvent.Invoke();
            if(onButtonPressedDelegate != null) {
                    onButtonPressedDelegate();
            }
        }

    }

    public void onInteract() {
        isAnimating = true;
        pressTimeStamp = Time.time + pressDuration;
    }
}
