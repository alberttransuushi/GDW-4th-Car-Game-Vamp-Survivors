using System.Collections;
using System.Collections.Generic;
using UnityEngine;

abstract public class Weapon : MonoBehaviour {
  abstract public void attack();
  public WeaponStats weaponStats;
  [SerializeField] int level = 1;
  [SerializeField] protected float damage;

  // Start is called before the first frame update
  void Start() {

  }

  // Update is called once per frame
  void Update() {
    
  }
  public void UpgradeLevel() {
    level += 1;
  }
}
