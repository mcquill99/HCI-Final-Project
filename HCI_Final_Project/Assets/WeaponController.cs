using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class WeaponController : MonoBehaviour
{
    public GameObject weaponVisual;
    public Transform unequipedPoint;
    public Transform equipedPoint;
    private bool isEquiped;
    public AnimationCurve equipAnimCurve;
    public float equipAnimDuration = 0.5f;
    private float equipAnimTimeStamp;

    public UnityEvent onFireStartEvent;
    public UnityEvent onFireStayEvent;
    public UnityEvent onFireStopEvent;

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
        float progress = Mathf.Clamp(Time.time - equipAnimTimeStamp, 0, equipAnimDuration) / equipAnimDuration;
        weaponVisual.transform.position = Vector3.Lerp(unequipedPoint.position, equipedPoint.position, progress);
        weaponVisual.transform.rotation = Quaternion.Lerp(unequipedPoint.rotation, equipedPoint.rotation, progress);
        if(progress == 1f) {
            isEquiped = true;
        }

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

    void onFireStart() {
        onFireStartEvent.Invoke();
    }

    void onFireStay() {
        onFireStayEvent.Invoke();
    }

    void onFireStop() {
        onFireStopEvent.Invoke();
    }
}
