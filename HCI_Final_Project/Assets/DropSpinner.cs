using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropSpinner : MonoBehaviour
{
    public float speed;
    void Start()
    {
        
    }

    void Update()
    {
        transform.eulerAngles += new Vector3(0, Time.deltaTime * speed, 0);
    }
}
