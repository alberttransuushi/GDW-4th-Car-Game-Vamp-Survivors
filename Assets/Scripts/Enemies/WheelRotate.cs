using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WheelRotate : MonoBehaviour
{
    //Basic wheel rotation script, when applied to wheel mesh, follows wheel collider rotations and applies to mesh.
    public WheelCollider targetWheel;

    Vector3 wheelPos = new Vector3();
    Quaternion wheelRot = new Quaternion();

    // Update is called once per frame
    void Update()
    {
        targetWheel.GetWorldPose(out wheelPos, out wheelRot);
        transform.position = wheelPos;
        transform.rotation = wheelRot;
    }
}
