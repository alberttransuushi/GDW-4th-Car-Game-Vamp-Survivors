using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
public class PoolManager : MonoBehaviour
{


    public static List<PooledInfo> objectPools = new List<PooledInfo>();

    public static GameObject SpawnObject(GameObject gameObject, Vector3 pos, Quaternion rotation)
    {

        //Check If Pool Exists
        PooledInfo pool = objectPools.Find(p => p.lookUpString == gameObject.name);

        if (pool == null)
        {
            pool = new PooledInfo() { lookUpString = gameObject.name };
            objectPools.Add(pool);
        }


        //See if there is an inactive object in the pool
        GameObject spawnableObject = pool.InactiveObjects.FirstOrDefault();

        //Instantiate a new object if there are no inactive objects to activate
        if (spawnableObject == null)
        {
            spawnableObject = Instantiate(gameObject, pos, rotation);
        }
        //Reactivate and "Spawn" Enemy
        else
        {
            spawnableObject.transform.position = pos;
            spawnableObject.transform.rotation = rotation;
            pool.InactiveObjects.Remove(spawnableObject);
            spawnableObject.SetActive(true);
        }
        return spawnableObject;


    }

    public static void ReturnObjectToPool(GameObject gameObject)
    {
        //Remove (Clone)
        string gameObjectName = gameObject.name.Substring(0, gameObject.name.Length - 7);

        PooledInfo pool = objectPools.Find(p => p.lookUpString == gameObjectName);

        if(pool == null)
        {
            Debug.LogWarning("Pool is Empty. There is nothing to release. " + gameObject.name);
        } 
        else
        {
            gameObject.SetActive(false);
            pool.InactiveObjects.Add(gameObject);

        }
    }

}
