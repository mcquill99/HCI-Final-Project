using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ViewTilter : MonoBehaviour
{
    public float sensitivity = 5f;
    public float returnSpeed = 3f;
    public float magnitudeClamp = 2f;

    void Update() {
        Vector3 offset = new Vector3(Input.GetAxis("Mouse Y") * sensitivity, -1f * Input.GetAxis("Mouse X") * sensitivity, 0);
        offset = Vector3.ClampMagnitude(offset , magnitudeClamp* Time.timeScale);
        transform.rotation *= Quaternion.Euler(offset);
        transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.identity, Time.deltaTime * returnSpeed);
    }
}
