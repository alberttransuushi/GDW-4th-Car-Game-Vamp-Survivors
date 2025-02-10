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
            //Debug.Log("Enemy");
            if (!other.gameObject.GetComponent<BaseEnemy>().targetingPlayer)
            {

                other.gameObject.GetComponent<BaseEnemy>().SwitchTarget(nextCheckPoint.targets[other.gameObject.GetComponent<BaseEnemy>().targetIndex]);

                RandomizeTargetIndex(other.gameObject.GetComponent<BaseEnemy>());

            }

            //s = nextCheckPoint.targets[other.gameObject.GetComponent<BaseEnemy>().targetIndex];
        }
    }


    public void RandomizeTargetIndex(BaseEnemy enemy)
    {
        // 1-2 Down, 3-8 Straight, 9-10 Up
        int direction = Random.Range(1, 21);
        if (enemy.targetIndex == 0 && (direction < 2 || direction > 19)) 
        {
            enemy.targetIndex++;
        } else if (enemy.targetIndex == 4 && (direction < 2 || direction > 19))
        {
            enemy.targetIndex--;
        } else
        {
            if (direction < 2)
            {
                enemy.targetIndex--;
            }
            if (direction > 19)
            {
                enemy.targetIndex++;
            }

        }

    }
}
