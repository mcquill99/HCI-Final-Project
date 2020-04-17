﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PathfindingTestController : MonoBehaviour
{
    public GameObject target;
    private NavMeshAgent agent;
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    void Update()
    {
        if(Vector3.SqrMagnitude(transform.position - target.transform.position) < 9) {
            agent.SetDestination(transform.position);
        } else {
            agent.SetDestination(target.transform.position);
        }
    }
}