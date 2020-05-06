using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIAnimationListener : MonoBehaviour
{
    public AIStateMachine stateMachine;
    private Animator animator;

    public void Start() {
        animator = GetComponent<Animator>();
        stateMachine.attackDelegate += attack;
    }

    public void attack() {
        animator.SetTrigger("leap");
    }

    public void doneAttacking() {
        stateMachine.attackFinished();
    }
}
