using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LegoDino : BaseEnemy
{
    [SerializeField] float minDistanceToThrow;
    [SerializeField] float maxDistanceToThrow;
    [SerializeField] float throwTimer;
    [SerializeField] float throwCooldown;

    float turnSpeed;
    bool canWalk;
    
    // Start is called before the first frame update
    public override void Start()
    {
        base.Start();
        playerCar = GameObject.Find("player");
    }

    // Update is called once per frame
    void Update()
    {
        FacePlayer();
    }
    
    void AdaptPlayerDistannce()
    {
        if (maxDistanceToThrow < CheckDistanceToPlayer() && CheckDistanceToPlayer() < minDistanceToThrow)
        {
            canWalk = false;
            
        }
    }

    void FacePlayer()
    {

    }
}
