using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCar : MonoBehaviour
{

    [SerializeField] float maxLandSpeed;
    [SerializeField] float acceleration;

    [SerializeField] float turnSpeed;
    [SerializeField] float turnAngle;
    [SerializeField] float driftEfficiency;
    [SerializeField] float driftTurnSpeedModifier;

    Rigidbody rb;
    [SerializeField] Vector3 dirToTurn;
    [SerializeField] LayerMask groundLayer;
    [SerializeField] GameObject centerOfMass;

    RaycastHit hit;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        
    }

    // Update is called once per frame
    void Update()
    {
        Movement();
        UpdateCoM();

    }


    public void Movement()
    {
        if (CheckGrounded())
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
            if (Input.GetKey(KeyCode.W))
            {
                rb.velocity += transform.forward * Time.deltaTime * acceleration;
            }
            if (Input.GetKey(KeyCode.S))
            {
                rb.velocity -= transform.forward * Time.deltaTime * acceleration;
            }

            if (rb.velocity.magnitude > 0)
            {
                TurnToWheel(dirToTurn);
                //print("Turning");
            }
        } 
    }

    void TurnToWheel(Vector3 dir)
    {
        transform.rotation = Quaternion.LookRotation(Vector3.RotateTowards(transform.forward, new Vector3(dir.x, dir.y, dir.z), turnSpeed * Time.deltaTime, 10.0f));
    }
    bool CheckGrounded()
    {
        return Physics.Raycast(transform.position, -transform.up, out hit, 1f, groundLayer);
    }

    void UpdateCoM()
    {
        rb.centerOfMass = centerOfMass.gameObject.transform.localPosition;
    }
}
