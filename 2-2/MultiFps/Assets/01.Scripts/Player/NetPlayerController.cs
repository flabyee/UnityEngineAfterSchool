using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NetPlayerController : PlayerController
{
    private Slider hpHUD;

    protected override void Start()
    {
        Debug.Log("net start");
        base.Start();
        AddHUD();
    }

    private void FixedUpdate()
    {
        CharacterMove();
    }
    private void LateUpdate()
    {
        UpdateHUDPosition();
    }


    public override void OnHit(int _hp)
    {
        this.hp = _hp;

        hpHUD.value = (float)hp / maxHP;

        if (!hpHUD.gameObject.activeSelf)
        {
            hpHUD.gameObject.SetActive(true);
        }

        //if(hp == 0)
        //{
        //    OnDead();
        //}
        // ToDo: HUD update
    }

    private void AddHUD()
    {
        hpHUD = UIManager.Instance.AddEnemyHUD().GetComponent<Slider>();
        hpHUD.gameObject.SetActive(false);
    }
    private void UpdateHUDPosition()
    {
        UIManager.Instance.UpdateHUDPosition(hpHUD.gameObject, transform.position + transform.up * 2);
    }

    public override void OnPlayerJump()
    {
        moveY = jumpForce;
        jumpTrigger = false;
    }

}
