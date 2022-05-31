using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Weapon
{
    Handgun = 0,
    Rifle = 1,
}

public class WeaponController : MonoBehaviour
{
    public Weapon weaponType;
    public WeaponRecoil recoil;
    public Animator animator;

    public Transform bulletSpawn;

    private bool isReloading = false;

    private int currentAmmo;
    private int maxAmmo;

    public ParticleSystem muzzleEffect;
    public GameObject bulletPref;

    private NetClient netClient;

    public int listOrder = -1;

    void Start()
    {
        switch (weaponType)
        {
            case Weapon.Handgun:
                maxAmmo = 100;
                recoil.SetRecoilConstant(1);
                break;
            case Weapon.Rifle:
                maxAmmo = 500;
                recoil.SetRecoilConstant(0f);
                break;
            default:
                break;
        }

        currentAmmo = maxAmmo;
        UIManager.Instance.SetMaxAmmo(maxAmmo);

        netClient = NetClient.Instance;
    }

    private void Update()
    {

    }

    public void SetListOrder(int listOrder)
    {
        this.listOrder = listOrder;
    }

    public void ShootByMe(Vector3 aimPos)
    {
        if (isReloading)
        {
            return;
        }

        currentAmmo--;

        UIManager.Instance.ChangeCurrentAmmo(currentAmmo);

        Vector3 shootDiretion = aimPos - bulletSpawn.position;
        // 0 : bulletSpawnPos, 1 : direction, 2 : currentAmmo + listorder
        string[] shootInfo = new string[3];
        shootInfo[0] = $"{bulletSpawn.position.x}:{bulletSpawn.position.y}:{bulletSpawn.position.z}";
        shootInfo[1] = $"{shootDiretion.x}:{shootDiretion.y}:{shootDiretion.z}";
        shootInfo[2] = $"{currentAmmo}:{netClient.listOrder}";

        netClient.SendShoot(shootInfo);

        ShootBullet(shootDiretion, bulletSpawn.position);
    }

    public void ShootByOther(Vector3 shootDirection, Vector3 bulletPos, int currentAmmo)
    {
        this.currentAmmo = currentAmmo;

        ShootBullet(shootDirection, bulletPos);
    }

    private void ShootBullet(Vector3 shootDirection, Vector3 bulletPos)
    {
        if (currentAmmo == 0)
        {
            isReloading = true;
            Invoke("Reload", 3f);
        }

        animator.SetTrigger("Shoot");

        muzzleEffect.Play();
        Instantiate(bulletPref, bulletPos, Quaternion.LookRotation(shootDirection));
        bulletPref.GetComponent<BulletController>().listOrder = listOrder;

        recoil.GenerateRecoil();
    }


    private void Reload()
    {
        isReloading = false;
        currentAmmo = maxAmmo;
        UIManager.Instance.ChangeCurrentAmmo(currentAmmo);
    }

    internal void ChangeAmmoUI()
    {
        UIManager.Instance.SetMaxAmmo(maxAmmo);
        UIManager.Instance.ChangeCurrentAmmo(currentAmmo);
    }

}
