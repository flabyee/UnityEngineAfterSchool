using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAim : MonoBehaviour
{
    public PlayerController playerController;

    public float turnSpeed = 15;
    //private Camera mainCam;
    private Cinemachine.CinemachineVirtualCamera cam;
    public Transform followTarget;
    public GameObject playerRoot;

    private bool isAiming = false;
    public Animator animator;

    public float transitionSpeed = 0.1f;
    public Cinemachine.AxisState xAxis;
    public Cinemachine.AxisState yAxis;

    private bool isMe = false;

    void Start()
    {
    }

    public void StartGame(bool isMe, Cinemachine.CinemachineVirtualCamera cam)
    {
        this.isMe = isMe;
        this.cam = cam;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        //mainCam = Camera.main;

        SetInitRotation();
    }

    private void Update()
    {
        if(!isMe)
        {
            return;
        }

        if (Input.GetMouseButtonDown(1))
        {
            isAiming = true;
            animator.SetBool("IsAiming", true);
            playerController.OnPlayerAiming(true);
        }

        if (Input.GetMouseButtonUp(1))
        {
            isAiming = false;
            animator.SetBool("IsAiming", false);
            playerController.OnPlayerAiming(false);
        }
    }

    public void SetInitRotation()
    {
        Vector3 dir = Vector3.zero - transform.position;
        Quaternion lookRot = Quaternion.LookRotation(dir, Vector3.up);

        xAxis.Value = lookRot.eulerAngles.y;
        //yAxis.Value = lookRot.eulerAngles.x;
    }


    private void FixedUpdate()
    {
        if (isMe)
        {
            // Follow Target Rotate for Cam Rotate
            xAxis.Update(Time.fixedDeltaTime);
            yAxis.Update(Time.fixedDeltaTime);
        }

        Rotate();
    }

    private void Rotate()
    {
        followTarget.eulerAngles = new Vector3(yAxis.Value, xAxis.Value, 0);

        // Player Rotate
        //float camY = cam.transform.rotation.eulerAngles.y;
        playerRoot.transform.rotation = Quaternion.Slerp(playerRoot.transform.rotation, Quaternion.Euler(0, xAxis.Value, 0), turnSpeed * Time.fixedDeltaTime);
    }

    public void UpdateNetPlayerRotation(float x, float y)
    {
        xAxis.Value = x;
        yAxis.Value = y;
    }

    public bool IsAiming()
    {
        return isAiming;
    }

}
