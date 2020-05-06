using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RadialMenuTilter : MonoBehaviour
{
    public float sensitivity = 5f;
    public float returnSpeed = 3f;
    public bool invert = false;
    public Transform offsetCursor;
    void Update()
    {
        Vector3 offset = (offsetCursor.position - transform.position).normalized;
        offset = offset * sensitivity;
        float temp = offset.x;
        offset.x = offset.y;
        offset.y = -1f * temp;
        offset = invert ? -1f * offset : offset;
        transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(offset), Time.deltaTime * returnSpeed);
    }
}
