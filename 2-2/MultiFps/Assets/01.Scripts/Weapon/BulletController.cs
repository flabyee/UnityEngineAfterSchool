using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour
{
    public Weapon weaponType;

    public BulletData bulletData;

    private Rigidbody rb;

    private int damage;

    private Vector3 origin;

    public int listOrder = -1;

    private void Awake()
    {
        origin = transform.position;
        rb = GetComponent<Rigidbody>();
        transform.Rotate(90, 0, 0);
    }

    void Start()
    {
        switch (weaponType)
        {
            case Weapon.Handgun:
                damage = 2;
                break;
            case Weapon.Rifle:
                damage = 1;
                break;
            default:
                damage = 1;
                break;
        }
        //Debug.Log(bulletData.maxDistance);
        rb.AddForce(transform.up * bulletData.bulletSpeed);

    }

    // Update is called once per frame
    void Update()
    {

        if(Vector3.Distance(origin, transform.position) > bulletData.maxDistance)
        {
            Destroy(gameObject);
        }

    }

    public int GetDamage()
    {
        return damage;
    }

    private void OnTriggerEnter(Collider other)
    {
        switch (other.gameObject.tag)
        {
            // zombie
            //case "EnemyHit":
            //    {
            //        EnemyController ec = other.gameObject.GetComponent<EnemyController>();
            //        int score = ec.OnHit(damage);

            //        if (score > 0)
            //        {
            //            UIManager.Instance.ChangeScore(score);
            //        }
            //    }
            //    break;
            case "Bullet":
                return;//Destroy 안하기 위해서
            case "PlayerHit":  // 내가 맞았을 때
                PlayerController pc = other.transform.parent.gameObject.GetComponent<PlayerController>();
                if(pc.GetListOrder() == listOrder)
                {
                    return;
                }
                // other : my hit collider, transform.position : bullet position
                pc.OnBleed(other.ClosestPointOnBounds(transform.position));
                pc.OnHit(damage);
                break;
            case "NetPlayerHit":
                NetPlayerController netPc = other.transform.parent.gameObject.GetComponent<NetPlayerController>();
                netPc.OnBleed(other.ClosestPointOnBounds(transform.position));

                if (netPc.GetListOrder() == listOrder)
                {
                    return;
                }
                break;
            default:
                break;
        }

        Destroy(gameObject);
    }
}
