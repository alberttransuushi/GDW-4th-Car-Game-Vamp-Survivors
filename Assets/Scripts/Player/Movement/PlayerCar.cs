using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PlayerCar : MonoBehaviour
{
    [SerializeField]
    [Header("Health Stats")]
    float currentHP;
    [SerializeField] public float maxHP;
    [SerializeField] float IframeDuration;
    bool damagable = true;
    [SerializeField] float ramDamage = 1f;
    [Space(10)]

    [Header("Movement Stats")]
    [SerializeField] public float maxLandSpeed;
    [SerializeField] float acceleration;
    [SerializeField] float turnSpeed;
    [SerializeField] float turnAngle;
    // 0 = no friction/slidey | 1 = no momentum from drifting
    float currentDriftFriction;
    [SerializeField] float setDriftFriction;
    [SerializeField] float driftTurnSpeedModifier;
    [SerializeField] float unGroundedGravity;
    bool AOAEnabled;
    [Space(10)]



    Rigidbody rb;
    Vector3 dirToTurn;

    [Header("Misc Stats")]
    [SerializeField] LayerMask groundLayer;
    [SerializeField] LayerMask enemyLayer;
    [SerializeField] GameObject centerOfMass;
    [SerializeField] Slider hpSlider;
    [SerializeField] float unStuckCooldown;
    float unStuckTimer;
    bool canUnStuck = true;
    [Space(10)]




    RaycastHit hit;
    public bool isDrifting;


    [Header("Controls")]
    //All our input objects, cause yay I guess
    [SerializeField]
    private InputActionReference movementControl;
    [SerializeField]
    private InputActionReference driftControl;
    [SerializeField]
    private InputActionReference fireControl;
    [SerializeField]
    private InputActionReference AoADriftControl;
    [SerializeField]
    private InputActionReference UnstuckControl;



    private void OnEnable()
    {
        movementControl.action.Enable();
        driftControl.action.Enable();
        fireControl.action.Enable();
        AoADriftControl.action.Enable();
        UnstuckControl.action.Enable();

    }

    private void OnDisable()
    {
        movementControl.action.Disable();
        driftControl.action.Disable();
        fireControl.action.Disable();
        AoADriftControl.action.Disable();
        UnstuckControl.action.Disable();

    }

    // Start is called before the first frame update
    public void Start()
    {
        currentHP = maxHP;
        rb = GetComponent<Rigidbody>();
        rb.centerOfMass = centerOfMass.gameObject.transform.localPosition;
        currentDriftFriction = setDriftFriction;
    }

    // Update is called once per frame
    void Update()
    {
        Movement();

        AOALimiter();

        UpdateHP();

    }


    public void Movement()
    {
        Steering();


        if (CheckGrounded())
        {

            AccelDeccel();


        }
        else
        {

            IncreasedGravity(unGroundedGravity);
        }

    }

    void AccelDeccel()
    {
        if (!AOAEnabled)
        {
            if (movementControl.action.ReadValue<Vector2>().y > 0)
            {
                rb.velocity += transform.forward * Time.deltaTime * acceleration;
                //Debug.Log("Should be moving forward, Vector 2 is: " + movementControl.action.ReadValue<Vector2>().x + movementControl.action.ReadValue<Vector2>().y);
            }
            if (movementControl.action.ReadValue<Vector2>().y < 0)
            {
                rb.velocity -= transform.forward * Time.deltaTime * acceleration;
                //Debug.Log("Should be moving backwards, Vector 2 is: " + movementControl.action.ReadValue<Vector2>().x + movementControl.action.ReadValue<Vector2>().y);
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
        if (driftControl.action.WasPressedThisFrame())
        {
            isDrifting = true;
        }
        else if (driftControl.action.WasReleasedThisFrame())
        {
            isDrifting = false;
        }

        if (UnstuckControl.action.triggered && canUnStuck)
        {
            canUnStuck = false;
            transform.position = transform.position + (Vector3.up * 5f);
            rb.velocity = Vector3.zero;
            StartCoroutine(StartUnStuckCooldown(unStuckCooldown));
        }
        FrictionVelocity();

    }

    void Steering()
    {
        dirToTurn = gameObject.transform.forward;
        if (movementControl.action.ReadValue<Vector2>().x < 0)
        {
            dirToTurn = Quaternion.AngleAxis(-turnAngle, Vector3.up) * transform.forward;
            //Debug.Log("Should be turning left, Vector 2 is: " + movementControl.action.ReadValue<Vector2>().x + movementControl.action.ReadValue<Vector2>().y);
        }
        if (movementControl.action.ReadValue<Vector2>().x > 0)
        {
            dirToTurn = Quaternion.AngleAxis(turnAngle, Vector3.up) * transform.forward;
            //Debug.Log("Should be turning right, Vector 2 is: " + movementControl.action.ReadValue<Vector2>().x + movementControl.action.ReadValue<Vector2>().y);
        }
        if (rb.velocity.magnitude >= 0)
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
        }
        else
        {
            transform.rotation = Quaternion.LookRotation(Vector3.RotateTowards(transform.forward, new Vector3(dir.x, dir.y, dir.z), (turnSpeed + driftTurnSpeedModifier) * Time.deltaTime, 10.0f));
        }

    }

    bool CheckGrounded()
    {
        if (Physics.Raycast(transform.position, -transform.up, out hit, 2f, groundLayer) || Physics.Raycast(transform.position, -transform.up, out hit, 2f, enemyLayer))
        {
            return true;
        }
        else
        {
            return false;
        }

    }

    void FrictionVelocity()
    {
        //Apply Friction caused by drifting
        Vector3 perpendicularVelocity = transform.right * Vector3.Dot(transform.right, rb.velocity);
        float perpendiuclarSpeed = perpendicularVelocity.magnitude * currentDriftFriction;
        rb.velocity -= perpendicularVelocity.normalized * perpendiuclarSpeed;


    }


    void IncreasedGravity(float inc)
    {
        rb.velocity -= new Vector3(0, inc, 0);
    }

    void AOALimiter()
    {
        if (isDrifting && AoADriftControl.action.WasPressedThisFrame())
        {
            AOAEnabled = true;
            currentDriftFriction = 0;
            Time.timeScale = 0.6f;
        }
        if ((!isDrifting || AoADriftControl.action.WasReleasedThisFrame()) && AOAEnabled)
        {
            AOAEnabled = false;
            currentDriftFriction = setDriftFriction;
            rb.velocity = rb.velocity.magnitude * transform.forward;
            Time.timeScale = 1.0f;
        }
    }


    bool Held(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            return true;
        }
        if (context.canceled)
        {
            return false;
        }
        return false;
    }


    public void TakeDamage(float damage)
    {

        if (damagable)
        {

            currentHP -= damage;
            damagable = false;
            if (currentHP <= 0)
            {
                Destroy(this.gameObject);
            }

            StartCoroutine(EnforceIFrames(IframeDuration));

        }

    }


    IEnumerator EnforceIFrames(float deltaTime)
    {


        yield return new WaitForSeconds(deltaTime);

        damagable = true;
    }

    public void RepairDamage(float repairValue)
    {
        if (currentHP < maxHP)
        {
            currentHP += repairValue;
        }

        if (currentHP > maxHP)
        {
            currentHP = maxHP;
        }
    }

    public void UpdateHP()
    {
        hpSlider.value = currentHP / maxHP;
    }

    IEnumerator StartUnStuckCooldown(float time)
    {
        yield return new WaitForSeconds(time);
        canUnStuck = true;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            collision.gameObject.GetComponent<BaseEnemy>().takeDamge(ramDamage * rb.velocity.magnitude / 10);
        }
    }
}
