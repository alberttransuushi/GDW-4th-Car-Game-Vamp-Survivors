using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExpOrb : MonoBehaviour {
  [SerializeField] int value = 1;
  bool inAir = true;
  GameObject player;
  bool isPickedup;
  float velocity;
  float temp;
  public void SetExpValue(int expValue) {
    value = expValue;
  }
  private void Start() {
    player = GameObject.FindGameObjectWithTag("Player");
    Vector3 tempVector = new Vector3(Random.Range(180, -180), 0, Random.Range(180, -180));
    tempVector = tempVector.normalized;
    GetComponent<Rigidbody>().AddForce(tempVector.x * 300, 700, tempVector.z * 300);
        //Debug.Log("force added");
        isPickedup = true;
  }
  private void Update() {
  /*  if (Vector3.Distance(player.transform.position, transform.position) < PlayerStats.expPickupRange && !isPickedup && Time.timeScale != 0) {
      isPickedup = true;
    }*/
    if (isPickedup && Time.timeScale != 0) {
      velocity += Time.deltaTime;
      if (Vector3.Distance(player.transform.position, transform.position) < velocity && Time.timeScale != 0) {
        player.GetComponent<PlayerExp>().GetExp(value);
        Destroy(this.gameObject);
      }
      transform.position += Vector3.Normalize(player.transform.position - transform.position) * velocity;
    }
  }
  private void OnTriggerEnter(Collider other) {
    if (other.gameObject.tag == "Ground" && inAir) {
      gameObject.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezePosition;

      inAir = false;
    }
  }
}
