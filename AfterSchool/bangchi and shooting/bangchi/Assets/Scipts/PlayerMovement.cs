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
        attackSpeed = (1f / Mathf.Sqrt(DataManager.Instance.attackSpeedLevel));
        transform.Translate(0, speed * Time.deltaTime, 0);
        if(transform.position.y > 4.5 && (speed > 0))
        {
            speed = -speed;
        }
        if(transform.position.y < -4.5 && (speed < 0))
        {
            speed = -speed;
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

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Enemy")
        {

            DataManager.Instance.StageLevelReset();
        }
    }
}
