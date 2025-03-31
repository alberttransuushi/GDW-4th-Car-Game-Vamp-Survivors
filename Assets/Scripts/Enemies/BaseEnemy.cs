using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseEnemy : MonoBehaviour {

  [SerializeField] float health;
  [SerializeField] float maxHealth;
  [SerializeField] protected GameObject playerCar;
  [SerializeField] protected float catchUpBonus;
  [SerializeField] protected float distancePerCatchUp;
  [SerializeField] protected float collisionDamage;
  protected Rigidbody rb;
  [SerializeField] GameObject centerOfMass;
  public GameObject targetObject;
  public bool targetingPlayer;
  public bool isPlayerAhead;

  [SerializeField]
  public int targetIndex;
  [SerializeField] float despawnRange = 2000f;

  [SerializeField] protected float turnSpeed;
  public float AngleToTarget;

  [SerializeField] LayerMask groundLayer;
  RaycastHit hit;
  [SerializeField] protected float height;

    [SerializeField] public bool isStunned;
    public virtual void Awake() {
    rb = GetComponent<Rigidbody>();
    rb.centerOfMass = centerOfMass.gameObject.transform.localPosition;

  }

  public virtual void LateUpdate() {
    IsTargetingPlayer();
    CheckDespawnCondition();
    isPlayerAhead = IsPlayerAhead();
  }


  void IsTargetingPlayer() {
    if (targetObject == playerCar) {
      targetingPlayer = true;
    } else {
      targetingPlayer = false;
    }
  }

    public IEnumerator Stun(float stunDuration)
    {
        isStunned = true;
        yield return new WaitForSeconds(stunDuration);
        isStunned = false;
    }

    public virtual void OnEnable() {
    health = maxHealth;
    rb.velocity = Vector3.zero;
    rb.angularVelocity = Vector3.zero;
    rb.centerOfMass = centerOfMass.gameObject.transform.localPosition;
    //GameObject closestTarget = FindClosestTarget();
    //SetRandomIndex();
    //targetObject = closestTarget.GetComponentInParent<CheckPointManager>().nextCheckPoint.targets[targetIndex];



  }

  public GameObject FindClosestTarget() {
    GameObject[] gameObjects = GameObject.FindGameObjectsWithTag("target");
    GameObject closestTarget = null;

    float minDistance = Mathf.Infinity;
    foreach (GameObject gameObject in gameObjects) {

      float distance = Vector3.Distance(transform.position, gameObject.transform.position);
      if (distance < minDistance) {
        minDistance = distance;
        closestTarget = gameObject;
      }

    }
    return closestTarget;
  }

  public void SetRandomIndex() {
    SetTargetIndex(Random.Range(0, 5));
  }

  public void SetTargetIndex(int index) {
    targetIndex = index;
  }

  public virtual void CheckAlive() {
    if (health < 0) {
      gameObject.GetComponent<EnemyExp>().DropExp();
      EventManager.enemiesKilled += 1;


      PoolManager.ReturnObjectToPool(gameObject);
    }
  }
  public void takeDamge(float damage) {
    health -= damage;
  }

  protected bool CheckGrounded() {
    return Physics.Raycast(transform.position, -transform.up, out hit, height / 2, groundLayer);
  }

  public float CheckDistanceToTarget() {

    return (gameObject.transform.position - targetObject.transform.position).magnitude;

  }

  public virtual float GetCatchUpBonus() {
    //print(Mathf.Floor(CheckDistanceToPlayer() / distancePerCatchUp) * catchUpBonus);

    return Mathf.Floor(CheckDistanceToTarget() / distancePerCatchUp) * catchUpBonus;

  }

  public virtual void OnCollisionEnter(Collision collision) {
    if (collision.gameObject == playerCar) {
      playerCar.GetComponent<PlayerCar>().TakeDamage(collisionDamage);

    }
  }

  protected Vector3 GetDirToTarget() {

    Vector3 dirToTarget = targetObject.transform.position - transform.position;
    return dirToTarget;

  }
  protected void TurnToTarget() {

    Vector3 dirToTarget = GetDirToTarget();

    if (TargetIsRight()) {
      Quaternion turn = Quaternion.Euler(0, turnSpeed * Time.deltaTime, 0);
      rb.MoveRotation(rb.rotation * turn);
    } else {
      Quaternion turn = Quaternion.Euler(0, -turnSpeed * Time.deltaTime, 0);
      rb.MoveRotation(rb.rotation * turn);
    }

    //transform.rotation = Quaternion.LookRotation(Vector3.RotateTowards(transform.forward, new Vector3(dirToTarget.x, 0, dirToTarget.z), turnSpeed * Time.deltaTime, 10.0f));

    AngleToTarget = Vector3.Angle(dirToTarget, transform.forward);

  }


  protected bool TargetIsRight() {
    Vector3 targetDir = (targetObject.transform.position - transform.position).normalized;
    float dot = Vector3.Dot(transform.right, targetDir);
    return dot > 0;
  }

  protected bool IsPlayerAhead() {
    Vector3 targetDir = (playerCar.transform.position - transform.position).normalized;
    float dot = Vector3.Dot(transform.forward, targetDir);

    return dot > 0;
  }

  void CheckDespawnCondition() {
    if (Vector3.Distance(gameObject.transform.position, playerCar.transform.position) >= despawnRange) {
      PoolManager.ReturnObjectToPool(gameObject);
    }
  }
  public void SwitchTarget(GameObject target) {
    targetObject = target;
  }
}
