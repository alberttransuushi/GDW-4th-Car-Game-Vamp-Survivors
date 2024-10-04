using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseEnemy : MonoBehaviour
{
    [SerializeField] float health;
    [SerializeField] protected GameObject playerCar;
    [SerializeField] protected float catchUpBonus;
    [SerializeField] protected float distancePerCatchUp;
    [SerializeField] protected float collisionDamage;
    protected Rigidbody rb;
    [SerializeField] GameObject centerOfMass;

    [SerializeField] protected float turnSpeed;
    public float AngleToPlayer;

    [SerializeField] LayerMask groundLayer;
    RaycastHit hit;
    [SerializeField] protected float height;


    public virtual void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.centerOfMass = centerOfMass.gameObject.transform.localPosition;
    }

    public virtual void CheckAlive()
    {
        if (health < 0)
        {
            gameObject.GetComponent<EnemyExp>().DropExp();
            Destroy(gameObject);

        }
    }
    public void takeDamge(float damage)
    {
        health -= damage;
    }

    protected bool CheckGrounded()
    {
        return Physics.Raycast(transform.position, -transform.up, out hit, height/2, groundLayer);
    }

    public float CheckDistanceToPlayer()
    {

        return (gameObject.transform.position - playerCar.transform.position).magnitude;

    }

    public virtual float GetCatchUpBonus()
    {
        //print(Mathf.Floor(CheckDistanceToPlayer() / distancePerCatchUp) * catchUpBonus);
        return Mathf.Floor(CheckDistanceToPlayer() / distancePerCatchUp) * catchUpBonus;
    }

    public virtual void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject == playerCar)
        {
            playerCar.GetComponent<PlayerCar>().TakeDamage(collisionDamage);

        }
    }

    protected Vector3 GetDirToPlayer()
    {
        Vector3 dirToPlayer = playerCar.transform.position - transform.position;
        return dirToPlayer;
    }
    protected void TurnToPlayer()
    {
        Vector3 dirToPlayer = GetDirToPlayer();

        transform.rotation = Quaternion.LookRotation(Vector3.RotateTowards(transform.forward, new Vector3(dirToPlayer.x, 0, dirToPlayer.z), turnSpeed * Time.deltaTime, 10.0f));

        AngleToPlayer = Vector3.Angle(dirToPlayer, transform.forward);
    }

}
