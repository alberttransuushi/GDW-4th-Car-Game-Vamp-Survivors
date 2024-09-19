using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCar : MonoBehaviour
{

    [SerializeField] public float maxLandSpeed;
    [SerializeField] float acceleration;

    [SerializeField] float turnSpeed;
    [SerializeField] float turnAngle;

    // 0 = no friction/slidey | 1 = no momentum from drifting
    [SerializeField] float driftFriction;
    [SerializeField] float driftTurnSpeedModifier;

    Rigidbody rb;
    [SerializeField] Vector3 dirToTurn;
    [SerializeField] LayerMask groundLayer;
    [SerializeField] GameObject centerOfMass;

    [SerializeField] float unGroundedGravity;

    RaycastHit hit;
    bool isDrifting;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.centerOfMass = centerOfMass.gameObject.transform.localPosition;
    }

    // Update is called once per frame
    void Update()
    {
        Movement();
        

    }


    public void Movement()
    {
        Steering();


        if (CheckGrounded())
        {
            
            AccelDeccel();
            

        } else
        {
            IncreasedGravity(unGroundedGravity);
        }
        
    }

    void AccelDeccel()
    {
        if (Input.GetKey(KeyCode.W))
        {
            rb.velocity += transform.forward * Time.deltaTime * acceleration;
        }
        if (Input.GetKey(KeyCode.S))
        {
            rb.velocity -= transform.forward * Time.deltaTime * acceleration;
        }
        if (Input.GetKey(KeyCode.LeftShift))
        {
            isDrifting = true;
        }
        else
        {
            isDrifting = false;
        }

        FrictionVelocity();
    }

    void Steering()
    {
        dirToTurn = gameObject.transform.forward;
        if (Input.GetKey(KeyCode.A))
        {
            dirToTurn = Quaternion.AngleAxis(-turnAngle, Vector3.up) * transform.forward;
        }
        if (Input.GetKey(KeyCode.D))
        {
            dirToTurn = Quaternion.AngleAxis(turnAngle, Vector3.up) * transform.forward;
        }
        if (rb.velocity.magnitude > 5)
        {
            TurnToWheel(dirToTurn);
            //print("Turning");
        }
    }
    void TurnToWheel(Vector3 dir)
    {
        //Add More Turn Speed when drifting
        if (!isDrifting)
        {
            transform.rotation = Quaternion.LookRotation(Vector3.RotateTowards(transform.forward, new Vector3(dir.x, dir.y, dir.z), turnSpeed * Time.deltaTime, 10.0f));
        } else
        {
            transform.rotation = Quaternion.LookRotation(Vector3.RotateTowards(transform.forward, new Vector3(dir.x, dir.y, dir.z), (turnSpeed + driftTurnSpeedModifier) * Time.deltaTime, 10.0f));
        }
       
    }

    bool CheckGrounded()
    {
        return Physics.Raycast(transform.position, -transform.up, out hit, 1f, groundLayer);
    }

    void FrictionVelocity()
    {
        //Apply Friction caused by drifting
        Vector3 perpendicularVelocity = transform.right * Vector3.Dot(transform.right, rb.velocity);
        float perpendiuclarSpeed = perpendicularVelocity.magnitude * driftFriction;
        rb.velocity -= perpendicularVelocity.normalized * perpendiuclarSpeed;

       
    }


    void IncreasedGravity(float inc)
    {
        rb.AddForce(new Vector3(0, inc, 0));
    }
}
