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


    [SerializeField] float unGroundedGravity;
    
    
    [SerializeField] float driftEfficiency;
    //[SerializeField] float randomScaleStrenght;
    

    // Start is called before the first frame update
    public override void Start()
    {
        playerCar = GameObject.FindGameObjectWithTag("Player");
        base.Start();
        
        //Vector3 scaleChange = new Vector3(Random.Range(0.75f, 2f), Random.Range(0.75f, 2f), Random.Range(0.75f, 2f));
        //gameObject.transform.localScale += scaleChange;
    }

    // Update is called once per frame
    void Update()
    {
        CheckAlive();
        Movement();
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
        else
        {

            IncreasedGravity(unGroundedGravity);
        }
    }

    public override void OnCollisionEnter(Collision collision)
    {
        base.OnCollisionEnter(collision);
        
    }

    

    void IncreasedGravity(float inc)
    {
        rb.velocity -= new Vector3(0, inc, 0);
    }
}
