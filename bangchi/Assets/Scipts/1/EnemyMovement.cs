using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    public float speed = -10f;

    private SpriteRenderer rd = null;

    
    // Start is called before the first frame update
    void Awake()
    {
        rd = GetComponent<SpriteRenderer>();
        rd.color = new Color(Random.value, Random.value, Random.value, 1f);
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(speed * Time.deltaTime, 0, 0);
        if(transform.position.x < -12)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Destroy(gameObject);
        if(collision.tag == "bullet")
        {
            Destroy(collision.gameObject);
        }
    }
}
