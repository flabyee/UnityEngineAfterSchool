using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public GameObject BulletPrefab = null;

    public float speed = 5f;

    public float attackSpeed = 100;


    void Start()
    {
        StartCoroutine(Shoot());
    }

    void Update()
    {
        transform.Translate(0, speed * Time.deltaTime, 0);
        if(transform.position.y > 4.5)
        {
            speed *= -1f;
        }
        if(transform.position.y < -4.5)
        {
            speed *= -1f;
        }
    }

    IEnumerator Shoot()
    {
        while(true)
        {
            Instantiate(BulletPrefab, transform.position, Quaternion.identity);
            yield return new WaitForSeconds(attackSpeed);
        }
    }
}
