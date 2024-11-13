using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Minigun : Weapon
{
  public Transform baseTransform;
  public Vector3 direction;
  public float rotationSpeed = 1;

  public float timeSinceLastFired;

  [SerializeField] Transform barrelTip;
  [SerializeField] GameObject bullet;

    [SerializeField] SoundData soundData;

  bool firing = false;
  override public void Attack() {
    if (timeSinceLastFired > weaponStats.fireRate) {
      GameObject b = Instantiate(bullet, barrelTip.position, baseTransform.rotation);
      b.GetComponent<Bullet>().DamageMultiply(weaponStats.damageModifier);
      timeSinceLastFired -= weaponStats.fireRate;
            SoundManager.Instance.CreateSound().WithSoundData(soundData).WithRandomPitch().WithPosition(barrelTip.position).Play();
    }
  }
  public override void LevelUp1() {
    weaponStats.fireRate = weaponStats.fireRate / 2;
  }
  public override void LevelUp2() {
    weaponStats.damageModifier += 0.1f;
  }
  public override void LevelUp3() {
    weaponStats.projectiles += 1;
  }
  public override void LevelUp4() {
    //NON fuctional as of yet
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
