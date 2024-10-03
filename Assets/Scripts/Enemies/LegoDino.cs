using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LegoDino : BaseEnemy
{
    [SerializeField] float minDistanceToThrow;
    [SerializeField] float maxDistanceToThrow;
    [SerializeField] float throwTimer;
    [SerializeField] float throwCooldown;
    [SerializeField] float throwAnimDelay;
    [SerializeField] GameObject boulderPrefab;
    [SerializeField] GameObject boulderSpawner;

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
        AdaptPlayerDistannce();
    }
    
    void AdaptPlayerDistannce()
    {
        if (maxDistanceToThrow < CheckDistanceToPlayer() && CheckDistanceToPlayer() < minDistanceToThrow)
        {
            canWalk = false;
            Throwing();
        }
    }

    void FacePlayer()
    {

    }

    void Throwing() { 
    
        StartCoroutine(ThrowDelay());

    }

    IEnumerator ThrowDelay()
    {

        yield return new WaitForSeconds(throwAnimDelay);

        GameObject boulder = Instantiate(boulderPrefab, boulderSpawner.transform.position, transform.rotation);



    }
}
