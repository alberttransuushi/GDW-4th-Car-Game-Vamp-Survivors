using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseEnemy : MonoBehaviour
{
    [SerializeField] float health;
    [SerializeField] protected GameObject playerCar;


    public virtual void CheckAlive()
    {
        if (health < 0)
        {
            Destroy(gameObject);
        }
    }
}
