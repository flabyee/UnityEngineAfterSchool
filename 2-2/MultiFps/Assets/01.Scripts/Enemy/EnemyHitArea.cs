using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHitArea : MonoBehaviour
{
    private EnemyController ec;
    private void Start()
    {
        ec = GetComponentInParent<EnemyController>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Bullet"))
        {
            BulletController bc = other.GetComponent<BulletController>();

            int score = ec.OnHit(bc.GetDamage());

            if (score > 0)
            {
                UIManager.Instance.ChangeScore(score);
            }
        }
    }
}
