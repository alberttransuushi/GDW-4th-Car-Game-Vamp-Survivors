using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class MissileLauncher : MonoBehaviour
{
    [SerializeField]
    private InputActionReference fireControl;
    [SerializeField]
    private InputActionReference reloadControl;

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

    public int maxAmmo = 5;
    public int currentAmmo;
    public float reloadTime;
    public bool isReloading;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        GUICanvas = GameObject.FindGameObjectWithTag("GUI");
        for(int i = 0; i < numberOfMissiles; i++)
        {
            GameObject prefab = Instantiate(crosshairPrefab, GUICanvas.transform);
            lockOnUIs.Add(prefab);
        }
        currentAmmo = maxAmmo;
    }
    private void OnEnable()
    {
        fireControl.action.Enable();
        reloadControl.action.Enable();
    }
    private void OnDisable()
    {
        fireControl.action.Disable();
        reloadControl.action.Disable(); 
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


        if (fireControl.action.WasPressedThisFrame())
        {
            if (currentAmmo > 0)
            {
                FireMissiles();
                Debug.Log("FIRE");
            }
            else if (!isReloading) StartCoroutine(Reload());
                
        }

        if(reloadControl.action.WasPressedThisFrame() && !isReloading) StartCoroutine(Reload());

        currentCam = Camera.main;
        UpdateLockOn();
  
    }

    IEnumerator Reload()
    {
        isReloading = true;
        currentAmmo = 0;
        yield return new WaitForSeconds(reloadTime);
        isReloading = false;
        currentAmmo = maxAmmo;
    }

    void FireMissiles()
    {
        int trueNumberOfMissiles = Mathf.Min(numberOfMissiles, validTargets.Count);
        bool hasFired = false;
        for(int i = 0; i < trueNumberOfMissiles; i++)
        {
            hasFired = true;
            GameObject spawnedMissiles = Instantiate(missilePrefab, missileSpawner.transform.position, missileSpawner.transform.rotation);
            spawnedMissiles.GetComponent<MissileProjectile>().target = validTargets[i];
            
        }
        if (hasFired) currentAmmo--;
        
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
