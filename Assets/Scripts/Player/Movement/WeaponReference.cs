using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponReference : MonoBehaviour {
  public List<BaseEnemy> closeEnemies;
  public float closestEnemyDistance;
  public Transform closestEnemyTransform;
  float closerEnemyTest;
  // Start is called before the first frame update
  void Start() {

  }

  // Update is called once per frame
  void Update() {
    for (int i = 0; i < closeEnemies.Count; i++) {
      if (closeEnemies[i] == null) {
        closeEnemies.RemoveAt(i);
        i--;
      }
    }
    closestEnemyDistance = Mathf.Infinity;
    for (int i = 0; i < closeEnemies.Count; i++) {
      closerEnemyTest = Vector3.Distance(closeEnemies[i].gameObject.transform.position, gameObject.transform.position);
      if (closerEnemyTest < closestEnemyDistance) {
        closestEnemyTransform = closeEnemies[i].transform;
        closestEnemyDistance = closerEnemyTest;
      }
    }

  }
  public Transform GetClosestEnemyPosition() {
    return closestEnemyTransform;
  }
  private void OnTriggerEnter(Collider other) {
    if (other.gameObject.GetComponent<BaseEnemy>() != null) {
      closeEnemies.Add(other.GetComponent<BaseEnemy>());
    }
  }
  private void OnTriggerExit(Collider other) {
    if (other.gameObject.GetComponent<BaseEnemy>() != null) {
      closeEnemies.Remove(other.GetComponent<BaseEnemy>());
    }
  }
}
