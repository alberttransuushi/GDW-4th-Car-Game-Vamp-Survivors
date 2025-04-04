using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class MissileLauncher : PrimaryWeapon {

  public GameObject crosshairPrefab;
  public GameObject missilePrefab;
  public GameObject missileSpawner;
  public GameObject GUICanvas;

    [SerializeField] float damage;
    [SerializeField] float knockbackStrength;
  [SerializeField, Range(0, 75)] float lockOnAngle;

  [SerializeField] int numberOfMissiles;

  [SerializeField] List<GameObject> lockOnUIs = new List<GameObject>();
  public Camera currentCam;

    public int missileCountUpgradeIncrease;
    public float reloadUpgradeDecrease;
    public float DamageUpgradeIncrease;
    public float KnockbackUpgradeIncrease;

  public override void Start() {
    base.Start();
    GUICanvas = GameObject.FindGameObjectWithTag("GUI");
    for (int i = 0; i < numberOfMissiles; i++) {
      GameObject prefab = Instantiate(crosshairPrefab, GUICanvas.transform);
      lockOnUIs.Add(prefab);
        }

    }


    // Update is called once per frame
    public override void Update()
    {

        validTargets.Clear();
        foreach (GameObject enemy in enemyArray)
        {

            if (enemy != null && enemy.gameObject != null) // Ensure enemy still exists
            {
                if (Vector3.Angle(Vector3.Normalize(enemy.transform.position - transform.position), transform.forward) <= lockOnAngle
                    && Vector3.Distance(enemy.transform.position, transform.position) <= maxRange
                    && Vector3.Distance(enemy.transform.position, transform.position) >= minRange)
                {
                    validTargets.Add(enemy);
                }
            }

        }
        currentCam = Camera.main;
        UpdateLockOn();
        base.Update();

        numberOfMissiles = lockOnUIs.Count;
    }

    public override void Fire() {
    if (currentAmmo > 0) {
      FireMissiles();

    } else if (!isReloading) StartCoroutine(Reload());
  }



  void FireMissiles() {
    int trueNumberOfMissiles = Mathf.Min(numberOfMissiles, validTargets.Count);
    bool hasFired = false;
    for (int i = 0; i < trueNumberOfMissiles; i++) {
      hasFired = true;
      GameObject spawnedMissiles = Instantiate(missilePrefab, missileSpawner.transform.position, missileSpawner.transform.rotation);
      spawnedMissiles.GetComponent<MissileProjectile>().target = validTargets[i];
            spawnedMissiles.GetComponent<MissileProjectile>().damage = damage;
            spawnedMissiles.GetComponent<MissileProjectile>().explosionKnockback = knockbackStrength;
        }
    if (hasFired) currentAmmo--;

  }

  void UpdateLockOn() {
    foreach (GameObject ui in lockOnUIs) {
      RectTransform rect = ui.gameObject.GetComponent<RectTransform>();
      rect.anchoredPosition = new Vector2(Screen.width + 100, Screen.height + 100);
    }
    int trueNumberOfMissiles = Mathf.Min(numberOfMissiles, validTargets.Count);
    for (int i = 0; i < trueNumberOfMissiles; i++) {
      //Debug.Log(validTargets[i].transform.position);
      Vector3 targetScreenPosition = currentCam.WorldToScreenPoint(validTargets[i].transform.position);
      lockOnUIs[i].gameObject.GetComponent<RectTransform>().position = targetScreenPosition;
    }

  }


    protected override void Upgrade1()
    {
        numberOfMissiles += missileCountUpgradeIncrease;
    }
    protected override void Upgrade2()
    {


        reloadTime -= reloadUpgradeDecrease;

    }
    protected override void Upgrade3()
    {
        damage += DamageUpgradeIncrease;
        knockbackStrength += KnockbackUpgradeIncrease;

    }
}
