using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using NaughtyAttributes;
public class LegWalkerController : MonoBehaviour
{
    public Transform body;
    private NavMeshAgent agent;
    public AudioSource walkSound;
    public float offsetHeight = 0.3f;
    float turnSpeed = 0f;
    public float moveSpeed = 3f;
    float leapTimestamp = 0;
    [Slider(0f, 1f)]public float ikInfluence = 1f;
    LegController[] legs;
    Animator animator;
    bool isGrounded;
    float fallSpeed = 0;
    private float length;

    private static float time;
    private static float currentTime;

    public static float getTime()
    {
        currentTime = Time.time - time;
        return currentTime;
    }

    void Start()
    {
        legs = transform.GetComponentsInChildren<LegController>();
        UpdateBypass.instance.onUpdate += onUpdate;
        if(walkSound != null){
            length = walkSound.clip.length;
        }
        time = Time.time;
    }

    void onUpdate()
    {


        Vector3 goalPos = Vector3.zero; //transform.position;
        foreach(LegController t in legs) {
            t.ikInfluence = ikInfluence;
            goalPos += t.legIK.endBone.position;
        }
        goalPos /= legs.Length;
        goalPos += Vector3.up * offsetHeight;

        goalPos.x = body.position.x;
        goalPos.z = body.position.z;
        if(walkSound != null && getTime() >= length ){
            walkSound.pitch = Random.Range(0.85f, 1.15f);
            time = Time.time;
            AudioSource.PlayClipAtPoint(walkSound.clip, transform.position);
        }
    }


}
