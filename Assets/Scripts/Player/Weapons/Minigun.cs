using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Minigun : Weapon
{
  public WeaponReference weaponRef;
  public Transform baseTransform;
  public Vector3 direction;
  public float rotationSpeed = 1;
  public float firerate = 0.1f;
  public float timeSinceLastFired;

  [SerializeField] Transform barrelTip;
  [SerializeField] GameObject bullet;

  bool firing = false;
  override public void Attack() {
    if (timeSinceLastFired > firerate) {
      Instantiate(bullet, barrelTip.position, baseTransform.rotation);
      timeSinceLastFired -= firerate;
    }
  }
  public override void LevelUp1() {
    firerate = firerate / 2;
  }
  public override void LevelUp2() {

  }
  public override void LevelUp3() {

  }
  public override void LevelUp4() {

  }
  private void Update() {
    if (weaponRef.GetClosestEnemyPosition() != null) {
      firing = true;
      direction = weaponRef.GetClosestEnemyPosition().position - baseTransform.position;
      direction = direction.normalized;
      baseTransform.rotation = Quaternion.RotateTowards(baseTransform.rotation, Quaternion.LookRotation(direction), 5 * rotationSpeed);

    } else {
      firing = false;
    }
    timeSinceLastFired += Time.deltaTime;
    if (firing) {
      Attack();
    } else {
      
    }
  }
}
