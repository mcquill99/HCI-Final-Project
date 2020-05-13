using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public enum AICombatState {Initializing, Patrolling, Aggressive}

public class AIStateMachine : MonoBehaviour
{
    public NavMeshAgent agent;
    public Transform target;
    public AttackToken token;
    public VoidDelegate attackDelegate;
    public float attackCooldown;
    private float attackTimestamp;
    private VoidDelegate attackFinishedDelegate;
    public CombatStateDelegate combatStateChangedDelegate;
    private AICombatState currentCombatState = AICombatState.Initializing;

    public void Start() {
        //TODO: Not this;
        target = GameObject.Find("Player").transform;
        if(currentCombatState == AICombatState.Initializing) {
            setCombatState(AICombatState.Patrolling);
            agent.SetDestination(AIGridGenerator.points[Random.Range(0, AIGridGenerator.points.Count)].point);
        }
    }

    void Update() {
        Debug.DrawLine(transform.position, agent.destination, Color.magenta);
    }

    public void OnDestroy() {
        if(token != null) {
            AIMasterController.instance.returnToken(token);
        }
    }

    public bool requestToken(TokenType type) {
        if(attackTimestamp >= Time.time) {
            return false;
        }
        return AIMasterController.instance.requestToken(type, out token);
    }

    public void returnToken() {
        AIMasterController.instance.returnToken(token);
        token = null;
    }

    public void attack(VoidDelegate callback) {
        attackTimestamp = Time.time + attackCooldown;
        attackFinishedDelegate += callback;
        attackDelegate?.Invoke();
    }

    public void attackFinished() {
        attackFinishedDelegate?.Invoke();
        attackFinishedDelegate = null;
    }

    public void setCombatState(AICombatState newState) {
        currentCombatState = newState;

        if(combatStateChangedDelegate != null) {
            combatStateChangedDelegate(newState);
        }
    }

}
