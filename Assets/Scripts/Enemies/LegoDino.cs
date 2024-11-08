using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LegoDino : BaseEnemy
{
    [SerializeField] float minDistanceToThrow;
    [SerializeField] float maxDistanceToThrow;
    [SerializeField] float throwCooldown;
    [SerializeField] float throwAnimDelay;
    [SerializeField] float attackExhaustDuration;
    public bool canAttack;
    [SerializeField] float initialThrowXZSpeed;
    [SerializeField] float initialThrowYSpeed;
    [SerializeField] float collisionKnockBack;

    [SerializeField] GameObject boulderPrefab;
    [SerializeField] GameObject boulderSpawner;

    bool canWalk;
    Animator animator;


    [SerializeField] float walkSpeed;
    
    // Start is called before the first frame update
    public override void Start()
    {
        base.Start();
        playerCar = GameObject.FindGameObjectWithTag("Player");
        canAttack = true;
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        CheckAlive();
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
            //rb.velocity = Vector3.zero;

        } 
    }




    void WalkToPlayer()
    {
        print(CheckGrounded());
        if (canWalk && CheckGrounded())
        {
            Vector3 dirToPlayer = GetDirToPlayer().normalized;
            rb.velocity = new Vector3(dirToPlayer.x,0,dirToPlayer.z) * walkSpeed;
        }
        if (!CheckGrounded())
        {
            IncreasedGravity(1f);
        }
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
        animator.SetTrigger("Attack");
        //rb.velocity = Vector3.zero;

        //print("StartDelay");
        yield return new WaitForSeconds(throwAnimDelay);

        //print("Throw");
        GameObject boulder = Instantiate(boulderPrefab, boulderSpawner.transform.position, transform.rotation);
        boulder.transform.rotation = Quaternion.LookRotation(GetDirToPlayer().normalized); 

        boulder.GetComponent<Rigidbody>().velocity = new Vector3(1, 0, 1) * initialThrowXZSpeed;
        boulder.GetComponent<Rigidbody>().velocity += Vector3.up * initialThrowYSpeed;

        yield return new WaitForSeconds(attackExhaustDuration);

        canWalk = true;

        yield return new WaitForSeconds(throwCooldown);
        //print("end");
        canAttack = true;
        


    }

    void TailAttack()
    {
        if (playerCar.GetComponent<PlayerCar>().damagable)
        {
            Debug.Log("MyBoy");

            playerCar.GetComponent<Rigidbody>().AddForce(transform.forward.normalized * collisionKnockBack * 2f, ForceMode.Acceleration);
            rb.AddForce(-transform.forward.normalized * collisionKnockBack * 2f, ForceMode.Acceleration);

            playerCar.GetComponent<PlayerCar>().TakeDamage(collisionDamage);

        }

        
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player")
        {

            TailAttack();

        }
    }



    void IncreasedGravity(float inc)
    {
        rb.velocity -= new Vector3(0, inc, 0);
    }
}
