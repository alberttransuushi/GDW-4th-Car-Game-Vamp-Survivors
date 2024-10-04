using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class WeaponUI : MonoBehaviour
{
  [SerializeField] List<GameObject> buttons;

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
        SetUpButton(buttons[i], newWeapons[rand]);
      }
    }
    
  }
  public void ExitMenu() {
    Time.timeScale = 1;
    gameObject.SetActive(false);
  }
  private void SetUpButton(GameObject button, GameObject weapon) {
    button.GetComponentInChildren<TextMeshPro>().text = weapon.GetComponentInChildren<Weapon>().weaponStats.description;
    
  }
}
