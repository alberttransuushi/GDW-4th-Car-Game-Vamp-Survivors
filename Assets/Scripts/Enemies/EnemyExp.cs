using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyExp : MonoBehaviour
{

  [SerializeField] int exp = 1;
  [SerializeField] GameObject expOrb;
  public void SetEnemyExp(int xp) {
    exp = xp;
  }
  public void DropExp() {/*
    for (int i = 0; i < exp; i++) {
      Instantiate(expOrb, gameObject.transform.position, gameObject.transform.rotation);
    }*/
        GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerExp>().GetExp(exp);
        
  }
}
