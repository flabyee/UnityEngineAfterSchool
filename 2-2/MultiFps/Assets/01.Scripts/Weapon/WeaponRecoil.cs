using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponRecoil : MonoBehaviour
{
    public PlayerAim playerAim;
    public float verticalRecoil = 1f;
    public float horizontalRecoil = 1f;

    private float recoilConstant = 1;

    public Cinemachine.CinemachineImpulseSource impulse;

    public void GenerateRecoil()
    {
        if (playerAim.IsAiming())
        {
            return;
        }
        //else
        //{
        //    recoilConstant = 1;
        //}


        float randomX = Random.Range(-horizontalRecoil, horizontalRecoil);

        impulse.GenerateImpulse(Camera.main.transform.forward);

        playerAim.yAxis.Value -= verticalRecoil * recoilConstant;
        playerAim.xAxis.Value += randomX * recoilConstant;
    }

    internal void SetRecoilConstant(float c)
    {
        recoilConstant = c;
    }

}
