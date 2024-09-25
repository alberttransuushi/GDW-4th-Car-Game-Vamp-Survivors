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

    public float CheckDistanceToPlayer()
    {

        return (gameObject.transform.position - playerCar.transform.position).magnitude;

    }

    public float GetCatchUpBonus()
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

}
