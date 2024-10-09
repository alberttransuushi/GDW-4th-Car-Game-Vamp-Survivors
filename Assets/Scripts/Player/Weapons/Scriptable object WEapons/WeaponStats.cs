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
