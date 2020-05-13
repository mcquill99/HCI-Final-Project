using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;
using UnityEngine.AI;
public class LegController : MonoBehaviour
{
    public CustomIK legIK;
    // public NavMeshAgent agent;
    public CreatureMovementController controller;
    public float legSegmentLength = 1f;
    public Transform endPlacementCaster;
    public Transform animatedGoalIK;
    public Transform proceduralGoalIK;

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
    public float ikInfluence = 1f;
    void Start()
    {
        currentPlacement = legIK.targetIK.position;
        goalPlacement = currentPlacement;
        UpdateBypass.instance.onUpdate += onUpdate;

    }

    void OnDisable() {
        UpdateBypass.instance.onUpdate += onUpdate;
    }

    void OnDestroy() {
        UpdateBypass.instance.onUpdate += onUpdate;
    }
    void onUpdate()
    {
        legIK.targetIK.position = Vector3.Lerp(animatedGoalIK.position, proceduralGoalIK.position, ikInfluence);

        // if(agent.isOnOffMeshLink) {
        //     proceduralGoalIK.position = legIK.rootBone.position + Vector3.down * legSegmentLength;
        //     return;
        // }

        RaycastHit hit;
        if(Physics.Raycast(endPlacementCaster.position + controller.getVelocity() * overStepAmount, endPlacementCaster.forward, out hit, 4 * legSegmentLength, 1<<9)) {
            casterPlacement = hit.point;
            isGrounded = true;
        } else {
            isGrounded = false;
        }

        if(isGrounded) {
            float progress = 1f - (Mathf.Clamp(stepTimestamp - Time.time, 0, stepDuration) / stepDuration);
            if(progress < 1) {
                Vector3 targetPosition = Vector3.Lerp(currentPlacement, goalPlacement, XZEasingCurve.Evaluate(progress));
                targetPosition.y += YEasingCurve.Evaluate(progress) * stepHeight;
                proceduralGoalIK.position = targetPosition;
            } else {
                currentPlacement = goalPlacement;
                proceduralGoalIK.position = currentPlacement;
            }

            if(Vector3.Distance(currentPlacement, casterPlacement) > strideLength && stepTimestamp + stepDuration < Time.time && stepCooldownTimestamp < Time.time) {
                
                stepTimestamp = Time.time + stepDuration;
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
        Debug.DrawLine(proceduralGoalIK.position, currentPlacement, Color.green);
        Debug.DrawLine(proceduralGoalIK.position, goalPlacement, Color.red);

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
