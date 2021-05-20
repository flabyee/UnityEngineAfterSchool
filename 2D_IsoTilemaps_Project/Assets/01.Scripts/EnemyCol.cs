using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCol : MonoBehaviour
{
    Enemy my;
    
    private void Start()
    {
        my = transform.parent.GetComponent<Enemy>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("PlayerFireball"))
        {
            my.OnDamage();
            Destroy(collision.gameObject);
        }
    }
}
