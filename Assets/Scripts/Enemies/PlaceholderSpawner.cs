using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaceholderSpawner : MonoBehaviour
{

    public GameObject enemyPrefab;
    public float minRange;
    public float maxRange;
    public float timer;
    public float spawnDelay;




    // Start is called before the first frame update
    void Start()
    {
            
    }

    // Update is called once per frame
    void Update()
    {
        timer -= 1 * Time.deltaTime;
        while(timer <= 0)
        {
            timer = spawnDelay;


            Vector3 spawnPos = transform.position + (Vector3)(maxRange * Random.insideUnitSphere);
            spawnPos.y = 3;

            if ((transform.position - spawnPos).magnitude > minRange)
            {
                
                Instantiate(enemyPrefab, spawnPos, Quaternion.identity);
                Instantiate(enemyPrefab, spawnPos, Quaternion.identity);
                Instantiate(enemyPrefab, spawnPos, Quaternion.identity);
                Instantiate(enemyPrefab, spawnPos, Quaternion.identity);
                timer = spawnDelay;
            }


        }
    }
}
