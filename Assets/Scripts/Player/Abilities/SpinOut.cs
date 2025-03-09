using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpinOut : MonoBehaviour
{
    //List<GameObject> possibleTargets = new List<GameObject>();
    public GameObject[] enemyArray;
    [SerializeField] float maxRange;
    [SerializeField] float minRange;

    [SerializeField] float cooldown;
    [SerializeField] float cooldownCountdown;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        cooldownCountdown -= Time.deltaTime;
        if (cooldownCountdown <= 0) {
            if (Input.GetKeyDown(KeyCode.Mouse1))
            {
                DoAbility();
            }
        }



    }

    void DoAbility()
    {
        enemyArray = GameObject.FindGameObjectsWithTag("Enemy");
        //possibleTargets.Clear();
        foreach (GameObject enemy in enemyArray)
        {

            if (Vector3.Distance(enemy.transform.position, transform.position) <= maxRange && Vector3.Distance(enemy.transform.position, transform.position) >= minRange)
            {
                LandEnemy landEnemy = enemy.GetComponent<LandEnemy>();

                landEnemy.StartSpinOut();
            }

        }
    }
}
