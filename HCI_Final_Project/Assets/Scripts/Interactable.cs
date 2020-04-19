using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using NaughtyAttributes;

public class Interactable : MonoBehaviour
{
    [BoxGroup("Events")]public UnityEvent onInteractEvent;
    [BoxGroup("Events")]public VoidDelegate onInteractDelegate;
    public void Interact() {
        onInteractEvent.Invoke();
        if(onInteractDelegate != null) {
            onInteractDelegate();
        }
    }

}
