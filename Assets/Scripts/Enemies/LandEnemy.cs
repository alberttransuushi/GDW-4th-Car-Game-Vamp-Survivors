using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.HID;
using UnityEngine.UIElements;

public class LandEnemy : BaseEnemy
{

    float currentSpeed;
    [SerializeField] float maxLandSpeed;
    [SerializeField] float acceleration;



    
    [SerializeField] float turnSpeed;
    [SerializeField] float driftEfficiency;

    public float AngleToPlayer;

    [SerializeField] LayerMask groundLayer;
    RaycastHit hit;
    Rigidbody rb;

    // Start is called before the first frame update
    void Start()
    {
        playerCar = GameObject.Find("player");
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        CheckAlive();
        Movement();
    }

    void TurnToPlayer()
    {
        Vector3 dirToPlayer = playerCar.transform.position - transform.position;
        
        transform.rotation = Quaternion.LookRotation(Vector3.RotateTowards(transform.forward, new Vector3(dirToPlayer.x, dirToPlayer.y, dirToPlayer.z), turnSpeed * Time.deltaTime, 10.0f));

        AngleToPlayer = Vector3.Angle(dirToPlayer, transform.forward); 
    }

    void Movement()
    {
        
        currentSpeed = rb.velocity.magnitude;
        //if (CheckGrounded())
        if(CheckGrounded())
        {
            //Can only turn if moving
            if (rb.velocity.magnitude > 0)
            {
                TurnToPlayer();
                //print("Turning");
            }
            
            //Speed Up
            if (rb.velocity.magnitude < maxLandSpeed + GetCatchUpBonus())
            {
                //print(maxLandSpeed + GetCatchUpBonus());

                rb.velocity += transform.forward * Time.deltaTime * acceleration;

                //Slows down when drifting
                rb.velocity -= transform.forward * Time.deltaTime * acceleration * AngleToPlayer / 180;
                //print("Delta:" + Time.deltaTime);
                //print("Speed:" + (transform.forward * Time.deltaTime * acceleration).magnitude);
                //print("Friction: " + (transform.forward * Time.deltaTime * acceleration * AngleToPlayer / 180).magnitude);
                
                //Slows down perpendicular velocity
                Vector3 perpendicularVelocity = transform.right * Vector3.Dot(transform.right, rb.velocity);
                float perpendiuclarSpeed = perpendicularVelocity.magnitude * driftEfficiency;
                rb.velocity -= perpendicularVelocity.normalized * perpendiuclarSpeed;


            }
        }
    }

    bool CheckGrounded()
    {
        return Physics.Raycast(transform.position, -transform.up, out hit, 1f, groundLayer);
    }

}
