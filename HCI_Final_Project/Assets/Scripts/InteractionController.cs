using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using NaughtyAttributes;
public class InteractionController : MonoBehaviour
{
    [Tooltip("Maximum distance that Interactables can be interacted with")]
    [BoxGroup("Settings")]public float interactionDistance;

    [Tooltip("UnityEvent called when Interactable is interacted with. Delegate onInteractDelegate also executed")]
    [BoxGroup("Events")]public UnityEvent onInteractEvent;
    [BoxGroup("Events")]public VoidDelegate onInteractDelegate;


    void Update()
    {
        if(Input.GetKeyDown(KeyCode.E)) {
            RaycastHit hit;
            int layerMask = 1 | 1 << 9 | 1 << 10;
            if(Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit, interactionDistance, layerMask)) {
                Interactable i = hit.collider.gameObject.GetComponent<Interactable>();
                if(i != null) {
                    onInteract();
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
