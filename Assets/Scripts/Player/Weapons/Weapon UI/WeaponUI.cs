using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class WeaponUI : MonoBehaviour {
  GameObject player;
  [SerializeField] List<WeaponUIButton> buttons;

  [SerializeField] int maxWeapons = 5;
  [SerializeField] List<GameObject> newWeapons;
  [SerializeField] List<GameObject> upgradableWeapons;
  [SerializeField] List<GameObject> maxedWeapons;

  [SerializeField] PlayerWeaponList weaponList;
  private void Start() {
    player = GameObject.Find("player");
    weaponList = player.GetComponent<PlayerWeaponList>();
  }
  public void UpdateUpgradeMenu() {
    if (upgradableWeapons.Count + maxedWeapons.Count >= maxWeapons) {//Weapon limit reached
      if (upgradableWeapons.Count < buttons.Count) {//not enough space for weapons to fill buttons slots

      } else {// enough weapons to fill buttons
        int[] rands =  MakeRandomIntList(buttons.Count, upgradableWeapons.Count);
        for (int i = 0; i < buttons.Count; i++) {
          SetUpButton(buttons[i], upgradableWeapons[rands[i]], rands[i], true);
        }
      }
    } else {//can get new weapons
      int[] rands =  MakeRandomIntList(buttons.Count, newWeapons.Count + upgradableWeapons.Count);
      for (int i = 0; i < buttons.Count; i++) {
        if (rands[i] >= newWeapons.Count) {//upgrade weapon
          rands[i] -= newWeapons.Count;
          SetUpButton(buttons[i], upgradableWeapons[rands[i]], rands[i], true);
        } else {//new weapon
          SetUpButton(buttons[i], newWeapons[rands[i]], rands[i], false);
        }
      }
    }
  }
  public void WeaponChange(int weaponRef, bool isUpgrade) {
    if (isUpgrade) {//upgrade weapon
      upgradableWeapons[weaponRef].GetComponentInChildren<Weapon>().UpgradeWeapon();
      //upgradableWeapons[weaponRef].GetComponentInChildren<Weapon>().weaponStats.damageModifier += 10;
    } else {//get new weapon
      GameObject newWeapon =Instantiate(newWeapons[weaponRef], player.transform);
      newWeapon.GetComponentInChildren<Weapon>().UpgradeWeapon();
      upgradableWeapons.Add(newWeapon);
      newWeapons.RemoveAt(weaponRef);
    }

    ExitMenu();
  }
  public void ExitMenu() {
    Time.timeScale = 1;
    gameObject.SetActive(false);
  }
  private void SetUpButton(WeaponUIButton button, GameObject weapon, int r, bool u) {
    button.GetTMP().SetText(weapon.GetComponentInChildren<Weapon>().weaponStats.GetLevelUpDesc());
    button.SetSprite(weapon.GetComponentInChildren<Weapon>().weaponStats.sprite);
    button.SetWeapon(weapon);
    button.SetWeaponReference(r);
    button.SetIsUpgrade(u);
  }
  private int[] MakeRandomIntList(int listLength, int maxInt) {
    if (maxInt > listLength) return null; // max int must be more than size of array

    int[] rands = new int[listLength];
    bool repeat = true;
    for (int i = 0; i < rands.Length; i++) {
      repeat = true;
      while (repeat) {
        rands[i] = Random.Range(0, maxInt);
        repeat = false;
        for (int j = 0; j < i; j++) {
          if (rands[i] == rands[j]) {
            repeat = true;
          }
        }
      }
    }

    return rands;
  }
}
