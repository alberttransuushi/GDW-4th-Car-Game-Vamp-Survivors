using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class WeaponUI : MonoBehaviour
{
  [SerializeField] List<WeaponUIButton> buttons;

  [SerializeField] List<GameObject> newWeapons;
  [SerializeField] List<GameObject> upgradableWeapons;
  
  public void UpdateUpgradeMenu() {
    for (int i = 0; i < buttons.Count; i++) {
      int rand = Random.Range(0, newWeapons.Count + upgradableWeapons.Count);
      Debug.Log(rand);
      if (rand >= newWeapons.Count) {
        rand -= newWeapons.Count;
        SetUpButton(buttons[i], upgradableWeapons[rand]);
      } else {
        Debug.Log(buttons[i]);
        Debug.Log(newWeapons[rand]);
        SetUpButton(buttons[i], newWeapons[rand]);
      }
    }
    
  }
  public void ExitMenu() {
    Time.timeScale = 1;
    gameObject.SetActive(false);
  }
  private void SetUpButton(WeaponUIButton button, GameObject weapon) {
    button.GetTMP().SetText(weapon.GetComponentInChildren<Weapon>().weaponStats.description);
  }
}
