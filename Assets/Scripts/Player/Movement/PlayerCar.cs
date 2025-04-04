using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using TMPro;
using Unity.VisualScripting;

public class PlayerCar : MonoBehaviour
{
    [SerializeField]
    [Header("Health Stats")]
    public float currentHP;
    [SerializeField] public float maxHP;
    [SerializeField] float IframeDuration;
    public bool damagable = true;
    [SerializeField] float ramDamage = 1f;
    [SerializeField] float explosionDamage;
    [Space(10)]

    [Header("Movement Stats")]
    [SerializeField] public float maxLandSpeed;
    public float boostLeft;
    [SerializeField] float boostAdded;
    [SerializeField] float boostMultiplier;
    [SerializeField] bool canBoost;
    [SerializeField] float boostCooldownTime = 5f;

    [SerializeField] float acceleration;
    [SerializeField] float turnSpeed;
    [SerializeField] float downwardForceMultiplier = 1f;
    [SerializeField] float downwardForceMultiplierDrifting = 1f;
    //[SerializeField] float turnAngle;
    // 0 = no friction/slidey | 1 = no momentum from drifting
    float currentDriftFriction;
    [SerializeField] float setDriftFriction;
    [SerializeField] float driftFrictionModifier;

    
    
    [SerializeField] float driftTurnSpeedModifier;
    [SerializeField] float breakStrength = 0.05f;
    [SerializeField] float unGroundedGravity;
    [SerializeField] Slider accelorometer;
    [SerializeField] TMP_Text speedText;
    bool AOAEnabled;
    [Space(10)]



    Rigidbody rb;
    Vector3 dirToTurn;

    [Header("Misc Stats")]
    [SerializeField] LayerMask groundLayer;
    [SerializeField] float playerHeight = 2f;
    [SerializeField] LayerMask enemyLayer;
    [SerializeField] GameObject centerOfMass;
    [SerializeField] Slider hpSlider;
    [SerializeField] float unStuckCooldown;
    float unStuckTimer;
    bool canUnStuck = true;
    [Space(10)]


    [SerializeField] float unstuckAoeRange;
    [SerializeField] float unstuckExplosionStrength;
    [SerializeField] float unstuckPlayerDamage;
    [SerializeField] bool unstuckJumpFeature;
    [SerializeField] float unstuckBoost;


    RaycastHit hit;
    public bool isDrifting;


    [Header("Controls")]
    //All our input objects, cause yay I guess
    [SerializeField]
    public InputActionReference movementControl;
    [SerializeField]
    public InputActionReference driftControl;
    [SerializeField]
    public InputActionReference fireControl;
    [SerializeField]
    public InputActionReference AoADriftControl;
    [SerializeField]
    public InputActionReference UnstuckControl;
    [SerializeField]
    public InputActionReference boostControl;
    [SerializeField]
    public InputActionReference pauseControl;

    [Header("audio")]
    public AudioSource audioSource;
    [SerializeField]
    AudioClip startEngine;
    [SerializeField]
    AudioClip enginenoise;
    [SerializeField]
    AudioClip startDrift;
    [SerializeField]
    AudioClip driftNoise;
    public bool startedEngine;
    public bool driving;
    bool startedDrift;
    float baseVolume;
    bool breaking;


    [SerializeField] float AoeTempTime;
    [SerializeField] GameObject AoeVisualizer;

    public ParticleSystem driftSpark;

    [SerializeField] WheelCollider FRWheel;
    [SerializeField] WheelCollider FLWheel;
    [SerializeField] WheelCollider BRWheel;
    [SerializeField] WheelCollider BLWheel;

    public float frontWheelStiffness;
    public float backWheelStiffness;
    public float frontWheelDriftingStiffness;
    public float backWheelDriftingStiffness;

    public float wheelFrictionLerpTo;
    public float wheelFrictionLerpFrom;
    public float maxDriftAngle;

    float driftMaintainSpeed;
    private void OnEnable()
    {
        movementControl.action.Enable();
        driftControl.action.Enable();
        fireControl.action.Enable();
        AoADriftControl.action.Enable();
        UnstuckControl.action.Enable();
        boostControl.action.Enable();
        pauseControl.action.Enable();
    }

    private void OnDisable()
    {
        movementControl.action.Disable();
        driftControl.action.Disable();
        fireControl.action.Disable();
        AoADriftControl.action.Disable();
        UnstuckControl.action.Disable();
        boostControl.action.Disable();
        pauseControl.action.Disable();
    }

    // Start is called before the first frame update
    public void Start()
    {
        currentHP = maxHP;
        rb = GetComponent<Rigidbody>();
        rb.centerOfMass = centerOfMass.gameObject.transform.localPosition;
        currentDriftFriction = setDriftFriction;
        audioSource = GetComponent<AudioSource>();
        baseVolume = audioSource.volume;
        canBoost = true;
        //driftSpark.Stop();
    }

    // Update is called once per frame
    void Update()
    {
        Movement();

        //AOALimiter();

        UpdateHP();

        Boost();
    }

    
    void Boost()
    {
        if(boostLeft >= 0)
        {
            rb.velocity += transform.forward * Time.deltaTime * boostLeft * boostMultiplier;
            boostLeft -= Time.deltaTime;
        }
        AddBoost();
    }

    void AddBoost()
    {
        if (boostControl.action.IsPressed() && canBoost)
        {
            boostLeft = boostAdded;
            BoostCooldown();
            Debug.Log("BOOST");
        }
    }

    IEnumerator BoostCooldown()
    {
        canBoost = false;
        yield return new WaitForSeconds(boostCooldownTime);
        canBoost = true;
    }

    public void Movement()
    {
        Steering();
        Breaking();

        if (CheckGrounded())
        {

            AccelDeccel();
            ApplyDownwardForce();

            
        }
        else
        {

            IncreasedGravity(unGroundedGravity);
        }
        accelorometer.value = rb.velocity.magnitude / (maxLandSpeed);
        //accelorometer.value = rb.velocity.magnitude / (maxLandSpeed * 2);
        speedText.text = Mathf.RoundToInt(rb.velocity.magnitude).ToString();
        speedText.color = new Color(255, 255f - (rb.velocity.magnitude * 2), 255f - (rb.velocity.magnitude * 4));
        /*
        if (CheckGrounded())
        {
            Quaternion toRotateTo = Quaternion.FromToRotation(transform.up, hit.normal) * transform.rotation;
            transform.rotation = Quaternion.Slerp(transform.rotation, toRotateTo, 1 * Time.deltaTime);
        }*/

    }

    void ApplyDownwardForce()
    {
        rb.AddForce(0, downwardForceMultiplier * 0.00119f * Mathf.Pow(rb.velocity.magnitude, 2), 0, ForceMode.Force);
        //if (!isDrifting) rb.AddForce(0, downwardForceMultiplier * 0.00119f * Mathf.Pow(rb.velocity.magnitude, 2), 0, ForceMode.Force);
        //else rb.AddForce(0, downwardForceMultiplierDrifting * 0.00119f * Mathf.Pow(rb.velocity.magnitude, 2), 0, ForceMode.Force);
    }

    void AccelDeccel()
    {
        driving = false;
        
        
        if (rb.velocity.magnitude <= maxLandSpeed)
        {
            float accel = movementControl.action.ReadValue<Vector2>().y;
            if(accel > 0) { driving = true; }
            //Debug.Log(accel);
            rb.velocity -= transform.forward * Time.deltaTime * acceleration * -accel;
            rb.angularVelocity = Vector3.zero;
        }

        if (driving && !startedEngine)
        {
            audioSource.volume = baseVolume;
            audioSource.loop = false;
            startedEngine = true;
            audioSource.pitch = Random.Range(0.7f, 1.2f);
            Debug.Log(rb.velocity);
            
            audioSource.clip = startEngine;
            audioSource.Play();
        }
        else if (driving)
        {
            audioSource.loop = true;
            if (!audioSource.isPlaying)
            {
                audioSource.clip = enginenoise;
                audioSource.Play();
            }
        }
        else 
        {
            audioSource.volume -= 0.1f * Time.deltaTime;
            audioSource.loop = false;
            startedEngine = false;
        }


        
        
        /*
        //You can remove this and uncomment the one up top
        if (driftControl.action.WasPressedThisFrame())
        {
            isDrifting = true;
            driftSpark.Play();
        }
        else if (driftControl.action.WasReleasedThisFrame())
        {
            isDrifting = false;
            driftSpark.Stop();
        }
        */

        if (UnstuckControl.action.triggered && canUnStuck)
        {
            if (CheckGrounded())
            {
                canUnStuck = false;
                Unstuck();
                StartCoroutine(StartUnStuckCooldown(unStuckCooldown));
            }
        }

        FrictionVelocity();

    }

    void ChangeSidewaysFriction(WheelCollider wheel, float stiffVal)
    {
        if (isDrifting)
        {
            WheelFrictionCurve friction = wheel.sidewaysFriction;
            float wheelFriction = 60 * wheelFrictionLerpTo;
            float val = Mathf.Lerp(friction.stiffness, stiffVal, wheelFriction * Time.deltaTime);
            friction.stiffness = val;
            wheel.sidewaysFriction = friction;
        } else
        {
            WheelFrictionCurve friction = wheel.sidewaysFriction;
            float wheelFriction = 60 * wheelFrictionLerpFrom;
            float val = Mathf.Lerp(friction.stiffness, stiffVal, wheelFriction * Time.deltaTime);
            friction.stiffness = val;
            wheel.sidewaysFriction = friction;
        }
            
    }
    void Steering()
    {
        
        float turn = movementControl.action.ReadValue<Vector2>().x;
        
        TurnToWheel(turn * turnSpeed);

        if (driftControl.action.IsPressed())
        {
            if (FRWheel.sidewaysFriction.stiffness != frontWheelDriftingStiffness)
            {
                driftMaintainSpeed = rb.velocity.magnitude;
                isDrifting = true;
                ChangeSidewaysFriction(FRWheel, frontWheelDriftingStiffness);
                ChangeSidewaysFriction(FLWheel, frontWheelDriftingStiffness);
                ChangeSidewaysFriction(BLWheel, backWheelDriftingStiffness);
                ChangeSidewaysFriction(BRWheel, backWheelDriftingStiffness);
            }

        }
        else
        {
            if (FRWheel.sidewaysFriction.stiffness != frontWheelStiffness)
            {
                isDrifting = false;
                ChangeSidewaysFriction(FRWheel, frontWheelStiffness);
                ChangeSidewaysFriction(FLWheel, frontWheelStiffness);
                ChangeSidewaysFriction(BLWheel, backWheelStiffness);
                ChangeSidewaysFriction(BRWheel, backWheelStiffness);
            }
        }

    }


    void TurnToWheel(float speed)
    { 
        
        float driftAngle = Vector3.Angle(transform.forward, rb.velocity.normalized);
        if (!isDrifting || (isDrifting && driftAngle < maxDriftAngle))
        {
            Quaternion turn = Quaternion.Euler(0, speed * Time.deltaTime, 0);

            rb.MoveRotation(rb.rotation * turn);
        }
        //Debug.Log(Vector3.Angle(transform.forward, rb.velocity.normalized));
    }

   

    public bool CheckGrounded()
    {
        if (Physics.Raycast(transform.position, -transform.up, out hit, playerHeight, groundLayer) || Physics.Raycast(transform.position, -transform.up, out hit, 2f, enemyLayer))
        {
            return true;
        }
        else
        {
            //Debug.Log("false");
            return false;
        }

    }

    void FrictionVelocity()
    {
        //Apply Friction caused by drifting
        Vector3 perpendicularVelocity = transform.right * Vector3.Dot(transform.right, rb.velocity);
        float perpendiuclarSpeed;
        
        perpendiuclarSpeed = perpendicularVelocity.magnitude * currentDriftFriction;
        rb.velocity -= perpendicularVelocity.normalized * perpendiuclarSpeed;


    }


    void IncreasedGravity(float inc)
    {
        rb.velocity -= new Vector3(0, inc, 0);
    }

    void Breaking()
    {
        if (AoADriftControl.action.WasPressedThisFrame())
        {
            breaking = true;
        }
        if ((AoADriftControl.action.WasReleasedThisFrame()) && breaking)
        {
            breaking = false;
        }
        if (breaking == true)
        {
            rb.velocity = rb.velocity / (1 + breakStrength * Time.deltaTime);
        }
    }
    /*
    void AOALimiter()
    {
        if (AoADriftControl.action.WasPressedThisFrame())
        {
            AOAEnabled = true;
            currentDriftFriction = 0;
            Time.timeScale = 0.6f;
        }
        if ((AoADriftControl.action.WasReleasedThisFrame()) && AOAEnabled)
        {
            AOAEnabled = false;
            currentDriftFriction = setDriftFriction;
            rb.velocity = rb.velocity.magnitude * transform.forward;
            Time.timeScale = 1.0f;
        }
    }
    */

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
      EventManager.timesHit += 1;
      EventManager.timeSinceLastHit = 0;
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

    void Unstuck()
    {
        TakeDamage(unstuckPlayerDamage);

        Explode();

        if (unstuckJumpFeature)
        {
            rb.AddForce(unstuckExplosionStrength * Vector3.up / 2f, ForceMode.Acceleration);
        }

        rb.velocity += transform.forward * unstuckBoost;
    }

    public void Explode()
    {

        Vector3 pointOfExplosion = transform.position;

        Collider[] explosionCollider = Physics.OverlapSphere(pointOfExplosion, unstuckAoeRange);

        foreach (Collider collider in explosionCollider)
        {
            if (collider.gameObject.tag == "Enemy")
            {

                collider.GetComponent<Rigidbody>().AddExplosionForce(unstuckExplosionStrength, pointOfExplosion, unstuckAoeRange, 3.0f, ForceMode.Acceleration);

                collider.GetComponent<BaseEnemy>().takeDamge(explosionDamage);

            }
        }
    }




    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            collision.gameObject.GetComponent<BaseEnemy>().takeDamge(ramDamage * rb.velocity.magnitude / 10);
            

        }
    }
}
