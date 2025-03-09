using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SpinOutWallCrash : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("WALL1");
        if (collision.transform.tag == "Enemy")
        {
            Debug.Log("WALL");
            if(collision.transform.GetComponent<LandEnemy>().spinOut == true)
            {
                Destroy(collision.transform.gameObject);
            }
        }
    }
    private void OnCollisionStay(Collision collision)
    {
        Debug.Log("WALL1");
        if (collision.transform.tag == "Enemy")
        {
            Debug.Log("WALL");
            if (collision.transform.GetComponent<LandEnemy>().spinOut == true)
            {
                Destroy(collision.transform.gameObject);
            }
        }
    }
}

