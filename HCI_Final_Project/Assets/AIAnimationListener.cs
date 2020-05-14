using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIAnimationListener : MonoBehaviour
{
    public AIStateMachine stateMachine;
    private Animator animator;
    public AudioSource leapSound;

    public void Start() {
        animator = GetComponent<Animator>();
        stateMachine.attackDelegate += attack;
    }

    public void attack() {
        animator.SetTrigger("leap");
        if(leapSound != null){
            AudioSource.PlayClipAtPoint(leapSound.clip, transform.position);
        }
    }

    public void doneAttacking() {
        stateMachine.attackFinished();
    }
}
