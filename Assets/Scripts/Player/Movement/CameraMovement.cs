using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraMovement : MonoBehaviour
{
    [SerializeField] GameObject playerCar;
    [SerializeField] Camera frontCamera;
    [SerializeField] Camera backCamera;
    [SerializeField] Rigidbody playerCarRB;
    [SerializeField] float cameraTurnSpeed;
    [SerializeField] float turnThreshold;
    [SerializeField] float maxFoV;
    [SerializeField] float minFoV;
    [SerializeField]
    private InputActionReference CameraReverseControl;

    // Start is called before the first frame update
    void Awake()
    {
        
        frontCamera.enabled = true;
        backCamera.enabled = false;
        playerCar = GameObject.FindGameObjectWithTag("Player");
        playerCarRB = playerCar.GetComponent<Rigidbody>();
        
    }

    private void OnEnable()
    {
        frontCamera.enabled = true;
        backCamera.enabled = false;
        playerCar = GameObject.FindGameObjectWithTag("Player");
        playerCarRB = playerCar.GetComponent<Rigidbody>();
        CameraReverseControl.action.Enable();


    }

    private void OnDisable()
    {
        CameraReverseControl.action.Disable();


    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //Zoom in/out depending on speed
        frontCamera.fieldOfView = Mathf.Lerp(minFoV, maxFoV, playerCarRB.velocity.magnitude / playerCar.GetComponent<PlayerCar>().maxLandSpeed);
        backCamera.fieldOfView = Mathf.Lerp(minFoV, maxFoV, playerCarRB.velocity.magnitude / playerCar.GetComponent<PlayerCar>().maxLandSpeed);

        //Lag behind 
        Vector3 dir = playerCarRB.velocity.normalized;
        if (playerCarRB.velocity.magnitude < turnThreshold || Vector3.Dot(playerCar.transform.forward, playerCar.GetComponent<Rigidbody>().velocity.normalized) < 0)
        {
            transform.rotation = Quaternion.LookRotation(Vector3.RotateTowards(transform.forward, playerCar.transform.forward, cameraTurnSpeed * Time.deltaTime, 10.0f));

        }
        else
        {

            transform.rotation = Quaternion.LookRotation(Vector3.RotateTowards(transform.forward, new Vector3(dir.x, dir.y, dir.z), cameraTurnSpeed * Time.deltaTime, 10.0f));

        }

        transform.position = playerCar.transform.position;

        /*
        if(CameraReverseControl.action.WasPressedThisFrame()) { 
            frontCamera.enabled = false;
            backCamera.enabled = true;

        } else if (CameraReverseControl.action.WasReleasedThisFrame())
        {
            frontCamera.enabled = true;
            backCamera.enabled = false;
        }*/
        if (CameraReverseControl.action.IsPressed())
        {
            frontCamera.enabled = false;
            backCamera.enabled = true;

        }
        else 
        {
            frontCamera.enabled = true;
            backCamera.enabled = false;
        }
    }
}
