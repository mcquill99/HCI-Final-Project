using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using NaughtyAttributes;

public class PathfindingTestController : MonoBehaviour
{
    [Tooltip("Target to navigate towards")]
    [BoxGroup("Settings")]public GameObject target;
    private NavMeshAgent agent;
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        target = GameObject.Find("Player");
    }

    void Update()
    {
        // if(Vector3.SqrMagnitude(transform.position - target.transform.position) < 9) {
        //     agent.SetDestination(transform.position);
        // } else {
        //     agent.SetDestination(target.transform.position);
        // }
    }
}
