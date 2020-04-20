using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class AIGridPoint {
    [SerializeField]
    
    public Vector3 point;
    public float sqrProximityToPlayer;

    public AIGridPoint(Vector3 point) {
        this.point = point;
    }

    public Vector3 getPoint() {
        return point;
    }
}

[System.Serializable]
public struct SearializableAIGridPoint {
    public float pointX;
    public float pointY;
    public float pointZ;
}