using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public GameObject BulletPrefab = null;

    public float speed = 5f;

    public float attackSpeed = 100f;


    void Start()
    {
        StartCoroutine(Shoot());
        attackSpeed = 1f;
    }

    void Update()
    {
        attackSpeed = 1f - ((float)(DataManager.Instance.attackSpeedLevel) / 10);
        transform.Translate(0, speed * Time.deltaTime, 0);
        if(transform.position.y > 4.5)
        {
            StartCoroutine(turn());
        }
        if(transform.position.y < -4.5)
        {
            StartCoroutine(turn());
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

    IEnumerator turn()
    {
        
        speed *= -1f;
        yield return new WaitForSeconds(0.1f);
    }
}
