using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PauseMenu : MonoBehaviour
{
     [SerializeField] List<GameObject> list;
     [SerializeField] PlayerCar playerRef;
     [SerializeField]
     private InputActionReference pauseControl;
    [SerializeField]
    SoundManager soundManagerRef;

     private bool gamePaused = false;


    private void Update() {
        //Debug.Log(gamePaused);
        if (pauseControl.action.WasPressedThisFrame() && gamePaused == false)
        {


            gamePaused = true;

            for (int i = 0; i < list.Count; i++)
            {
                list[i].SetActive(true);
                Time.timeScale = 0;
            }
            playerRef.audioSource.Pause();
            playerRef.movementControl.action.Enable();
            playerRef.driftControl.action.Enable();
            playerRef.fireControl.action.Enable();
            playerRef.AoADriftControl.action.Enable();
            playerRef.UnstuckControl.action.Enable();
            playerRef.boostControl.action.Enable();

            soundManagerRef.maxSoundInstances = 0;


        }
        else if (pauseControl.action.WasPressedThisFrame() && gamePaused == true)
        {
            CloseMenu();
        }

  }
  public void CloseMenu() {
    for (int i = 0; i < list.Count; i++) {
      list[i].SetActive(false);
    }
    Time.timeScale = 1;
    playerRef.audioSource.UnPause();
    playerRef.movementControl.action.Enable();
    playerRef.driftControl.action.Enable();
    playerRef.fireControl.action.Enable();
    playerRef.AoADriftControl.action.Enable();
    playerRef.UnstuckControl.action.Enable();
    playerRef.boostControl.action.Enable();

        soundManagerRef.maxSoundInstances = 30;

        gamePaused = false;
    }
}
