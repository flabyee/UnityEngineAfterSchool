using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Animations.Rigging;

//[Serializable]
//public struct NetTransform
//{
//    public Position position;
//    public Rotation rotation;

//    [Serializable]
//    public struct Position
//    {
//        public float x;
//        public float y;
//        public float z;
//    }

//    [Serializable]
//    public struct Rotation
//    {
//        public float x;
//        public float y;
//        public float z;
//    }

    

//    public NetTransform(float x, float y, float z, float rx, float ry, float rz)
//    {
//        position.x = x;
//        position.y = y;
//        position.z = z;

//        rotation.x = rx;
//        rotation.y = ry;
//        rotation.z = rz;
//    }
//}

public class PlayerController : BaseCharacterController
{
    private float originSpeed;
    public GameObject rotateRoot;

    public Animator animator;
    public GameObject bloodEffectPref;
    public PlayerSkinManager skinManager;
    public RaycastAim raycastAim;
    public PlayerAim playerAim;
    public Cinemachine.CinemachineVirtualCamera cinemachineCam;

    private CharacterController controller;
    public Rig[] rigs;
    public bool dead = false;
    private bool started = false;
    public bool jumpTrigger = false;

    private float hitDelay = 0;

    private float invincibleTime = 0;
    private float invincibleDuration = 5;
    private int invincibleHitCondition = 3;
    private int invincibleHitCount = 0;

    private float transformDelay = 0.1f;

    private int listOrder = -1;

    private NetPacket jumpPacket;   // 값이 같아서 재사용가능

    protected override void Awake()
    {
        base.Awake();
        controller = GetComponent<CharacterController>();
    }

    protected override void Start()
    {
        base.Start();

        originSpeed = 20;
        speed = originSpeed;
        jumpForce = 20;
        gravity = 50f;

    }

    void Update()
    {
        if (UIManager.Instance.IsPaused())
        {
            return;
        }

        if (hitDelay > 0)
        {
            hitDelay -= Time.deltaTime;
        }

      
    }

    private void FixedUpdate()
    {
        if(!started)
        {
            return;
        }

        if (dead)
        {
            return;
        }

        moveDir = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));

        UpdateMoveDirection(rotateRoot.transform.TransformDirection(moveDir));

        if(Input.GetButton("Jump"))
        {
            jumpTrigger = true;
        }

        CharacterMove();
    }

    public override void UpdateMoveDirection(Vector3 dir)
    {
        moveDir = dir.normalized * speed;
    }

    public void CharacterMove()
    {
        if (!controller.isGrounded)
        {
            moveY -= gravity * Time.deltaTime;
        }
        else
        {
            moveY = 0;
        }

        if (jumpTrigger)
        {
            OnPlayerJump();
        }

        moveDir.y = moveY;

        controller.Move(moveDir * Time.deltaTime);

        animator.SetFloat("Speed", controller.velocity.magnitude);
    }

    public void StartCamera()
    {
        cinemachineCam.Priority++;
        cinemachineCam.enabled = true;
    }

    public void StartRotate(bool isMe, int _listOrder)
    {
        this.listOrder = _listOrder;

        raycastAim.StartGame(isMe, listOrder, cinemachineCam);
        playerAim.StartGame(isMe, cinemachineCam);
    }

    public void StartGame(bool isMe)
    {
        //raycastAim.StartGame(isMe, _listOrder, cinemachineCam);
        //playerAim.StartGame(isMe, cinemachineCam);

        started = true;


        SetHP();

        if(isMe)
        {
            jumpPacket = new NetPacket(NetProtocol.EVT_PLAYER_JUMP, listOrder);
            StartCoroutine(SendTransformCoroutine());
        }
        else
        {
            gameObject.tag = "NetPlayer";
            cinemachineCam.enabled = true;
            skinManager.SetEnemyColor();
        }

    }

    private IEnumerator SendTransformCoroutine()
    {
        while(!dead)
        {
            // Custom Structure
            // string[] : new string[2], 0: position string = "x:y:z", 1: rotoation "x:y:z"
            //NetTransform trans = new NetTransform(transform.position.x, transform.position.y, transform.position.z, rotateRoot.transform.eulerAngles.x, rotateRoot.transform.eulerAngles.y, rotateRoot.transform.eulerAngles.z);

            string[] trans = new string[2];
            trans[0] = $"{transform.position.x}:{transform.position.y}:{transform.position.z}";
            //trans[1] = $"{rotateRoot.transform.eulerAngles.x}:{rotateRoot.transform.eulerAngles.y}:{rotateRoot.transform.eulerAngles.z}";
            trans[1] = $"{playerAim.xAxis.Value}:{playerAim.yAxis.Value}";
            NetClient.Instance.SendTransform(trans);
            SendTransform();

            yield return new WaitForSeconds(transformDelay);
        }
    }

    public void SendTransform()
    {

    }

    public void OnPlayerAiming(bool isAiming)
    {

        speed = isAiming? originSpeed * 0.5f : originSpeed;
    }

    public override void OnPlayerJump()
    {
        if (controller.isGrounded)
        {
            moveY = jumpForce;
            NetClient.Instance.SendData(jumpPacket);
        }
        jumpTrigger = false;
    }

    public bool isDead()
    {
        return dead;
    }

    private void SetHP()
    {
        //maxHP = 20;
        hp = maxHP;

        UIManager.Instance.InitPlayerHP(maxHP);
    }

    public int GetListOrder()
    {
        return listOrder;
    }
    public void SetListOrder(int _listOrder)
    {
        listOrder = _listOrder;
    }
    
    //private void Rotate()
    //{
    //   transform.rotation = Quaternion.Euler(0,Camera.main.transform.rotation.eulerAngles.y, Camera.main.transform.rotation.eulerAngles.z);
    //}



    //public void OnHit(int attackPower, Vector3 enemyPosition)
    //{
    //    if (invincibleTime > 0)
    //    {
    //        Debug.Log("invincible");
    //        return;
    //    }

    //    hp -= attackPower;

    //    Vector3 enemyDir = enemyPosition - transform.position;
    //    Vector3 hitPos = enemyPosition - enemyDir*3/4;
        
    //    hitPos.y = 1;
    //    Instantiate(bloodEffectPref, hitPos, Quaternion.LookRotation(enemyDir,Vector3.up));

    //    OnMinusHP();

    //    invincibleHitCount++;
    //    if (invincibleHitCount == invincibleHitCondition)
    //    {
    //        invincibleHitCount = 0;
    //        invincibleTime = invincibleDuration;
    //        skinManager.StartInvincible();
    //        Invoke("StopInvincible", invincibleTime);
    //    }
    //}

    private void StopInvincible()
    {
        invincibleTime = 0;
        skinManager.StopInvincible();
    }

    public override void OnHit(int damage)
    {
        //if (hitDelay <= 0)
        {
            //hitDelay = 2;

            hp -= damage;

            OnMinusHP();
        }
    }

    public override void OnBleed(Vector3 hitPos)
    {
        Vector3 bloodDir = hitPos - transform.position;

        Instantiate(bloodEffectPref, hitPos, Quaternion.LookRotation(bloodDir, Vector3.up));
    }

    private void OnMinusHP()
    {

        if (hp < 0)
        {
            hp = 0;
        }

        UIManager.Instance.UpdatePlayerHP(hp);

        NetClient.Instance.SendData(new NetPacket(NetProtocol.EVT_PLAYER_HIT, hp, listOrder));

        if (hp == 0)
        {
            InGameManager.Instance.deadPlayerCount++;

            OnDead();

            NetClient.Instance.SendData(new NetPacket(NetProtocol.EVT_PLAYER_DEAD, listOrder));
            //UIManager.Instance.transitionManager.StartFadeOut();
        }
    }

    public void OnDead()
    {
        dead = true;
        controller.enabled = false;
        animator.SetBool("Dead", true);
        foreach(Rig rig in rigs)
        {
            rig.weight = 0;
        }

        //TODO: UI Off;
        //TODO: Fade Out

        InGameManager.Instance.CheckRoundEnd();
    }
}
