using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using NaughtyAttributes;
public class AIAnimationController : MonoBehaviour
{
    public Transform body;
    CreatureMovementController movementController;
    private NavMeshAgent agent;
    float turnSpeed = 0f;
    float moveSpeed = 3f;
    [Slider(0f, 1f)]public float ikInfluence = 1f;
    LegController[] legs;
    Animator animator;
    bool isGrounded;
    float fallSpeed = 0;

    IEnumerator Start()
    {
        legs = transform.GetComponentsInChildren<LegController>();
        animator = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
        movementController = GetComponent<CreatureMovementController>();
        moveSpeed = movementController.movementSpeed;
        agent.speed = 0;
        agent.angularSpeed = 0;

        while(true) {
            if(agent.isOnOffMeshLink) {
                animator.SetBool("isOffMeshLink", true);

                StartCoroutine(Parabola(agent, 1f, 1f));
                agent.CompleteOffMeshLink();
                animator.SetBool("isOffMeshLink", false);
    
            }
            yield return null;
        }
    }

    void Update()
    {
        bool tempGrounded = isGrounded;
        isGrounded = Physics.Raycast(body.position, Vector3.down, 1f, 1 | 1 << 9);
        if(!isGrounded && tempGrounded) {
            fallSpeed = 0;
        }
        if(!isGrounded) {
            fallSpeed += 0.981f * 0.03f;
        }
        animator.SetBool("isGrounded", isGrounded);
        animator.SetFloat("fallSpeed", fallSpeed);


        float turn = Vector3.Dot(transform.right, (agent.steeringTarget - transform.position));
        turn = Mathf.Abs(turn) < 0.2f ? 0 : Mathf.Sign(turn);
        turnSpeed = Mathf.Lerp(turnSpeed, turn, Time.deltaTime * moveSpeed);
        animator.SetFloat("moveSpeed", ((1 - (Mathf.Abs(Mathf.Clamp(turnSpeed, -1f, 1f))))));
        animator.SetFloat("turn", turnSpeed);

    }

    IEnumerator Parabola (NavMeshAgent agent, float height, float duration) {
        OffMeshLinkData data = agent.currentOffMeshLinkData;
        Vector3 startPos = agent.transform.position;
        Vector3 endPos = data.endPos + Vector3.up*agent.baseOffset;
        float normalizedTime = 0.0f;
        while (normalizedTime < 1.0f) {
            float yOffset = height * 4.0f*(normalizedTime - normalizedTime*normalizedTime);
            agent.transform.position = Vector3.Lerp (startPos, endPos, normalizedTime) + yOffset * Vector3.up;
            normalizedTime += Time.deltaTime / duration;
            yield return null;
        }
    }
}
