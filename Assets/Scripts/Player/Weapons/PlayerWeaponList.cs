using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWeaponList : MonoBehaviour
{
  [SerializeField] List<GameObject> weapons;
  [SerializeField] int weaponLimit;

  public List<GameObject> GetWeapons() {
    return weapons;
  }
  public int GetWeaponLimit() {
    return weaponLimit;
  }
}
