using System.Collections;
using System.Collections.Generic;
using UnityEngine;

abstract public class Weapon : MonoBehaviour {
  abstract public void Attack();
  public WeaponStats weaponStats;
  [SerializeField] protected float damage;

  // Start is called before the first frame update
  public void Start() {
    weaponStats = Instantiate(weaponStats);
  }

  // Update is called once per frame
  void Update() {
    
  }
  public void UpgradeWeapon() {
    switch (weaponStats.GetLevel()) {
      case 0:
        weaponStats = Instantiate(weaponStats);
        Debug.Log("base level");
        break;

      case 1:
        LevelUp1();
        break;

      case 2:
        LevelUp2();
        break;

      case 3:
        LevelUp3();
        break;

      case 4:
        LevelUp4();
        break;

      default:
        Debug.Log("max level");
        break;
    }
    weaponStats.LevelUp();
  }
  abstract public void LevelUp1();
  abstract public void LevelUp2();
  abstract public void LevelUp3();
  abstract public void LevelUp4();

}
