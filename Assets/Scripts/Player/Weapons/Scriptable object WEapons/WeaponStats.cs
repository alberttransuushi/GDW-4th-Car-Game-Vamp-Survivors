using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class WeaponStats : ScriptableObject
{
  public string[] levelUpDescriptions;

  [SerializeField] int level = 0;
  public Sprite sprite;

  public float damageModifier;
  public float fireRate;
  public float projectiles;
  public float size;
  public float critChance;
  public float critDamage = 50;

  public float slowingPercent;
  public float slowingTime;
  public float freezingTime;



  public string GetLevelUpDesc() {
    return levelUpDescriptions[level];
  }
  public void LevelUp() {
    level++;
  }
  public int GetLevel() {
    return level;
  }
}
