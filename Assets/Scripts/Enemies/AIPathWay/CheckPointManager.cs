using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPointManager : MonoBehaviour
{
    public List<GameObject> targets;
    public CheckPointManager nextCheckPoint;
    public CheckPointManager prevCheckPoint;
    public GameObject s;
    private void OnTriggerEnter(Collider other)
    {
        //Set Enemy Target to the Next CheckPoint
        if(other.tag == "Enemy")
        {
            Debug.Log("Enemy");
            other.gameObject.GetComponent<BaseEnemy>().SwitchTarget(nextCheckPoint.targets[other.gameObject.GetComponent<BaseEnemy>().targetIndex]);
            s = nextCheckPoint.targets[other.gameObject.GetComponent<BaseEnemy>().targetIndex];
        }
    }

}
