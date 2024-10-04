using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LegoDino : BaseEnemy
{
    [SerializeField] float minDistanceToThrow;
    [SerializeField] float maxDistanceToThrow;
    [SerializeField] float throwCooldown;
    [SerializeField] float throwAnimDelay;
    public bool canAttack;
    [SerializeField] float initialThrowXZSpeed;
    [SerializeField] float initialThrowYSpeed;

    [SerializeField] GameObject boulderPrefab;
    [SerializeField] GameObject boulderSpawner;

    bool canWalk;
    [SerializeField] float walkSpeed;
    
    // Start is called before the first frame update
    public override void Start()
    {
        base.Start();
        playerCar = GameObject.Find("player");
        canAttack = true;
    }

    // Update is called once per frame
    void Update()
    {
    
        TurnToPlayer();
        AdaptPlayerDistannce();
        WalkToPlayer();
    }
    
    void AdaptPlayerDistannce()
    {
        //print(CheckDistanceToPlayer());
        if (maxDistanceToThrow > CheckDistanceToPlayer() && CheckDistanceToPlayer() > minDistanceToThrow)
        {
            //Debug.Log("throw");
            Throwing();

        } 
    }




    void WalkToPlayer()
    {
        Vector3 dirToPlayer = GetDirToPlayer().normalized;
        rb.velocity = dirToPlayer * walkSpeed;
    }

    void Throwing() { 
    
        if(canAttack)
        {
            
            StartCoroutine(ThrowDelay());
        }
        
    }

    

    IEnumerator ThrowDelay()
    {
        canAttack = false;
        canWalk = false;
        //print("StartDelay");
        yield return new WaitForSeconds(throwAnimDelay);

        //print("Throw");
        GameObject boulder = Instantiate(boulderPrefab, boulderSpawner.transform.position, transform.rotation);
        boulder.transform.rotation = Quaternion.LookRotation(GetDirToPlayer().normalized); 

        boulder.GetComponent<Rigidbody>().velocity = new Vector3(1, 0, 1) * initialThrowXZSpeed;
        boulder.GetComponent<Rigidbody>().velocity += Vector3.up * initialThrowYSpeed;

        canWalk = true;

        yield return new WaitForSeconds(throwCooldown);
        //print("end");
        canAttack = true;
        


    }

    IEnumerator TailDelay()
    {
        //print("StartDelay");
        canAttack = false;
        canWalk = false;
        yield return new WaitForSeconds(throwAnimDelay);

        //print("THROW");


        GameObject boulder = Instantiate(boulderPrefab, boulderSpawner.transform.position, transform.rotation);

        boulder.GetComponent<Rigidbody>().velocity = new Vector3(1, 0, 1) * initialThrowXZSpeed;
        boulder.GetComponent<Rigidbody>().velocity += Vector3.up * initialThrowYSpeed;


        canWalk = true;

        yield return new WaitForSeconds(throwCooldown);

        canAttack = true;

        //print("OVER");
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            if(canAttack)
            {
                TailDelay();
            }
        }
    }
}
