using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class WeaponUIButton : MonoBehaviour
{
  [SerializeField] WeaponUI weaponUI;
  [SerializeField] TextMeshProUGUI text;
  [SerializeField] Image weaponIcon;
  [SerializeField] GameObject weapon;
  int weaponRef;
  bool isUpgrade;

  public TextMeshProUGUI GetTMP() {
    return text;
  }
  public Image GetImage() {
    return weaponIcon;
  }
  public void SetWeapon(GameObject w) {
    weapon = w;
  }
  public GameObject GetWeapon() {
    return weapon;
  }
  public void SetWeaponReference(int r) {
    weaponRef = r;
  }
  public int GetWeaponReference() {
    return weaponRef;
  }
  public void SetIsUpgrade(bool u) {
    isUpgrade = u;
  }
  public bool GetIsUpgrade() {
    return isUpgrade;
  }
  public void SetSprite(Sprite s) {
    weaponIcon.sprite = s;
  }
  public void ChangeWeapon() {
    weaponUI.WeaponChange(weaponRef, isUpgrade);
  }
}
