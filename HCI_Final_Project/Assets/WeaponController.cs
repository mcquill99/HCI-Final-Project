using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using NaughtyAttributes;

public class WeaponController : MonoBehaviour
{
    [BoxGroup("References")] public GameObject weaponVisual;
    [BoxGroup("References")] public Transform unequipedPoint;
    [BoxGroup("References")] public Transform equipedPoint;

    [Space]

    [BoxGroup("Settings")] public float equipAnimDuration = 0.5f; //duration of the equip animation
    [BoxGroup("Settings")] public AnimationCurve equipAnimCurve;  //Easing curve for equip animation
    
    [Space]

    [BoxGroup("Events")] public UnityEvent onFireStartEvent;   //Called once when fire key is pressed down
    [BoxGroup("Events")] public UnityEvent onFireStayEvent;    //Called continuously while fire key is pressed down
    [BoxGroup("Events")] public UnityEvent onFireStopEvent;    //Called once when fire key is released
    
    [Space]

    [BoxGroup("DEBUG")][ShowNonSerializedField] private bool isEquiped; //whether or not the weapon can fire
    private float equipAnimTimeStamp; //represents time that weapon was equiped. Is used for animation
    
    

    //Called when weapon is equiped
    void OnEnable() {
        equipAnimTimeStamp = Time.time;
        weaponVisual.transform.position = unequipedPoint.transform.position;
        weaponVisual.transform.rotation = unequipedPoint.transform.rotation;
    }

    //Called when weapon is unequiped
    void OnDisable() {
        isEquiped = false;
        weaponVisual.transform.position = unequipedPoint.transform.position;
        weaponVisual.transform.rotation = unequipedPoint.transform.rotation;
        
    }

    void Start()
    {
        
    }

    void Update()
    {
        //Animate weapon position and rotation
        float progress = Mathf.Clamp(Time.time - equipAnimTimeStamp, 0, equipAnimDuration) / equipAnimDuration;
        weaponVisual.transform.position = Vector3.Lerp(unequipedPoint.position, equipedPoint.position, equipAnimCurve.Evaluate(progress));
        weaponVisual.transform.rotation = Quaternion.Lerp(unequipedPoint.rotation, equipedPoint.rotation, equipAnimCurve.Evaluate(progress));
        
        //If animation is finished, count weapon as equiped
        if(progress == 1f) {
            isEquiped = true;
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
    }

    //Called continuously while fire key is pressed down
    void onFireStay() {
        onFireStayEvent.Invoke();
    }

    //Called once when fire key is released
    void onFireStop() {
        onFireStopEvent.Invoke();
    }
}
