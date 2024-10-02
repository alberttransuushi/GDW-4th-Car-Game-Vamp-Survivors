using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DinoBoulder : MonoBehaviour
{
    public GameObject playerCar;
    float speed;
    [SerializeField] float trackingDelay;
    [SerializeField] float aoeRange;
    [SerializeField] float damage;
    [SerializeField] float damageFalloff;
    [SerializeField] bool tracking;
    [SerializeField] float turnSpeed;
    [SerializeField] float trackingSpeed;

    Rigidbody rb;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(DelayTracking());
        playerCar = GameObject.Find("player");
        rb = GetComponent<Rigidbody>();

        rb.velocity = new Vector3(0, 200, 0);

    }

    // Update is called once per frame
    void Update()
    {
        if(tracking)
        {
            TrackPlayer();
        }
    }

    void TrackPlayer()
    {
        Vector3 dirToPlayer = playerCar.transform.position - transform.position;

        transform.rotation = Quaternion.LookRotation(Vector3.RotateTowards(transform.forward, new Vector3(dirToPlayer.x, dirToPlayer.y, dirToPlayer.z), turnSpeed * Time.deltaTime, 10.0f));

        rb.velocity = transform.forward * speed;
    }

    IEnumerator DelayTracking()
    {
        print("YEs");
        yield return new WaitForSeconds(trackingDelay);

        tracking = true;

        speed = rb.velocity.magnitude;
    }
}
