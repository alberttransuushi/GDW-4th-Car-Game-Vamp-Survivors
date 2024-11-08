using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RampAccelerator : MonoBehaviour
{
    [SerializeField] float rampAddedAcceleration;
    [SerializeField] float maxSpeed;
    private void OnTriggerStay(Collider other)
    {
        //print(other.gameObject.name);
        if (other.tag == "Player" || other.tag == "Enemy")
        {
            if (other.GetComponent<Rigidbody>().velocity.magnitude < maxSpeed)
            {
                other.GetComponent<Rigidbody>().velocity += other.transform.forward * rampAddedAcceleration;
                
            }
            
        }
    }
}
