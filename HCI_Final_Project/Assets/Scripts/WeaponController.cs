using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using NaughtyAttributes;

public class WeaponController : MonoBehaviour
{
    [Tooltip("Reference to weapon visual for animation")]
    [Required][BoxGroup("References")] public GameObject weaponVisual;

    [Tooltip("Reference to unequiped transform for animation. Uses position and rotation")]
    [Required][BoxGroup("References")] public Transform unequipedPoint;

    [Tooltip("Reference to equiped transform for animation. Uses position and rotation")]
    [Required][BoxGroup("References")] public Transform equipedPoint;
    
    //TODO: Fix this to not be specific to player
    [Tooltip("Reference to Movement Controller. Used for tracking velocity")]
    [Required][BoxGroup("References")] public MovementController movementController;

    [Space]

    [Tooltip("Duration of time in seconds for the equip animation")]
    [BoxGroup("Settings")] public float equipAnimDuration = 0.5f; //duration of the equip animation

    [Tooltip("Easing curve for equip animation used for both position and rotation")]
    [BoxGroup("Settings")] public AnimationCurve equipAnimCurve;  //Easing curve for equip animation
    
    [Space]

    [Tooltip("UnityEvent called when fire key is pressed down. Delegate onFireStartDelegate also executed")]
    [BoxGroup("Events")] public UnityEvent onFireStartEvent;   //Called once when fire key is pressed down

    [Tooltip("UnityEvent called when fire key is held down. Delegate onFireStayDelegate also executed")]
    [BoxGroup("Events")] public UnityEvent onFireStayEvent;    //Called continuously while fire key is pressed down

    [Tooltip("UnityEvent called when fire key is released. Delegate onFireStopDelegate also executed")]
    [BoxGroup("Events")] public UnityEvent onFireStopEvent;    //Called once when fire key is released
    public VoidDelegate onFireStartDelegate;
    public VoidDelegate onFireStayDelegate;
    public VoidDelegate onFireStopDelegate;    

    [Space]

    [Tooltip("Is weapon able to fire/done with animation")]
    [BoxGroup("DEBUG")][ShowNonSerializedField] public bool isEquiped; //whether or not the weapon can fire
    private float equipAnimTimeStamp; //represents time that weapon was equiped. Is used for animation
    
    

    //Called when weapon is equiped
    void OnEnable() {
        //equipAnimTimeStamp = Time.time + equipAnimDuration;
        //weaponVisual.transform.position = unequipedPoint.transform.position;
        //weaponVisual.transform.rotation = unequipedPoint.transform.rotation;
        //WeaponAnimationSyncController.instance.addAnimator(gameObject.GetComponentInChildren<Animator>());
        //WeaponAnimationSyncController.instance.setBool("isEquiped", true);
    }

    //Called when weapon is unequiped
    void OnDisable() {
        //isEquiped = false;
        //weaponVisual.transform.position = unequipedPoint.transform.position;
        //weaponVisual.transform.rotation = unequipedPoint.transform.rotation;
        //WeaponAnimationSyncController.instance.setBool("isEquiped", false);

    }

    void Start()
    {
        
    }

    void Update()
    {
        //Animate weapon position and rotation
        //float progress = 1f - (Mathf.Clamp(equipAnimTimeStamp - Time.time, 0, equipAnimDuration) / equipAnimDuration);
        //weaponVisual.transform.position = Vector3.Lerp(unequipedPoint.position, equipedPoint.position, equipAnimCurve.Evaluate(progress));
        //weaponVisual.transform.rotation = Quaternion.Lerp(unequipedPoint.rotation, equipedPoint.rotation, equipAnimCurve.Evaluate(progress));
        
        //If animation is finished, count weapon as equiped
        // if(progress == 1f) {
        //     isEquiped = true;
        // }

        if(weaponVisual) {
            weaponVisual.SetActive(isEquiped);

            // Quaternion currRot = weaponVisual.transform.parent.localRotation;
            

            // weaponVisual.transform.parent.localRotation = Quaternion.Lerp(weaponVisual.transform.parent.localRotation, Quaternion.Euler(-1 * movementController.getVelocity()), Time.deltaTime * 5f);
        }



        //if weapon is equiped, it can be used
        if(isEquiped) {
            if(Input.GetKeyDown(KeyCode.Mouse0)) {
                onFireStart();
            }
            else if(Input.GetKeyUp(KeyCode.Mouse0)) {
                onFireStop();
            }
            else if(Input.GetKey(KeyCode.Mouse0)) {
                onFireStay();
            }

        }
    }

    //Called once when fire key is pressed down
    void onFireStart() {
        onFireStartEvent.Invoke();
        if(onFireStartDelegate != null) {
            onFireStartDelegate();
        }
    }

    //Called continuously while fire key is pressed down
    void onFireStay() {
        onFireStayEvent.Invoke();
        if(onFireStayDelegate != null) {
            onFireStayDelegate();
        }
    }

    //Called once when fire key is released
    void onFireStop() {
        onFireStopEvent.Invoke();
        if(onFireStopDelegate != null) {
            onFireStopDelegate();
        }
    }

    public bool getIsEquiped() {
        return isEquiped && WeaponAnimationSyncController.instance.getAnimationEquiped();
    }
}
