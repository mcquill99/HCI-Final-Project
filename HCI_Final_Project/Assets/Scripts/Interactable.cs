using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Interactable : MonoBehaviour
{
    public UnityEvent onInteractEvent;
    public VoidDelegate onInteractDelegate;
    public void Interact() {
        onInteractEvent.Invoke();
        if(onInteractDelegate != null) {
            onInteractDelegate();
        }
    }

}
