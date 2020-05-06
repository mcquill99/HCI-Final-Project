using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AtPatrolPointBehaviour : StateMachineBehaviour
{
    private float patrolPointWaitTime = 3f;
    private float stateEnterTime;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
       stateEnterTime = Time.time;
    }


    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
       if(stateEnterTime + patrolPointWaitTime < Time.time) {
           AIStateMachine sm = animator.GetComponent<AIStateMachine>();
           if(sm.agent.enabled) {
                sm.agent.SetDestination(AIGridGenerator.points[Random.Range(0, AIGridGenerator.points.Count)].point);
                animator.SetTrigger("navPointInvalid");
           }
       }
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    //override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    
    //}

    // OnStateMove is called right after Animator.OnAnimatorMove()
    //override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that processes and affects root motion
    //}

    // OnStateIK is called right after Animator.OnAnimatorIK()
    //override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that sets up animation IK (inverse kinematics)
    //}
}
