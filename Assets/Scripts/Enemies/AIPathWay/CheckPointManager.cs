using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPointManager : MonoBehaviour
{
    public List<GameObject> targets;
    public CheckPointManager nextCheckPoint;
    public CheckPointManager prevCheckPoint;

    private void OnTriggerEnter(Collider other)
    {
        //Set Enemy Target to the Next CheckPoint
    }

}
