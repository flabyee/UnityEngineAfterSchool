using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletMovement : MonoBehaviour
{
    public float speed = 10f;

    GameManager gameManager = null;

    void Start()
    {
        gameManager = FindObjectOfType<GameManager>();

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
        if(collision.tag == "Enemy")
        {
            DataManager.Instance.KillEnemy();


            gameManager.textLoad();
            Destroy(gameObject);
        }

    }
}
