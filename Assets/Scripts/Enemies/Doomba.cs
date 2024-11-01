using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Doomba : MonoBehaviour
{
    [SerializeField] Collider[] childColliders;
    [SerializeField] GameObject dadObject;
    [SerializeField] float spinSpeed;
    [SerializeField] float suckSpeed;
    [SerializeField] float suckDamage;
    public GameObject player;

    public float launchDelay;
    public float launchStrength;
    public float playerCollisionDelay;
    public bool isSucking;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindWithTag("Player");
        childColliders = dadObject.GetComponentsInChildren<Collider>();
    }

    // Update is called once per frame
    void Update()
    {
        transform.RotateAround(transform.position, Vector3.up, spinSpeed * Time.deltaTime);
        if (isSucking)
        {
            player.transform.position = Vector3.Lerp(player.transform.position, dadObject.transform.position, suckSpeed);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            //other.GetComponent<Collider>().enabled = false;
            Physics.IgnoreLayerCollision(6, 8, true);
            isSucking = true;
            StartCoroutine(ShootOutPlayer());
        }
    }

    IEnumerator ShootOutPlayer()
    {
        yield return new WaitForSeconds(launchDelay);

        isSucking = false;
        player.gameObject.GetComponent<PlayerCar>().TakeDamage(suckDamage);

        LaunchPlayer();

        yield return new WaitForSeconds(playerCollisionDelay);
        //player.GetComponent<Collider>().enabled = true;
        Physics.IgnoreLayerCollision(6, 8, false);

    }

    public void LaunchPlayer()
    {

        player.GetComponent<Rigidbody>().AddForce(-dadObject.transform.forward * launchStrength + dadObject.transform.up * launchStrength * 0.5f, ForceMode.VelocityChange);
    }

}
