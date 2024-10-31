using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElectricOrb : Weapon {
  public List<Vector3> enemyLocations;
  private void LateUpdate() {
    Attack();
  }
  override public void Attack() {
    GetComponent<LineRenderer>().positionCount = enemyLocations.Count + 1;
    GetComponent<LineRenderer>().SetPosition(0, this.transform.position);
    for (int i = 0; i < enemyLocations.Count; i++) {
      GetComponent<LineRenderer>().SetPosition(i + 1, enemyLocations[i]); ;
    }
    enemyLocations.RemoveRange(0, enemyLocations.Count);
  }
  public override void LevelUp1() {
    weaponStats.damageModifier += 0.1f;
  }
  public override void LevelUp2() {
    weaponStats.critChance += 10;
  }
  public override void LevelUp3() {
    GetComponent<SphereCollider>().radius = GetComponent<SphereCollider>().radius * 1.5f;
  }
  public override void LevelUp4() {
    weaponStats.critChance += 40;
  }
  private void OnTriggerStay(Collider other) {
    if (other.GetComponent<BaseEnemy>() != null) {
      BaseEnemy enemy = other.GetComponent<BaseEnemy>();
      enemyLocations.Add(enemy.gameObject.transform.position);
      int crit = Random.Range(0, 100);
      if (weaponStats.critChance > crit) {
        enemy.takeDamge(damage * weaponStats.damageModifier * (1+ (weaponStats.critDamage / 100)));
      } else {
        enemy.takeDamge(damage * weaponStats.damageModifier);
      }
    }
  }
}
