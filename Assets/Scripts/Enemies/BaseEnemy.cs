using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseEnemy : MonoBehaviour
{
    [SerializeField] float health;
    [SerializeField] protected GameObject playerCar;
    [SerializeField] protected float catchUpBonus;
    [SerializeField] protected float distancePerCatchUp;


    public virtual void CheckAlive()
    {
        if (health < 0)
        {
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

}
