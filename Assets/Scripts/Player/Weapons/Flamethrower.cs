using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flamethrower : Weapon
{
  public List<BaseEnemy> enemy;
  public List<float> enemyBurnTime;
  public float burnTime;
  
  public Transform baseTransform;
  public Vector3 direction;
  public float rotationSpeed = 1;
  
  bool firing = false;
  ParticleSystem particleSystem;
  private void Start() {
    particleSystem = GetComponent<ParticleSystem>();
    base.Start();
  }
  override public void Attack() {
    var em = particleSystem.emission;
    if (firing) {
      em.enabled = true;
    } else {
      em.enabled = false;
    }
  }
  private void Update() {
    for (int i = 0; i < enemy.Count; i++) {
      if (enemy[i] != null) {
        //burn enemy
        bool crit = false;
        if (Random.Range(0,100) < weaponStats.critChance) {
          enemy[i].takeDamge(damage * Time.deltaTime * weaponStats.damageModifier * (1 + (weaponStats.critDamage / 100)));
        } else {
          enemy[i].takeDamge(damage * Time.deltaTime * weaponStats.damageModifier);

        }
        enemyBurnTime[i] -= Time.deltaTime;

        if (enemyBurnTime[i] <= 0) {
          //enemy burn over
          enemy.RemoveAt(i);
          enemyBurnTime.RemoveAt(i);
          i--;
        }
      } else {
        //enemy is dead
        enemy.RemoveAt(i);
        enemyBurnTime.RemoveAt(i);
        i--;
      }
    }
    if (weaponRef.GetClosestEnemyPosition() != null) {
      firing = true;
      direction = weaponRef.GetClosestEnemyPosition().position - baseTransform.position;
      direction = direction.normalized;
      baseTransform.rotation = Quaternion.RotateTowards(baseTransform.rotation, Quaternion.LookRotation(direction), 5 * rotationSpeed);

    } else {
      firing = false;
    }
    Attack();
    
  }
  public override void LevelUp1() {
    weaponStats.damageModifier += 0.1f;
  }
  public override void LevelUp2() {
    burnTime += 0.5f;
  }
  public override void LevelUp3() {
    weaponStats.damageModifier += 0.3f;
  }
  public override void LevelUp4() {
    weaponStats.critChance += 20;
  }
  private void OnParticleCollision(GameObject other) {
    if (other.GetComponent<BaseEnemy>() != null) {
      for (int i = 0; i < enemy.Count; i++) {
        if (enemy[i].gameObject == other) {
          enemyBurnTime[i] = burnTime;
          return;
        }
      }
      enemy.Add(other.GetComponent<BaseEnemy>());
      enemyBurnTime.Add(burnTime);
    }
  }
}
