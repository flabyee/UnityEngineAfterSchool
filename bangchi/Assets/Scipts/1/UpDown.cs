using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpDown : MonoBehaviour
{
    public float speed = 5f;

    void Start()
    {
        //StartCoroutine(updown());
 
    }

    void Update()
    {
        transform.Translate(0, speed * Time.deltaTime, 0);
        if(transform.position.y > 5)
        {
            speed *= -1f;
        }
        else if(transform.position.y < -5)
        {
            speed *= -1f;
        }
    }
    IEnumerator updown()
    {
        while(true)
        {
            transform.Translate(0, 3, 0);
            yield return new WaitForSeconds(0.1f);
            transform.Translate(0, -3, 0);
            yield return new WaitForSeconds(0.1f);
        }
        
    }
}
