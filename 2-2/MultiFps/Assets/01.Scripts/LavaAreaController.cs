using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LavaAreaController : MonoBehaviour
{
    private void OnCollisionStay(Collision collision)
    {
       
    }

    private void OnTriggerStay(Collider other)
    {
        //Debug.Log("TriggerStay");
        if (other.gameObject.tag == "Player")
        {
            PlayerController pc = other.gameObject.GetComponent<PlayerController>();
            pc.OnHit(1);
        }
    }

}
