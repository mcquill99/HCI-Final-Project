using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using NaughtyAttributes;
public class TriggerEvents : MonoBehaviour
{
    [Tooltip("Event only executes if trigger collider matches this LayerMask")]
    [BoxGroup("Settings")]public LayerMask layerMask = ~0;

    [Tooltip("UnityEvent called when collider enters trigger. Delegate onTriggerEnterDelegate also executed")]
    [BoxGroup("Events")]public UnityEvent onTriggerEnterEvent;
    [BoxGroup("Events")]public VoidDelegate onTriggerEnterDelegate;

    [Tooltip("UnityEvent called when collider stays in trigger. Delegate onTriggerStayDelegate also executed")]
    [BoxGroup("Events")]public UnityEvent onTriggerStayEvent;
    [BoxGroup("Events")]public VoidDelegate onTriggerStayDelegate;

    [Tooltip("UnityEvent called when collider exits trigger. Delegate onTriggerExitDelegate also executed")]
    [BoxGroup("Events")]public UnityEvent onTriggerExitEvent;
    [BoxGroup("Events")]public VoidDelegate onTriggerExitDelegate;

    void OnTriggerEnter(Collider collider) {
        if(!this.enabled){
            return;
        }
        if((layerMask & (1 << collider.gameObject.layer)) != 0) {
            onTriggerEnterEvent.Invoke();
            if(onTriggerEnterDelegate != null) {
                onTriggerEnterDelegate();
            }
        }
    }

    void OnTriggerStay(Collider collider) {
        if(!this.enabled){
            return;
        }
        if((layerMask & (1 << collider.gameObject.layer)) != 0) {
            onTriggerStayEvent.Invoke();
            if(onTriggerStayDelegate != null) {
                onTriggerStayDelegate();
            }
        }
    }

    void OnTriggerExit(Collider collider) {
        if(!this.enabled){
            return;
        }
        if((layerMask & (1 << collider.gameObject.layer)) != 0) {
            onTriggerExitEvent.Invoke();
            if(onTriggerExitDelegate != null) {
                onTriggerExitDelegate();
            }
        }
    }
}
