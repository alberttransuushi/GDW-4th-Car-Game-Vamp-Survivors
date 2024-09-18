using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RampAccelerator : MonoBehaviour
{
    [SerializeField] float rampAddedAcceleration;
    
    private void OnTriggerStay(Collider other)
    {
        print(other.gameObject.name);
        if (other.tag == "Player" || other.tag == "Enemy")
        {
            other.GetComponent<Rigidbody>().velocity += transform.right * rampAddedAcceleration;
        }
    }
}
