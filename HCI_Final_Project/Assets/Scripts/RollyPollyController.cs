using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class RollyPollyController : MonoBehaviour
{
    public NavMeshAgent agent;
    public Transform ball;

    public float turnSpeed = 2f;
    void Start()
    {
        
    }

    void Update()
    {
        agent.velocity = agent.desiredVelocity * Mathf.Clamp((Vector3.Cross(transform.right, agent.desiredVelocity).magnitude / 10f), 0, 1);
        
        Vector3 localVelocity = transform.rotation * agent.velocity;
        print(localVelocity);
        ball.RotateAround(ball.position, ball.right, Mathf.Abs(localVelocity.z));
    }
}
