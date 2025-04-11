using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillBox : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        if(collision.transform.tag == "Player")
        {
            collision.gameObject.GetComponent<PlayerCar>().TakeDamage(99999);
        }else Destroy(collision.gameObject);
    }
}
