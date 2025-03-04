using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissileLauncher : MonoBehaviour
{
    public GameObject player;
    public GameObject missilePrefab;
    public GameObject missileSpawner;
    public List<GameObject> targetList = new List<GameObject>();
    public GameObject[] enemyArray;
    public List<GameObject> validTargets = new List<GameObject>();

    [SerializeField, Range(0,75)] float lockOnAngle;
    [SerializeField] float minRange;
    [SerializeField] float maxRange;

    [SerializeField, Range(1, 5)] int numberOfMissiles;

    
    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");

    }

    
    // Update is called once per frame
    void Update()
    {
        enemyArray = GameObject.FindGameObjectsWithTag("Enemy");
        validTargets.Clear();
        foreach(GameObject enemy in enemyArray)
        {

            if (Vector3.Angle(Vector3.Normalize(enemy.transform.position - transform.position),transform.forward) <= lockOnAngle && Vector3.Distance(enemy.transform.position, transform.position) <= maxRange && Vector3.Distance(enemy.transform.position, transform.position) >= minRange)
            {
                validTargets.Add(enemy);
            }

        }


        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            FireMissiles();
        }


  
    }

    void FireMissiles()
    {
        for(int i = 0; i < numberOfMissiles; i++)
        {
            GameObject spawnedMissiles = Instantiate(missilePrefab, missileSpawner.transform.position, missileSpawner.transform.rotation);
            spawnedMissiles.GetComponent<MissileProjectile>().target = validTargets[i];
        }
    }
}
