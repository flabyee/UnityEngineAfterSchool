using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraKeyboardController : MonoBehaviour
{
    public GameObject follwingTarget;
    public Vector3 offset;
    public float distance;
    private Vector3 relativePos;
    public float verticalAngle;
    public float horizontalAngle;

    void Start()
    {
        offset = new Vector3(0, 5, -5);
        verticalAngle = 45;
        distance = 10;
        relativePos = Quaternion.Euler(verticalAngle, horizontalAngle, 0) * new Vector3(0, 0, -distance);
    }

    void LateUpdate()
    {


        if (Input.GetKey(KeyCode.Q))
        {
            Rotate(-1);
        }
        else if (Input.GetKey(KeyCode.E))
        {
            Rotate(1);
        }
        else
        {
            Rotate(0);
        }
        
        transform.position = follwingTarget.transform.position + relativePos;
        transform.LookAt(follwingTarget.transform.position);
    }

    private void Rotate(float angle)
    {
        horizontalAngle += angle;
        relativePos = Quaternion.Euler(verticalAngle, horizontalAngle, 0) * new Vector3(0,0,-distance);
        //transform.RotateAround(follwingTarget.transform.position, Vector3.up, angle);
    }
}
