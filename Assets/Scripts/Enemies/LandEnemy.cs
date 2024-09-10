using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.HID;
using UnityEngine.UIElements;

public class LandEnemy : MonoBehaviour
{

    
    [SerializeField] float maxLandVelocity;
    [SerializeField] float acceleration;

    [SerializeField] float health;
    
    [SerializeField] float turnSpeed;


    [SerializeField] GameObject playerCar;
    [SerializeField] float AngleToPlayer;

    [SerializeField] LayerMask groundLayer;
    RaycastHit hit;
    bool isCarGrounded;
    Rigidbody rb;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Movement();
        //print(currentTurnAngle);
    }

    void TurnToPlayer()
    {
        Vector3 dirToPlayer = playerCar.transform.position - transform.position;
        
        transform.rotation = Quaternion.LookRotation(Vector3.RotateTowards(transform.forward, dirToPlayer, turnSpeed * Time.deltaTime, 0.0f));


        AngleToPlayer = Vector3.Angle(dirToPlayer, transform.forward); 
    }

    void Movement()
    {
        CheckGrounded();

        if (rb.velocity.magnitude > 0)
        {
            TurnToPlayer();
        }
        if (rb.velocity.magnitude < maxLandVelocity && isCarGrounded)
        {

            rb.velocity += transform.forward * Time.deltaTime * acceleration;
            rb.velocity -= transform.forward * Time.deltaTime * acceleration * AngleToPlayer/180;

        }
    }

    void CheckGrounded()
    {
        isCarGrounded = Physics.Raycast(transform.position, -transform.up, out hit, 1f, groundLayer);
    }

}
