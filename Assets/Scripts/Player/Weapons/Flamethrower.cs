using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flamethrower : Weapon
{
  public List<BaseEnemy> enemy;
  override public void attack() {

  }
  private void Update() {
    for (int i = 0; i < enemy.Count; i++) {
      if (enemy[i] != null) {
        enemy[i].takeDamge(damage);
      }
    }
  }
  private void OnTriggerEnter(Collider other) {
    if (other.GetComponent<BaseEnemy>() != null) {
      enemy.Add(other.GetComponent<BaseEnemy>());
    }
  }
}
