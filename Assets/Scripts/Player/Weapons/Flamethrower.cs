using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flamethrower : Weapon
{
  public List<BaseEnemy> enemy;
  
  public WeaponReference weaponRef;
  public Transform baseTransform;
  public Vector3 direction;
  public float rotationSpeed = 0.000000001f;
  
  bool firing = false;
  ParticleSystem particleSystem;
  private void Start() {
    particleSystem = GetComponent<ParticleSystem>();
  }
  override public void attack() {
    
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
      baseTransform.rotation = Quaternion.LookRotation(direction);

      //baseTransform.rotation = Quaternion.LookRotation(Vector3.RotateTowards(baseTransform.rotation.eulerAngles, direction, rotationSpeed * Time.deltaTime, 0.0f));
    } else {
      firing = false;
    }
    var em = particleSystem.emission;
    if (firing) {
      em.enabled = true;
    } else {
      em.enabled = false;
    }
  }
  /**
  private void OnTriggerEnter(Collider other) {
    if (other.GetComponent<BaseEnemy>() != null) {
      enemy.Add(other.GetComponent<BaseEnemy>());
    }
  }
  */
  private void OnParticleCollision(GameObject other) {
    if (other.GetComponent<BaseEnemy>() != null) {
      enemy.Add(other.GetComponent<BaseEnemy>());
    }
  }
}
