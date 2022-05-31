using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaycastAim : MonoBehaviour
{
    public WeaponController[] weapons;
    public WeaponController weapon;

    private Weapon currentWeapon;

    private Vector3 aimPosition;

    public Transform cameraAimTarget; 
    public LayerMask mouseColliderLayerMask;

    public Transform aimDebugPos;


    private Ray ray;
    private RaycastHit hitInfo;

    public GameObject aimTarget;

    //private Camera mainCam;
    private Cinemachine.CinemachineVirtualCamera cam;

    private bool started = false;
    private bool isMe = false;

    void Start()
    {
        //screenCenterPos = new Vector2(Screen.width / 2f, Screen.height / 2f);
    }

    // bullet
    public void StartGame(bool isMe, int listOrder, Cinemachine.CinemachineVirtualCamera cam)
    {
        this.isMe = isMe;
        this.cam = cam;

        //mainCam = Camera.main;
        //cameraAimTarget = mainCam.transform.GetChild(0);
        
        currentWeapon = Weapon.Handgun;
        SwapWeapon(Weapon.Handgun);
        weapon.SetListOrder(listOrder);
        started = true;
    }



    // Update is called once per frame
    void Update()
    {
        if (!started)
            return;

        Vector3 forward = cam.State.CorrectedOrientation * Vector3.forward;
        ray.origin = cam.gameObject.transform.position + cam.transform.forward * 6;
        ray.direction = forward;

        if (Physics.Raycast(ray, out hitInfo, 999f, mouseColliderLayerMask))
        {
            aimPosition = hitInfo.point;
            aimTarget.transform.position = aimPosition;
        }

        if (!isMe)
            return;

        if (Input.GetKeyUp(KeyCode.Alpha1))
        {
            SwapWeapon(Weapon.Handgun);
        }else if (Input.GetKeyUp(KeyCode.Alpha2))
        {
            SwapWeapon(Weapon.Rifle);
        }

        if (Input.GetKeyUp(KeyCode.Mouse0))
        {
            weapon.ShootByMe(aimPosition);
        }

    }

    private void SwapWeapon(Weapon weaponType)
    {
        if(weaponType == currentWeapon)
        {
            return;
        }
        weapons[(int)currentWeapon].gameObject.SetActive(false);

        weapons[(int)weaponType].gameObject.SetActive(true);

        //switch (weaponType)
        //{
        //    case Weapon.Handgun:
        //        break;
        //    case Weapon.Rifle:
        //        break;
        //}
        currentWeapon = weaponType;
        weapon = weapons[(int)currentWeapon];
        weapon.ChangeAmmoUI();
        //TODO: ±≥√º µÙ∑π¿Ã & Animation
    }

}
