﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using NaughtyAttributes;
public class TriggerEvents : MonoBehaviour
{
    [BoxGroup("Settings")]public LayerMask layerMask = ~0;
    [BoxGroup("Events")]public UnityEvent onTriggerEnterEvent;
    [BoxGroup("Events")]public VoidDelegate onTriggerEnterDelegate;
    [BoxGroup("Events")]public UnityEvent onTriggerStayEvent;
    [BoxGroup("Events")]public VoidDelegate onTriggerStayDelegate;
    [BoxGroup("Events")]public UnityEvent onTriggerExitEvent;
    [BoxGroup("Events")]public VoidDelegate onTriggerExitDelegate;

    void OnTriggerEnter(Collider collider) {
        if((layerMask & (1 << collider.gameObject.layer)) != 0) {
            onTriggerEnterEvent.Invoke();
            if(onTriggerEnterDelegate != null) {
                onTriggerEnterDelegate();
            }
        }
    }

    void OnTriggerStay(Collider collider) {
        if((layerMask & (1 << collider.gameObject.layer)) != 0) {
            onTriggerStayEvent.Invoke();
            if(onTriggerStayDelegate != null) {
                onTriggerStayDelegate();
            }
        }
    }

    void OnTriggerExit(Collider collider) {
        if((layerMask & (1 << collider.gameObject.layer)) != 0) {
            onTriggerExitEvent.Invoke();
            if(onTriggerExitDelegate != null) {
                onTriggerExitDelegate();
            }
        }
    }
}
