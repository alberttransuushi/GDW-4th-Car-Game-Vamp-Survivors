using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    public GameObject FireTruckRef;
    public GameObject FireTruckRPGRef;
    public GameObject FireTruckPlowRef;
    public GameObject FireTruckHOGRef;

    public GameObject SamuraiCarRef;
    public GameObject SamuraiCarRPGRef;
    public GameObject SamuraiCarPlowRef;
    public GameObject SamuraiCarHOGRef;

    public GameObject PoliceCarRef;
    public GameObject PoliceCarRPGRef;
    public GameObject PoliceCarPlowRef;
    public GameObject PoliceCarHOGRef;

    private void Awake()
    {
        Debug.Log("GameManager Awake");
        switch (PlayerPrefs.GetInt("SelectedCar"))
        {
            case 0:
                FireTruckRef.SetActive(true);
                Debug.Log("FireTruck");
                switch (PlayerPrefs.GetInt("SelectedWeapon"))
                {
                    case 0:
                        FireTruckRPGRef.SetActive(true);

                        break;

                    case 1:
                        FireTruckPlowRef.SetActive(true);
                        break;
                    case 2:
                        FireTruckHOGRef.SetActive(true);
                        break;
                }
                break;
            case 1:
                SamuraiCarRef.SetActive(true);
                Debug.Log("SamuraiCar");
                switch (PlayerPrefs.GetInt("SelectedWeapon"))
                {
                    case 0:
                        SamuraiCarRPGRef.SetActive(true);
                        break;

                    case 1:
                        SamuraiCarPlowRef.SetActive(true); 
                        break;
                    case 2:
                        SamuraiCarHOGRef.SetActive(true);
                        break;
                }
                break;
            case 2:
                PoliceCarRef.SetActive(true);
                Debug.Log("PoliceCar");
                switch (PlayerPrefs.GetInt("SelectedWeapon"))
                {
                        case 0:
                            PoliceCarRPGRef.SetActive(true);
                            break;

                        case 1:
                            PoliceCarPlowRef.SetActive(true);
                            break;
                        case 2:
                            PoliceCarHOGRef.SetActive(true);
                         break;
                }
                break;
        }
    }
    

    
}
