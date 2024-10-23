using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{

    [SerializeField] Wave[] waves;
    public float minRange;
    public float maxRange;
    
    public float waveTimer;
    public float waveDuration;

    public int currentWave;
    public float spawnTimer;
    public float currentWaveSpawnDelay;

    public bool bossSpawner;







    // Start is called before the first frame update
    void Start()
    {
        //start timer for wave 1
        waveTimer = waveDuration;    
    }

    // Update is called once per frame
    void Update()
    {
        waveTimer -= 1 * Time.deltaTime;

        if (waveTimer < 0)
        {
            NextWave();
        }


        SpawnCooldown();



        /*
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
        */



    }

    void NextWave()
    {
        if (currentWave < waves.Length)
        {
            currentWave++;
        }
        waveTimer = waveDuration;

    }

    void SpawnCooldown()
    {
        spawnTimer -= 1 * Time.deltaTime;
        if (spawnTimer < 0 || bossSpawner) {

            spawnTimer = currentWaveSpawnDelay;
            SpawnEnemy();

        }
    }

    void SpawnEnemy()
    {
        Vector3 spawnPos = transform.position + (Vector3)(maxRange * Random.insideUnitSphere);
        spawnPos.y = transform.position.y;

        //Repeat until outside minimum range;
        while ((transform.position - spawnPos).magnitude < minRange)
        {


            spawnPos = transform.position + (Vector3)(maxRange * Random.insideUnitSphere);


        }


        //Instantiate(enemyPrefab, spawnPos, Quaternion.identity);
        
        

    }

    int calculateNumberOfEnemiesInCurrentWave()
    {

        int totalNumberOfEnemies = 0;
        foreach (WaveEnemyData enemies in waves[currentWave].enemiesInWave)
        {
            totalNumberOfEnemies += enemies.numberOfEnemies;
        }
        Debug.Log(totalNumberOfEnemies);
        return totalNumberOfEnemies;

    }

    void UpdateSpawnDelay()
    {
        currentWaveSpawnDelay = waveDuration / calculateNumberOfEnemiesInCurrentWave();
    }
}
