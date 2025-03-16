using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissileProjectile : MonoBehaviour
{
    public GameObject target;
    [SerializeField] float speed;
    [SerializeField, Range(0,0.75f)] float trackingDelay;
    [SerializeField] float aoeRange;
    [SerializeField] float damage;
    [SerializeField] float damageFalloff;
    [SerializeField] float explosionKnockback;
    [SerializeField] bool tracking;
    [SerializeField] float turnSpeed;
    [SerializeField] ParticleSystem fireParticles;
    [SerializeField] GameObject explosionParticles;

    //[SerializeField] float trackingSpeed;

    Rigidbody rb;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(DelayTracking());
        rb = GetComponent<Rigidbody>();
        fireParticles.Play();

        //rb.velocity = new Vector3(0, 200, 0);

    }

    // Update is called once per frame
    void Update()
    {
        rb.velocity = transform.forward * speed;
        speed += Time.deltaTime;
        if (tracking)
        {
            TrackTarget();
            if (!fireParticles.isPlaying)
            {
                fireParticles.Play();
            }
        }
        if(target == null)
        {
            Destroy(this.gameObject);
        }
    }

    void TrackTarget()
    {
        Vector3 dirToTarget = target.transform.position - transform.position;

        transform.rotation = Quaternion.LookRotation(Vector3.RotateTowards(transform.forward, new Vector3(dirToTarget.x, dirToTarget.y, dirToTarget.z), turnSpeed * Time.deltaTime, 10.0f));

        
    }

    IEnumerator DelayTracking()
    {
        //print("YEs");
        yield return new WaitForSeconds(trackingDelay);

        tracking = true;

    }

    private void OnCollisionEnter(Collision collision)
    {
        Vector3 pointOfExplosion = collision.contacts[0].point;

        Collider[] explosionCollider = Physics.OverlapSphere(pointOfExplosion, aoeRange);

        foreach (Collider collider in explosionCollider)
        {
            if (collider.gameObject.tag == "Enemy")
            {
                /*
                if (collider.gameObject.tag == "Player")
                {
                    collider.gameObject.GetComponent<PlayerCar>().TakeDamage(damage);
                }*/
                print("BOOM");

                Instantiate(explosionParticles, this.gameObject.transform.position, this.gameObject.transform.rotation);

                collider.gameObject.GetComponent<BaseEnemy>().takeDamge(damage);

                collider.GetComponent<Rigidbody>().AddExplosionForce(explosionKnockback, pointOfExplosion, aoeRange, 3.0f, ForceMode.Impulse);


            }
        }

        Destroy(this.gameObject);
    }
}
