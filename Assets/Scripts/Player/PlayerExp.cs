using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerExp : MonoBehaviour
{
  [SerializeField] float exp = 0;
  [SerializeField] float level = 1;
  [SerializeField] float nextLevelRequirement = 30;
  [SerializeField] Slider slider;
  [SerializeField] Text levelText;
  [SerializeField] GameObject upgradeUI;
  private void Start() {
    UpdateExpReq();
  }
  void UpdateExpReq() {
    levelText.text = "Level " + level.ToString();
    nextLevelRequirement = Mathf.RoundToInt(Mathf.Pow(4 * (level + 1), 2)) - Mathf.RoundToInt(Mathf.Pow(4 * level, 2));

  }
  public void GetExp(float xp) {
    exp += xp;
    slider.value = exp / nextLevelRequirement;
    if (exp > nextLevelRequirement) {
      level += 1;
      exp -= nextLevelRequirement;
      UpdateExpReq();
      Time.timeScale = 0;
      upgradeUI.gameObject.SetActive(true);
    }
  }
}
