using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using NaughtyAttributes;

public class Interactable : MonoBehaviour
{
    [Tooltip("UnityEvent called when Interactable is interacted with. Delegate onInteractDelegate also executed")]
    [BoxGroup("Events")]public UnityEvent onInteractEvent;
    [BoxGroup("Events")]public VoidDelegate onInteractDelegate;
    public void Interact() {
        if(this.enabled){
            onInteractEvent.Invoke();
            if(onInteractDelegate != null) {
                onInteractDelegate();
            }
        }
    }

}
