using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;

public class HealthControllerReferencer : MonoBehaviour
{
    [BoxGroup("References")]public HealthController healthController;
}
