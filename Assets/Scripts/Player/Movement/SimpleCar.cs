using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleCar : MonoBehaviour
{
    [SerializeField]
    public Rigidbody sphereRB;

    [SerializeField]
    public float maxSpeed = 20f;
    public float fwdSpeed;
    public float revSpeed;
    public float turnSpeed;

    [SerializeField]
    public float wheelRotationSpeed = 10f;
    public float turningSmoothness = 5f;

    [SerializeField]
    public float driftFactor = 0.5f;
    public float ExtremeDriftFactor = 0.8f;
    public float maxDriftFactor = 1.5f;
    public float maxDriftSpeed;
    public float originalMaxSpeed;
    public float speedTransitionTime;

    [SerializeField]
    private float currentSteeringAngleLeft;
    private float currentSteeringAngleRight;
    private float steeringVelocityLeft;
    private float steeringVelocityRight;

    [SerializeField]
    public LayerMask groundLayer;

    [SerializeField]
    private float moveInput;
    private float turnInput;
    public bool isCarGrounded;
    public bool isOnRamp = false;
    public bool isCenterOfMassAdjusting = false; // Flag to prevent multiple coroutines
    private bool isBoosting = false;

    [SerializeField]
    private float normalDrag;
    public float modifiedDrag;

    [SerializeField]
    public Vector3 normalCenterOfMass; // Normal center of mass
    public Vector3 airCenterOfMassOffset; // Initial center of mass offset while in the air
    public Vector3 maxAirCenterOfMassOffset; // Maximum center of mass offset in the air
    public float rampExitForce = 10f;
    public float rampExitTorque = 5f;
    public float angleThreshold = 30f;
    public float offsetIncreaseRate = 1f; // Rate at which the offset increases while in the air
    private float airTime = 0f; // Time the car has been in the air
    public float boostDuration = 2f; // Duration of the boost effect
    public float boosterForce = 20f; // Force to boost forward momentum
    public float rampBoost = 5f;
    public float alignToGroundTime;
    public float rampUpwardsForce;
    public float rampCenterOfMassAdjustmentForce;

    // Current speed variable
    private float currentSpeed;

    [SerializeField]
    // References to wheel transforms
    public Transform frontLeftWheel;
    public Transform frontRightWheel;
    public Transform rearLeftWheel;
    public Transform rearRightWheel;

    [SerializeField]
    public KeyCode driftKey = KeyCode.LeftShift; // Key to control drifting

    private RaycastHit hit; // Declare hit variable

    void Start()
    {
        // Detach Sphere from car
        sphereRB.transform.parent = null;
        originalMaxSpeed = maxSpeed; // Store the original max speed
        normalDrag = sphereRB.drag; // Initialize normal drag
        sphereRB.collisionDetectionMode = CollisionDetectionMode.Continuous;
    }

    void Update()
    {
        // Get Input
        moveInput = Input.GetAxisRaw("Vertical");
        turnInput = Input.GetAxisRaw("Horizontal");

        // Calculate Turning Rotation
        float newRot = turnInput * turnSpeed * Time.deltaTime * moveInput;

        if (isCarGrounded)
            transform.Rotate(0, newRot, 0, Space.World);

        // Set Car's Position to Our Sphere
        transform.position = sphereRB.transform.position;

        // Raycast to the ground and get normal to align car with it.
        isCarGrounded = Physics.Raycast(transform.position, -transform.up, out hit, 1f, groundLayer);

        // Rotate Car to align with ground
        /*
        if (isCarGrounded)
        {
            Quaternion toRotateTo = Quaternion.FromToRotation(transform.up, hit.normal) * transform.rotation;
            transform.rotation = Quaternion.Slerp(transform.rotation, toRotateTo, alignToGroundTime * Time.deltaTime);
        }
        */
        // Calculate Movement Direction
        moveInput *= moveInput > 0 ? fwdSpeed : revSpeed;

        // Adjust drag based on whether the car is grounded
        sphereRB.drag = isCarGrounded ? normalDrag : 0.001f; // Reduced drag in the air

        // Calculate current speed
        currentSpeed = sphereRB.velocity.magnitude;

        // Rotate wheels based on car movement
        //RotateWheels();
    }

    private void FixedUpdate()
    {
        //Debug.Log($"Current Center of Mass: {sphereRB.centerOfMass}");

        if (!isOnRamp)
        {
            // Smoothly reset the center of mass if not on a ramp
            if (sphereRB.centerOfMass != normalCenterOfMass)
            {
                StartCoroutine(AdjustCenterOfMass(normalCenterOfMass, 1f)); // Adjust duration as needed
            }
        }
        if (isCarGrounded)
        {
            sphereRB.AddForce(transform.forward * moveInput, ForceMode.Acceleration); // Add Movement

            sphereRB.angularVelocity = Vector3.zero; // Resets angular velocity to prevent spinning out

            Vector3 gravityForce = Physics.gravity * sphereRB.mass * 4f; // Adjust the multiplier as needed

            airTime = 0f;

            // Determine ramp angle
            float rampAngle = Vector3.Angle(hit.normal, Vector3.up);

            // Apply additional force boost when going up a ramp
            if (rampAngle > 0)
            {
                sphereRB.AddForce(transform.forward * rampBoost, ForceMode.Acceleration);
            }

            // Clamp the velocity if it exceeds the max speed
            if (sphereRB.velocity.magnitude > maxSpeed)
            {
                sphereRB.velocity = sphereRB.velocity.normalized * maxSpeed;
            }

            ApplyDrift();
        }
        else
        {
            Vector3 airControlForce = transform.forward * moveInput * 0.5f; // Adjust force as needed
            sphereRB.AddForce(airControlForce, ForceMode.Acceleration);

            // When in the air, we reduce the impact of gravity to maintain momentum
            Vector3 gravityForce = Physics.gravity * sphereRB.mass * 0.004f; // Adjust the multiplier as needed
            sphereRB.AddForce(gravityForce, ForceMode.Acceleration);

            // Apply air steering control
            float steeringTorque = turnInput * 1f; // Adjust torque strength as needed
            sphereRB.AddTorque(Vector3.up * steeringTorque, ForceMode.Acceleration);

            airTime += Time.deltaTime * 0.2f; // Increase air time
            Vector3 currentOffset = Vector3.Lerp(Vector3.zero, maxAirCenterOfMassOffset, airTime * offsetIncreaseRate);
            sphereRB.centerOfMass = normalCenterOfMass + currentOffset;

            ApplyRampExitForces();
        }
    }

    // Method to rotate wheels
    /*private void RotateWheels()
    {
        // Calculate the rotation amount based on speed
        float rotationAmount = currentSpeed * wheelRotationSpeed * Time.deltaTime;

        // Rotate each wheel around its local X axis
        frontLeftWheel.Rotate(0, rotationAmount, 0);
        frontRightWheel.Rotate(0, rotationAmount, 0);
        rearLeftWheel.Rotate(0, rotationAmount, 0);
        rearRightWheel.Rotate(0, rotationAmount, 0);

        // Calculate the target steering angle
        float targetSteeringAngle = turnInput * turnSpeed;

        // Smoothly interpolate the steering angles for the front wheels
        currentSteeringAngleLeft = Mathf.SmoothDampAngle(currentSteeringAngleLeft, targetSteeringAngle, ref steeringVelocityLeft, turningSmoothness);
        currentSteeringAngleRight = Mathf.SmoothDampAngle(currentSteeringAngleRight, targetSteeringAngle, ref steeringVelocityRight, turningSmoothness);

        // Apply the smooth steering rotation
        frontLeftWheel.localRotation = Quaternion.Euler(frontLeftWheel.localRotation.eulerAngles.x, currentSteeringAngleLeft, frontLeftWheel.localRotation.eulerAngles.z);
        frontRightWheel.localRotation = Quaternion.Euler(frontRightWheel.localRotation.eulerAngles.x, currentSteeringAngleRight, frontRightWheel.localRotation.eulerAngles.z);
    }*/

    void ApplyDrift()
    {
        // Determine the drift factor based on whether drifting is active
        float currentDriftFactor = Input.GetKey(driftKey) ? Mathf.Clamp(ExtremeDriftFactor, 0, maxDriftFactor) : driftFactor;

        // Calculate forward and sideways velocity components
        Vector3 forwardVelocity = transform.forward * Vector3.Dot(sphereRB.velocity, transform.forward);
        Vector3 sidewaysVelocity = transform.right * Vector3.Dot(sphereRB.velocity, transform.right);

        // Apply the drift factor to the sideways velocity for toned-down drifting
        sphereRB.velocity = forwardVelocity + sidewaysVelocity * currentDriftFactor;

        if (Input.GetKey(driftKey))
        {
            // Change the maximum speed while drifting over time
            maxSpeed = Mathf.Lerp(maxSpeed, maxDriftSpeed, Time.deltaTime / speedTransitionTime);
        }
        else
        {
            // Reset to the original maximum speed
            maxSpeed = originalMaxSpeed; // Ensure you store the original max speed in a variable
        }
    }

    private void ApplyRampExitForces()
    {
        // Apply upward force to help keep the car grounded
        sphereRB.AddForce(Vector3.up * rampExitForce, ForceMode.Acceleration);

        // Apply torque to stabilize car orientation
        Vector3 torque = Vector3.zero;
        if (sphereRB.velocity.y < 0) // Only apply torque when falling
        {
            // Calculate torque to keep the car upright
            torque = -transform.right * rampExitTorque;
            sphereRB.AddTorque(torque, ForceMode.Acceleration);
        }

        // Optional: Add a force to push the car towards the ground if necessary
        Vector3 groundCheckDirection = Vector3.down * 5f; // Adjust force strength as needed
        RaycastHit hit;
        if (Physics.Raycast(transform.position, groundCheckDirection, out hit, 10f, groundLayer))
        {
            float rampAngle = Vector3.Angle(hit.normal, Vector3.up);
            if (rampAngle > angleThreshold)
            {
                // Apply additional downward force if the ramp angle is steep
                sphereRB.AddForce(Vector3.down * rampExitForce * 0.5f, ForceMode.Acceleration);
            }
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        // Log collision information for debugging
        //Debug.Log($"Collided with: {collision.collider.name}");

        if (collision.collider.CompareTag("Ramp"))
        {
            Debug.Log("Collided with a ramp!");
            isOnRamp = true;

            // Apply a Y-axis boost
            Vector3 boostDirection = Vector3.up * rampUpwardsForce;
            sphereRB.AddForce(boostDirection, ForceMode.Impulse);

            // Adjust the center of mass
            Vector3 newCenterOfMass = normalCenterOfMass + new Vector3(0, 0, rampCenterOfMassAdjustmentForce);
            if (!isCenterOfMassAdjusting)
            {
                StartCoroutine(AdjustCenterOfMass(newCenterOfMass, 1f));
                isCenterOfMassAdjusting = true;
            }
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Booster") && !isBoosting)
        {
            Debug.Log("Triggered by booster!");
            isBoosting = true;
            sphereRB.AddForce(transform.forward * boosterForce, ForceMode.Impulse);
            StartCoroutine(BoostDuration());
        }
    }

    void OnCollisionExit(Collision collision)
    {
        if (collision.collider.CompareTag("Ramp"))
        {
            isOnRamp = false;

            // Reset the center of mass after leaving the ramp
            if (isCenterOfMassAdjusting)
            {
                StartCoroutine(AdjustCenterOfMass(normalCenterOfMass, 1f));
                isCenterOfMassAdjusting = false;
            }
        }
    }

    private IEnumerator AdjustCenterOfMass(Vector3 targetCenterOfMass, float duration)
    {
        Vector3 startCenterOfMass = sphereRB.centerOfMass;
        float time = 0;

        while (time < duration)
        {
            sphereRB.centerOfMass = Vector3.Lerp(startCenterOfMass, targetCenterOfMass, time / duration);
            time += Time.deltaTime;
            yield return null;
        }

        sphereRB.centerOfMass = targetCenterOfMass; // Ensure final value is set
    }

    private IEnumerator BoostDuration()
    {
        yield return new WaitForSeconds(boostDuration);
        isBoosting = false;
    }
}