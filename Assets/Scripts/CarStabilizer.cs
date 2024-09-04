using UnityEngine;

public class CarStabilizer : MonoBehaviour
{
    public Rigidbody carRigidbody;
    public float stabilizationForce = 500f; // Adjust this value to control the stabilization strength
    public float stabilizationSpeed = 10f; // Adjust this for how quickly the stabilization is applied

    void FixedUpdate()
    {
        // Apply stabilization torque
        Vector3 predictedUp = Quaternion.AngleAxis(carRigidbody.angularVelocity.magnitude * Mathf.Rad2Deg * stabilizationSpeed / stabilizationForce, carRigidbody.angularVelocity) * transform.up;
        Vector3 torqueVector = Vector3.Cross(predictedUp, Vector3.up);
        carRigidbody.AddTorque(torqueVector * stabilizationForce);
    }
}