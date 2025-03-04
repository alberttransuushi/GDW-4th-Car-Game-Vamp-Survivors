using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissileLauncher : MonoBehaviour
{
    public GameObject player;
    public GameObject missilePrefab;
    public GameObject missileSpawner;
    public GameObject GUICanvas;
    public GameObject crosshairPrefab;
    public List<GameObject> targetList = new List<GameObject>();
    public GameObject[] enemyArray;
    public List<GameObject> validTargets = new List<GameObject>();

    [SerializeField, Range(0,75)] float lockOnAngle;
    [SerializeField] float minRange;
    [SerializeField] float maxRange;

    [SerializeField, Range(1, 5)] int numberOfMissiles;

    [SerializeField] List<GameObject> lockOnUIs = new List<GameObject>();
    public Camera currentCam;
    
    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        GUICanvas = GameObject.FindGameObjectWithTag("GUI");
        for(int i = 0; i < numberOfMissiles; i++)
        {
            GameObject prefab = Instantiate(crosshairPrefab, GUICanvas.transform);
            lockOnUIs.Add(prefab);
        }

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

        currentCam = Camera.main;
        UpdateLockOn();
  
    }

    void FireMissiles()
    {
        int trueNumberOfMissiles = Mathf.Min(numberOfMissiles, validTargets.Count);
        for(int i = 0; i < trueNumberOfMissiles; i++)
        {

            GameObject spawnedMissiles = Instantiate(missilePrefab, missileSpawner.transform.position, missileSpawner.transform.rotation);
            spawnedMissiles.GetComponent<MissileProjectile>().target = validTargets[i];
        }
    }

    void UpdateLockOn()
    {
        foreach(GameObject ui in lockOnUIs)
        {
            RectTransform rect = ui.gameObject.GetComponent<RectTransform>();
            rect.anchoredPosition = new Vector2(Screen.width + 100, Screen.height + 100);
        }
        int trueNumberOfMissiles = Mathf.Min(numberOfMissiles, validTargets.Count);
        for (int i = 0; i < trueNumberOfMissiles; i++)
        {
            //Debug.Log(validTargets[i].transform.position);
            Vector3 targetScreenPosition = currentCam.WorldToScreenPoint(validTargets[i].transform.position);
            lockOnUIs[i].gameObject.GetComponent<RectTransform>().position = targetScreenPosition;
        }

    }
}
