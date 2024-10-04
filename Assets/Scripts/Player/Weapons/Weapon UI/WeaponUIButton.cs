using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class WeaponUIButton : MonoBehaviour
{
  [SerializeField] TextMeshProUGUI text;
  [SerializeField] Image weaponIcon;

  public TextMeshProUGUI GetTMP() {
    return text;
  }
  public Image GetImage() {
    return weaponIcon;
  }

}
