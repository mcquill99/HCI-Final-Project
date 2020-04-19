using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class LegWalkerController : MonoBehaviour
{
    public Transform body;
    private NavMeshAgent agent;
    public float offsetHeight = 0.3f;
    CustomIK[] legs;
    void Start()
    {
        legs = body.GetComponentsInChildren<CustomIK>();
        agent = GetComponent<NavMeshAgent>();
    }

    void Update()
    {
        

        Vector3 goalPos = Vector3.zero; //transform.position;
        foreach(CustomIK t in legs) {
            goalPos += t.endBone.position;
        }
        goalPos /= legs.Length;
        goalPos += Vector3.up * offsetHeight;

        goalPos.x = body.position.x;
        goalPos.z = body.position.z;


        RaycastHit hit;
        if(Physics.Raycast(transform.position + Vector3.up, Vector3.down, out hit, 5f, 1<<9)) {
            //body.position = Vector3.Lerp(body.position, goalPos, Time.deltaTime * 12f);
        }
    }
}
