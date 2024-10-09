using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flamethrower : Weapon
{
  public List<BaseEnemy> enemy;
  
  public WeaponReference weaponRef;
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
        enemy[i].takeDamge(damage*Time.deltaTime);
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
    
  }
  public override void LevelUp2() {
     
  }
  public override void LevelUp3() {

  }
  public override void LevelUp4() {

  }
  private void OnParticleCollision(GameObject other) {
    if (other.GetComponent<BaseEnemy>() != null) {
      enemy.Add(other.GetComponent<BaseEnemy>());
    }
  }
}
