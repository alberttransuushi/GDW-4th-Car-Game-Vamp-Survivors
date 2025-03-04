using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpinOut : MonoBehaviour
{
    List<GameObject> possibleTargets = new List<GameObject>();
    public GameObject[] enemyArray;
    [SerializeField] float maxRange;
    [SerializeField] float minRange;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        

    }

    void DoAbility()
    {
        enemyArray = GameObject.FindGameObjectsWithTag("Enemy");
        possibleTargets.Clear();
        foreach (GameObject enemy in enemyArray)
        {

            if (Vector3.Distance(enemy.transform.position, transform.position) <= maxRange && Vector3.Distance(enemy.transform.position, transform.position) >= minRange)
            {

                

            }

        }
    }
}
