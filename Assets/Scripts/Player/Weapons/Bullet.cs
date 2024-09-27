using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
  [SerializeField] float damage;
  [SerializeField] float speed;
  private void Update() {
    transform.position += transform.forward * speed;
  }
  private void OnTriggerEnter(Collider other) {
    if (other.CompareTag("enemy")) {
      other.gameObject.GetComponent<BaseEnemy>().takeDamge(damage);
      Destroy(this.gameObject);
    }
  }
}
