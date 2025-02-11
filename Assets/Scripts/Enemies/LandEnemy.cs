using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.HID;
using UnityEngine.UIElements;

public class LandEnemy : BaseEnemy
{

    float currentSpeed;
    [SerializeField] float currentLimitSpeed;
    [SerializeField] float minLandSpeed = 90f;
    [SerializeField] float maxLandSpeed = 210f;
    [SerializeField] float acceleration;


    [SerializeField] float unGroundedGravity;
    [SerializeField] float downwardForceMultiplier = 1f;
    
    [SerializeField] float driftEfficiency;

    //[SerializeField] float randomScaleStrenght;
    

    // Start is called before the first frame update
    public override void Awake()
    {
        playerCar = GameObject.FindGameObjectWithTag("Player");
        base.Awake();
        currentLimitSpeed = playerCar.GetComponent<PlayerCar>().maxLandSpeed;
        
        //Vector3 scaleChange = new Vector3(Random.Range(0.75f, 2f), Random.Range(0.75f, 2f), Random.Range(0.75f, 2f));
        //gameObject.transform.localScale += scaleChange;
    }

    // Update is called once per frame
    public override void LateUpdate()
    {
        base.LateUpdate();
        CheckAlive();
        Movement();
    }

    public override void OnEnable()
    {
        base.OnEnable();
        rb.velocity = Vector3.forward * playerCar.GetComponent<Rigidbody>().velocity.magnitude;
    }

    void Movement()
    {
        
        currentSpeed = rb.velocity.magnitude;
        //if (CheckGrounded())
        if(CheckGrounded())
        {
            ApplyDownwardForce();
            //Can only turn if moving
            if (rb.velocity.magnitude > 0)
            {
                TurnToTarget();
                //print("Turning");
            }
            
            //Speed Up
            if (IsPlayerAhead())
            {
                if (rb.velocity.magnitude < Mathf.Min(currentLimitSpeed + GetCatchUpBonus(), maxLandSpeed))
                {
                    //print(currentLimitSpeed + GetCatchUpBonus());

                    rb.velocity += transform.forward * Time.deltaTime * acceleration;
                    
                    Debug.Log("Speed: " + rb.velocity.magnitude);
                    //Slows down when drifting
                    rb.velocity -= transform.forward * Time.deltaTime * acceleration * AngleToTarget / 180;
                    //print("Delta:" + Time.deltaTime);
                    //print("Speed:" + (transform.forward * Time.deltaTime * acceleration).magnitude);
                    //print("Friction: " + (transform.forward * Time.deltaTime * acceleration * AngleToTarget / 180).magnitude);

                    //Slows down perpendicular velocity
                    Vector3 perpendicularVelocity = transform.right * Vector3.Dot(transform.right, rb.velocity);
                    float perpendiuclarSpeed = perpendicularVelocity.magnitude * driftEfficiency;
                    rb.velocity -= perpendicularVelocity.normalized * perpendiuclarSpeed;


                }
            } else
            {
                if (rb.velocity.magnitude < Mathf.Max(currentLimitSpeed - GetCatchUpBonus(), minLandSpeed))
                {
                    //print(currentLimitSpeed + GetCatchUpBonus());

                    rb.velocity += transform.forward * Time.deltaTime * acceleration;
                    
                    Debug.Log("Speed: " + rb.velocity.magnitude);
                    //Slows down when drifting
                    rb.velocity -= transform.forward * Time.deltaTime * acceleration * AngleToTarget / 180;
                    //print("Delta:" + Time.deltaTime);
                    //print("Speed:" + (transform.forward * Time.deltaTime * acceleration).magnitude);
                    //print("Friction: " + (transform.forward * Time.deltaTime * acceleration * AngleToTarget / 180).magnitude);

                    //Slows down perpendicular velocity
                    Vector3 perpendicularVelocity = transform.right * Vector3.Dot(transform.right, rb.velocity);
                    float perpendiuclarSpeed = perpendicularVelocity.magnitude * driftEfficiency;
                    rb.velocity -= perpendicularVelocity.normalized * perpendiuclarSpeed;


                }
            }
        }
        else
        {

            IncreasedGravity(unGroundedGravity);
        }
    }
    void ApplyDownwardForce()
    {
        rb.AddForce(0, downwardForceMultiplier * 0.00119f * Mathf.Pow(rb.velocity.magnitude, 2), 0, ForceMode.Force);
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
