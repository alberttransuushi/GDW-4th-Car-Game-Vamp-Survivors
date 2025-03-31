using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plow : PrimaryWeapon
{
    [SerializeField] float startingSize;
    [SerializeField] float sizeIncreaseOnActive;
    [SerializeField] float damage;
    [SerializeField] float knockBackStrength;

    [SerializeField] float stunDuration;
    // Start is called before the first frame update
    public override void Start()
    {
        base.Start();
    }

    // Update is called once per frame
    public override void Update()
    {
        base.Update();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.CompareTag("Enemy"))
        {
            StartCoroutine(collision.gameObject.GetComponent<BaseEnemy>().Stun(stunDuration));
            collision.gameObject.GetComponent<BaseEnemy>().takeDamge(damage);
            Vector3 knockbackVector = (collision.transform.position - transform.position).normalized * knockBackStrength;
            collision.gameObject.GetComponent<Rigidbody>().AddForce(knockbackVector, ForceMode.Impulse);
            collision.gameObject.GetComponent<Rigidbody>().AddForce(collision.transform.up * knockBackStrength / 1.5f, ForceMode.Impulse);

            player.gameObject.GetComponent<PlayerCar>().TakeDamage(-damage * 2);
        }
    }
}
