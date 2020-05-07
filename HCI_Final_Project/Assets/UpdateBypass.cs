using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpdateBypass : MonoBehaviour
{
    public static UpdateBypass instance;
    public VoidDelegate onUpdate;

    void Awake() {
        instance = this;
    }

    void Update()
    {
        onUpdate?.Invoke();
    }
}
