using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurbineSpinner : MonoBehaviour
{
    public float speed;
    void Start()
    {
        
    }

    void Update()
    {
        transform.rotation = Quaternion.Euler(transform.eulerAngles + new Vector3(0, 0, speed * Time.deltaTime));
    }
}
