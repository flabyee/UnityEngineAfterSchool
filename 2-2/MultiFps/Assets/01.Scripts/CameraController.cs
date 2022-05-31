using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public GameObject follwingTarget;
    //public Vector3 offset;
    public float distance;
    private Vector3 relativePos;
    public Vector3 lookOffset;
    
    public float verticalAngle;
    public float horizontalAngle;

    public float rotateSensitivity;
    public float rotateYSensitivity;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;

        //offset = new Vector3(0, 5, -5);
        verticalAngle = 45;
        distance = 5;
        rotateSensitivity = 8;
        rotateYSensitivity = 1.5f;
        lookOffset = new Vector3(0.5f, 1.5f, 0);
        relativePos = Quaternion.Euler(verticalAngle, horizontalAngle, 0) * new Vector3(0, 0, -distance);
    }

    void LateUpdate()
    {
        if (UIManager.Instance.IsPaused())
        {
            return;
        }

        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = -Input.GetAxis("Mouse Y");

        Debug.Log(distance);


        if (Input.GetKey(KeyCode.Mouse1))
        {
            distance = 3;
        }
        else
        {
            distance = 5;

        }

        Rotate(mouseX*rotateSensitivity, mouseY * rotateYSensitivity);

        transform.position = follwingTarget.transform.position + relativePos;
        transform.LookAt(follwingTarget.transform.position + transform.TransformDirection(lookOffset));
    }

    private void Rotate(float x,float y)
    {
        horizontalAngle += x;
        verticalAngle += y;
        relativePos = Quaternion.Euler(Mathf.Clamp(verticalAngle, -45, 80), horizontalAngle, 0) * new Vector3(0,0,-distance);
        //transform.RotateAround(follwingTarget.transform.position, Vector3.up, angle);
    }
}
