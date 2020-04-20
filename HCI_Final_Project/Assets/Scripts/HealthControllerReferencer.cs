using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;

public class HealthControllerReferencer : MonoBehaviour
{
    [Tooltip("Reference to health controller that collider is attached to")]
    [BoxGroup("References")]public HealthController healthController;
}
