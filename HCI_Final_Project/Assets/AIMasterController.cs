using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;
public class AIMasterController : MonoBehaviour
{
    public static AIMasterController instance;
    
    public int availableMeleeTokens = 3;
    public float tokenCooldown = 2f;
    
    [ReadOnly]public int currentTokenQueueSize = 0;
    private Queue<AttackToken> meleeTokens;    


    void Awake() {
        instance = this;

        meleeTokens = new Queue<AttackToken>(availableMeleeTokens);
        for(int i = 0; i < availableMeleeTokens; i++) {
            meleeTokens.Enqueue(new AttackToken(TokenType.Melee, tokenCooldown));
        }
        currentTokenQueueSize = meleeTokens.Count;
    }

    public bool requestToken(TokenType type, out AttackToken token) {
        token = null;
        if(type == TokenType.Melee) {
            if(meleeTokens.Count > 0 && meleeTokens.Peek().isCooledDown()) {
                token = meleeTokens.Dequeue();
                currentTokenQueueSize = meleeTokens.Count;

                return true;
            }
        }
        return false;
    }

    public void returnToken(AttackToken token) {
        if(token == null) return;

        if(token.GetTokenType() == TokenType.Melee) {
            token.useToken();
            meleeTokens.Enqueue(token);
            currentTokenQueueSize = meleeTokens.Count;
        
        }
    }
}

public enum TokenType {Melee, Ranged}

public class AttackToken {
    private float timestamp;
    private float cooldown;
    private TokenType type;

    public AttackToken(TokenType type, float cooldown) {
        this.type = type;
        this.cooldown = cooldown;
    }

    public bool isCooledDown() {
        return timestamp < Time.time;
    }

    public void useToken() {
        timestamp = Time.time + cooldown;
    }

    public TokenType GetTokenType() {
        return type;
    }
}