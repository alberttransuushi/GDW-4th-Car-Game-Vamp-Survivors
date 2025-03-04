using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleSpawner : MonoBehaviour
{

    public float minRange;
    public float maxRange;

    public GameObject obstacle;

    public List<GameObject> targets = new List<GameObject>();
    Rigidbody playerCarRb;
    [SerializeField] float rangeAddedFromSpeedMultiplier;

    public CheckPointManager[] checkPoints;

    List<GameObject> spawnedObstacles = new List<GameObject>();
    public GameObject obstacleCheckPoint;

    public List<GameObject> targetList = new List<GameObject>();

    [SerializeField] float minSpawnDelay;
    [SerializeField] float maxSpawnDelay;
    [SerializeField] float obstacleDespawnDistance;

    // Start is called before the first frame update
    void Start()
    {
        playerCarRb = GetComponent<Rigidbody>();
        checkPoints = FindObjectsOfType<CheckPointManager>();
        Invoke("InvokeSpawnAtRandomInterval", Random.Range(minSpawnDelay, maxSpawnDelay));

    }

    private void Update()
    {
        //If no Spawned Obstacles
        if (spawnedObstacles.Count > 0)
        {
            //If obstacle checkpoint is behind
            if (!isInFront(obstacleCheckPoint) && Vector3.Distance(obstacleCheckPoint.transform.position, transform.position) > obstacleDespawnDistance) 
            {
                //Delete all obstacles
                for (int i = spawnedObstacles.Count - 1; i >= 0; i--)
                {
                    Destroy(spawnedObstacles[i]);
                    spawnedObstacles.RemoveAt(i);  
                }



            }
        }

    }

    void InvokeSpawnAtRandomInterval()
    {
        //If no Spawned Obstacles
        if(spawnedObstacles.Count == 0)
        {

            SpawnObstacle();
        }
        Invoke("InvokeSpawnAtRandomInterval", Random.Range(minSpawnDelay, maxSpawnDelay));
    }


    List<GameObject> FindAllValidCheckPoints()
    {

        

        List<GameObject> validGameObjects = new List<GameObject>();

        foreach (var obj in checkPoints)
        {
            GameObject gameObject = obj.gameObject;

            if (isInFront(gameObject))
            {
                float distance = Vector3.Distance(transform.position, gameObject.transform.position);
                if (distance > minRange + playerCarRb.velocity.magnitude * rangeAddedFromSpeedMultiplier && distance < maxRange + playerCarRb.velocity.magnitude * rangeAddedFromSpeedMultiplier)
                {
                    validGameObjects.Add(gameObject);
                }
            }
        }

        return validGameObjects;
    }


    bool isInFront(GameObject obj)
    {
        return Vector3.Dot(Vector3.forward, transform.InverseTransformPoint(obj.transform.position)) > 0;
    }

    void SpawnObstacle()
    {
        targetList = FindAllValidCheckPoints();
        if (targetList.Count <= 0)
        {
            return;
        }
        int f = Random.Range(0, targetList.Count);
        //Debug.Log(f);
        GameObject target = targetList[f];

        obstacleCheckPoint = target; 
        
        int numberOfObstacles = Random.Range(1, 4);
        while (spawnedObstacles.Count < numberOfObstacles)
        {
            GameObject targetLocation = obstacleCheckPoint.GetComponent<CheckPointManager>().targets[Random.Range(0, 5)];
            GameObject spawnedObject = Instantiate(obstacle, targetLocation.transform.position, targetLocation.transform.rotation);
            spawnedObstacles.Add(spawnedObject);
        }


    }
}
