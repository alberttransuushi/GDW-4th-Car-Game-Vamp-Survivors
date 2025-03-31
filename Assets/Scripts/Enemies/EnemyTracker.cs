using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyTracker : MonoBehaviour
{
    public GameObject[] enemyArray;
    public float updateDelay;

    // Start is called before the first frame update
    void Start()
    {
        InvokeRepeating("UpdateEnemyArray", 0f, updateDelay);
    }

    void UpdateEnemyArray()
    {
        enemyArray = GameObject.FindGameObjectsWithTag("Enemy");
    }

    public GameObject[] GetEnemyArray()
    {
        return enemyArray;
    }
}
