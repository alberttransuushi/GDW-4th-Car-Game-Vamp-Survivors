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
    public List<WaveEnemyData> currentWaveData;
    public List<int> currentWaveEnemyCount;


    




    // Start is called before the first frame update
    void Start()
    {
        //CopyWaveDataToRunTime();

        //start timer for wave 1
        waveTimer = waveDuration;

        //var waves = Instantiate<Wave>(waves);

        UpdateSpawnDelay();
        UpdateWaveData();
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

    }

    void NextWave()
    {
        if (currentWave < waves.Length-1)
        {
            currentWave++;
        }
        waveTimer = waveDuration;
        UpdateSpawnDelay();
        UpdateWaveData();

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

        //Gets Random Enemy From Wave
        int spawnedEnemyIndex = Random.Range(0, currentWaveEnemyCount.Count);
        currentWaveEnemyCount[spawnedEnemyIndex]--;

        //Spawn Enemy

        Debug.Log(currentWaveData[spawnedEnemyIndex].enemyType.name);

        PoolManager.SpawnObject(currentWaveData[spawnedEnemyIndex].enemyType, spawnPos, Quaternion.identity);

        //Remove from Wave Data when spawn max number of an enemy type
        if (currentWaveEnemyCount[spawnedEnemyIndex] <= 0)
        {
            currentWaveEnemyCount.RemoveAt(spawnedEnemyIndex);
        }



        //Instantiate(enemyPrefab, spawnPos, Quaternion.identity);



    }

    int calculateNumberOfEnemiesInCurrentWave()
    {

        int totalNumberOfEnemies = 0;
        foreach (WaveEnemyData enemies in waves[currentWave].enemiesInWaveRuntime)
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

    void UpdateWaveData()
    {
        currentWaveData = waves[currentWave].enemiesInWaveRuntime;
        currentWaveEnemyCount = new List<int> { };
        for(int i = 0; i < currentWaveData.Count; i++)
        {
            currentWaveEnemyCount.Add(currentWaveData[i].numberOfEnemies);
        }
    }
    /*
    void CopyWaveDataToRunTime()
    {
        for(int i = 0; i < waves.Length; i++)
        {
            waves[i].enemiesInWaveRuntime = waves[i].enemiesInWaveInitialize;
        }
    }*/
}
