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

    /*
    //int[] rands = new int[buttons.Count];
    bool repeat = true;
    for (int i = 0; i < buttons.Count; i++) {
      
      //is weapon limits reached
      if (weaponList.GetWeapons().Count >= weaponList.GetWeaponLimit()) {

        while (repeat) {
          rands[i] = Random.Range(0, upgradableWeapons.Count);
          repeat = false;
          for (int j = 0; j < i; j++) {
            if (rands[i] == rands[j]) {
              repeat = true;
            }
          }
        }

        for (int j = 0; j < rands.Length; j++) {
          Debug.Log(rands[j]);
        }
        SetUpButton(buttons[i], upgradableWeapons[rands[i]]);

      } else { //player can get new weapons

        while (repeat) {//gen new number
          rands[i] = Random.Range(0, newWeapons.Count + upgradableWeapons.Count);
          repeat = false;
          for (int j = 0; j < i; j++) {
            if (rands[i] == rands[j]) {
              repeat = true;
            }
          }
        }

        if (rands[i] >= newWeapons.Count) {//upgrade weapon
          rands[i] -= newWeapons.Count;
          SetUpButton(buttons[i], upgradableWeapons[rands[i]]);
        } else {//new weapon
          Debug.Log(buttons[i]);
          Debug.Log(newWeapons[rands[i]]);
          SetUpButton(buttons[i], newWeapons[rands[i]]);
        }
      }
      
    }  
    */
  }
  public void WeaponChange(int weaponRef, bool isUpgrade) {
    if (isUpgrade) {//upgrade weapon
      upgradableWeapons[weaponRef].GetComponentInChildren<Weapon>().weaponStats.damageModifier += 10;
    } else {//get new weapon
      upgradableWeapons.Add(Instantiate(newWeapons[weaponRef], player.transform));
      newWeapons.RemoveAt(weaponRef);
    }

    ExitMenu();
  }
  public void ExitMenu() {
    Time.timeScale = 1;
    gameObject.SetActive(false);
  }
  private void SetUpButton(WeaponUIButton button, GameObject weapon, int r, bool u) {
    button.GetTMP().SetText(weapon.GetComponentInChildren<Weapon>().weaponStats.description);
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
