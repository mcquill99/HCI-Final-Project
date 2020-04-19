using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using NaughtyAttributes;
public class InteractionController : MonoBehaviour
{

    [BoxGroup("Settings")]public float interactionDistance;

    [BoxGroup("Events")]public UnityEvent onInteractEvent;
    [BoxGroup("Events")]public VoidDelegate onInteractDelegate;


    void Update()
    {
        if(Input.GetKeyDown(KeyCode.E)) {
            RaycastHit hit;
            int layerMask = 1 | 1 << 9 | 1 << 10;
            if(Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit, interactionDistance, layerMask)) {
                onInteract();
                Interactable i = hit.collider.gameObject.GetComponent<Interactable>();
                if(i != null) {
                    i.Interact();
                }
            }
        }
    }

    public void onInteract() {
        onInteractEvent.Invoke();
        if(onInteractDelegate != null) {
            onInteractDelegate();
        }
    }

}
