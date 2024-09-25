using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponUI : MonoBehaviour
{
  [SerializeField] GameObject button1;
  [SerializeField] GameObject button2;
  [SerializeField] GameObject button3;
  [SerializeField] GameObject button4;
  [SerializeField] GameObject button5;
  
  public void ExitMenu() {
    Time.timeScale = 1;
    gameObject.SetActive(false);
  }
}
