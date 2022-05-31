using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private GameManager gameManager = null;
    public GameObject BulletPrefab = null;

    public float speed = 5f;

    private void Awake()
    {
        
    }

    void Start()
    {
        gameManager = FindObjectOfType<GameManager>();

        
    }

    void Update()
    {
        
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

    
}
