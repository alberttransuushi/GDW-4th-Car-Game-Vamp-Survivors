using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PrimaryWeapon : MonoBehaviour {
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

  private void OnEnable() {
    fireControl.action.Enable();
    reloadControl.action.Enable();
  }
  private void OnDisable() {
    fireControl.action.Disable();
    reloadControl.action.Disable();
  }

  public virtual void Start() {
    enemyTracker = GameObject.FindGameObjectWithTag("EnemyTracker").GetComponent<EnemyTracker>();
    player = GameObject.FindGameObjectWithTag("Player");
    if (UI != null) {
      UI.SetPlayerWeapon(this.gameObject);
    }
    currentAmmo = maxAmmo;
  }


  public virtual void Update() {
    //update enemy array
    enemyArray = enemyTracker.GetEnemyArray();


    if (fireControl.action.WasPressedThisFrame()) {
      Fire();

    }

    if (reloadControl.action.WasPressedThisFrame() && !isReloading) StartCoroutine(Reload());
  }


  public virtual void Fire() {

  }

  public virtual IEnumerator Reload() {
    isReloading = true;
    currentAmmo = 0;
    yield return new WaitForSeconds(reloadTime);
    isReloading = false;
    currentAmmo = maxAmmo;
  }
  //UI STUFF
  [SerializeField] WeaponUI UI;
  [SerializeField] List<string> upgradeTexts;
  [SerializeField] List<Sprite> upgradeSprites;
  public int GetUpgradeTypeAmount() {
    return upgradeTexts.Count;
  }
  public string GetUpgradeText (int i) {
    if (upgradeTexts.Count < i) {
      return "ERROR";
    }
    return upgradeTexts[i];
  }
  public Sprite GetUpgradeSprite (int i) {
    if (upgradeSprites.Count < i) {
      return null;
    }
    return upgradeSprites[i];
  }
  public void Upgrade(int upgradeRef) {
    //THE UPGRADE SYSTEM WILL REFER TO THE UPGRADE TEXT FOR UPGRADE FORMAT
    //USE A SWITCH CASE HERE TO EXECUTE THE RIGHT UPGRADE FOR THE PLAYER

  }
}
