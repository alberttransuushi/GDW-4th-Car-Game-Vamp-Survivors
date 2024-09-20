using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    [SerializeField] GameObject playerCar;
    [SerializeField] Camera camera;
    [SerializeField] Rigidbody playerCarRB;
    [SerializeField] float cameraTurnSpeed;
    [SerializeField] float turnThreshold;
    [SerializeField] float maxFoV;
    [SerializeField] float minFoV;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //Zoom in/out depending on speed
        camera.fieldOfView = Mathf.Lerp(maxFoV, minFoV, playerCarRB.velocity.magnitude / playerCar.GetComponent<PlayerCar>().maxLandSpeed);

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
    }
}
