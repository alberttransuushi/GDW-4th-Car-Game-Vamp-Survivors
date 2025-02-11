using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{
  [SerializeField] List<GameObject> list;
  [SerializeField] PlayerCar playerRef;


  private void Update() {
    if (Input.GetKeyDown(KeyCode.Escape)) {
      for (int i = 0; i < list.Count; i++) {
        list[i].SetActive(true);
        Time.timeScale = 0;
      }
      playerRef.audioSource.Pause();
    }
  }
  public void CloseMenu() {
    for (int i = 0; i < list.Count; i++) {
      list[i].SetActive(false);
    }
    Time.timeScale = 1;
    playerRef.audioSource.UnPause();
    }
}
