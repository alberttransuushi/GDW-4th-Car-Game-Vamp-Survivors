using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PrimaryWeapon : MonoBehaviour
{
    [SerializeField]
    protected InputActionReference fireControl;
    [SerializeField]
    protected InputActionReference reloadControl;

    public GameObject player;
    public GameObject[] enemyArray;
    public List<GameObject> validTargets = new List<GameObject>();

    [SerializeField] protected float minRange;
    [SerializeField] protected float maxRange;


    public int maxAmmo = 5;
    public int currentAmmo;
    public float reloadTime;
    public bool isReloading;

    [SerializeField] protected EnemyTracker enemyTracker;

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

    public virtual void Start()
    {
        enemyTracker = GameObject.FindGameObjectWithTag("EnemyTracker").GetComponent<EnemyTracker>();
        player = GameObject.FindGameObjectWithTag("Player");
        currentAmmo = maxAmmo;
    }

    public virtual void Update()
    {
        //update enemy array
        enemyArray = enemyTracker.GetEnemyArray();


        if (fireControl.action.WasPressedThisFrame())
        {
            Fire();

        }

        if (reloadControl.action.WasPressedThisFrame() && !isReloading) StartCoroutine(Reload());
    }


    public virtual void Fire()
    {

    }

    public virtual IEnumerator Reload()
    {
        isReloading = true;
        currentAmmo = 0;
        yield return new WaitForSeconds(reloadTime);
        isReloading = false;
        currentAmmo = maxAmmo;
    }
}
