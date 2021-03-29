using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletMovement : MonoBehaviour
{
    
    public float speed = 10f;
    void Start()
    {
        
    }

    void Update()
    {
        transform.Translate(speed * Time.deltaTime, 0, 0);
        if(transform.position.x > 12f)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        DataManager.Instance.KillEnemy();
        Destroy(collision.gameObject);
        Destroy(gameObject);

    }
}
