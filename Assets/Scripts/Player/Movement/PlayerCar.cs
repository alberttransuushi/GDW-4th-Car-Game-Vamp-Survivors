using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerCar : MonoBehaviour {

  [SerializeField] public float maxLandSpeed;
  [SerializeField] float acceleration;

  [SerializeField] float turnSpeed;
  [SerializeField] float turnAngle;

  // 0 = no friction/slidey | 1 = no momentum from drifting
  float currentDriftFriction;
  [SerializeField] float setDriftFriction;
  [SerializeField] float driftTurnSpeedModifier;

  Rigidbody rb;
  [SerializeField] Vector3 dirToTurn;
  [SerializeField] LayerMask groundLayer;
  [SerializeField] GameObject centerOfMass;

  [SerializeField] float unGroundedGravity;

  public bool AOAEnabled;

  RaycastHit hit;
  public bool isDrifting;

  //All our input objects, cause yay I guess
  [SerializeField]
  private InputActionReference movementControl;
  [SerializeField]
  private InputActionReference driftControl;
  [SerializeField]
  private InputActionReference fireControl;

  private void OnEnable() {
    movementControl.action.Enable();
    driftControl.action.Enable();
    fireControl.action.Enable();

  }

  private void OnDisable() {
    movementControl.action.Disable();
    driftControl.action.Disable();
    fireControl.action.Disable();

  }

  // Start is called before the first frame update
  void Start() {
    rb = GetComponent<Rigidbody>();
    rb.centerOfMass = centerOfMass.gameObject.transform.localPosition;
    currentDriftFriction = setDriftFriction;
  }

  // Update is called once per frame
  void Update() {
    Movement();

    AOALimiter();


  }


  public void Movement() {
    Steering();


    if (CheckGrounded()) {

      AccelDeccel();


    } else {
            
      IncreasedGravity(unGroundedGravity);
    }

  }

  void AccelDeccel() {
    if (!AOAEnabled) {
      if (movementControl.action.ReadValue<Vector2>().y > 0) {
        rb.velocity += transform.forward * Time.deltaTime * acceleration;
        Debug.Log("Should be moving forward, Vector 2 is: " + movementControl.action.ReadValue<Vector2>().x + movementControl.action.ReadValue<Vector2>().y);
      }
      if (movementControl.action.ReadValue<Vector2>().y < 0) {
        rb.velocity -= transform.forward * Time.deltaTime * acceleration;
        Debug.Log("Should be moving backwards, Vector 2 is: " + movementControl.action.ReadValue<Vector2>().x + movementControl.action.ReadValue<Vector2>().y);
      }
    }
    /**
    if (driftControl.action.triggered) {
      isDrifting = true;
    } else {
      isDrifting = false;
    }
    **/

    //You can remove this and uncomment the one up top
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

  void Steering() {
    dirToTurn = gameObject.transform.forward;
    if (movementControl.action.ReadValue<Vector2>().x < 0) {
      dirToTurn = Quaternion.AngleAxis(-turnAngle, Vector3.up) * transform.forward;
      Debug.Log("Should be turning left, Vector 2 is: " + movementControl.action.ReadValue<Vector2>().x + movementControl.action.ReadValue<Vector2>().y);
    }
    if (movementControl.action.ReadValue<Vector2>().x > 0) {
      dirToTurn = Quaternion.AngleAxis(turnAngle, Vector3.up) * transform.forward;
      Debug.Log("Should be turning right, Vector 2 is: " + movementControl.action.ReadValue<Vector2>().x + movementControl.action.ReadValue<Vector2>().y);
    }
    if (rb.velocity.magnitude > 5) {
      TurnToWheel(dirToTurn);
      //print("Turning");
    }
  }
  void TurnToWheel(Vector3 dir) {
    //Add More Turn Speed when drifting
    if (!isDrifting) {
      transform.rotation = Quaternion.LookRotation(Vector3.RotateTowards(transform.forward, new Vector3(dir.x, dir.y, dir.z), turnSpeed * Time.deltaTime, 10.0f));
    } else {
      transform.rotation = Quaternion.LookRotation(Vector3.RotateTowards(transform.forward, new Vector3(dir.x, dir.y, dir.z), (turnSpeed + driftTurnSpeedModifier) * Time.deltaTime, 10.0f));
    }

  }

  bool CheckGrounded() {
    return Physics.Raycast(transform.position, -transform.up, out hit, 1f, groundLayer);
  }

  void FrictionVelocity() {
    //Apply Friction caused by drifting
    Vector3 perpendicularVelocity = transform.right * Vector3.Dot(transform.right, rb.velocity);
    float perpendiuclarSpeed = perpendicularVelocity.magnitude * currentDriftFriction;
    rb.velocity -= perpendicularVelocity.normalized * perpendiuclarSpeed;


  }


  void IncreasedGravity(float inc) {
        rb.velocity -= new Vector3(0, inc, 0);
  }

  void AOALimiter() {
    if (isDrifting && Input.GetKeyDown(KeyCode.Space)) {
      AOAEnabled = true;
      currentDriftFriction = 0;
    }
    if ((!isDrifting || Input.GetKeyUp(KeyCode.Space)) && AOAEnabled) {
      AOAEnabled = false;
      currentDriftFriction = setDriftFriction;
      rb.velocity = rb.velocity.magnitude * transform.forward;
    }
  }

  bool Held(InputAction.CallbackContext context) {
    if (context.performed) {
      return true;
    }
    if (context.canceled) {
      return false;
    }
    return false;
  }
}
