using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;
using UnityEngine.AI;
public class LegController : MonoBehaviour
{
    public CustomIK legIK;
    public NavMeshAgent agent;
    public float legSegmentLength = 1f;
    public Transform endPlacementCaster;

    public AnimationCurve XZEasingCurve;
    public AnimationCurve YEasingCurve;
    public float stepDuration = 1f;
    public float strideLength = 1f;
    public float stepHeight = 0.5f;
    public float overStepAmount = 1.75f;
    [MinMaxSlider(0, 100)]public Vector2 stepCooldown = new Vector2(0.3f, 0.6f);
    private float stepCooldownTimestamp;
    private float stepTimestamp;

    private bool isGrounded = false;
    private Vector3 casterPlacement;
    private Vector3 goalPlacement;
    private Vector3 currentPlacement;
    void Start()
    {
        currentPlacement = legIK.targetIK.position;
        goalPlacement = currentPlacement;
    }

    void Update()
    {
        if(agent.isOnOffMeshLink) {
            legIK.targetIK.position = legIK.rootBone.position + Vector3.down * legSegmentLength;
            return;
        }

        RaycastHit hit;
        if(Physics.Raycast(endPlacementCaster.position + agent.velocity * overStepAmount, endPlacementCaster.forward, out hit, 4 * legSegmentLength, 1<<9)) {
            casterPlacement = hit.point;
            isGrounded = true;
        } else {
            isGrounded = false;
        }

        if(isGrounded) {
            float progress = Mathf.Clamp(Time.time - stepTimestamp, 0, stepDuration) / stepDuration;
            if(progress < 1) {
                Vector3 targetPosition = Vector3.Lerp(currentPlacement, goalPlacement, XZEasingCurve.Evaluate(progress));
                targetPosition.y += YEasingCurve.Evaluate(progress) * stepHeight;
                legIK.targetIK.position = targetPosition;
            } else {
                currentPlacement = goalPlacement;
                legIK.targetIK.position = currentPlacement;
            }

            if(Vector3.Distance(currentPlacement, casterPlacement) > strideLength && stepTimestamp + stepDuration < Time.time && stepCooldownTimestamp < Time.time) {
                stepTimestamp = Time.time;
                stepCooldownTimestamp = Time.time + Random.Range(stepCooldown.x, stepCooldown.y);
                //currentPlacement = goalPlacement;
                goalPlacement = casterPlacement;
                // Vector3 newGoalPlacement = (casterPlacement - currentPlacement) * overStepAmount;
                // Vector2 rand = Random.insideUnitCircle * (1 - overStepAmount);
                // newGoalPlacement += new Vector3(rand.x, 0, rand.y);
                // goalPlacement = currentPlacement + newGoalPlacement;
            }
        }

        Debug.DrawLine(endPlacementCaster.position, casterPlacement, Color.cyan);
        Debug.DrawLine(legIK.targetIK.position, currentPlacement, Color.green);
        Debug.DrawLine(legIK.targetIK.position, goalPlacement, Color.red);

    }


    [Button("Reset Leg")]
    public void resetLeg() {
        legIK.isEnabled = false;
        legIK.transform.rotation = Quaternion.identity;
        legIK.rootBone.rotation = Quaternion.identity;
        legIK.elbowBone.rotation = Quaternion.identity;
        legIK.elbowBone.position = legIK.rootBone.position + legIK.rootBone.forward * legSegmentLength;
        legIK.endBone.rotation = Quaternion.identity;
        legIK.endBone.position = legIK.elbowBone.position + legIK.elbowBone.forward * legSegmentLength;
    }

    [Button("Enable Leg IK")]
    public void enableLeg() {
        legIK.isEnabled = true;
    }
}
