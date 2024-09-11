using System.Collections;
using System.Collections.Generic;
using UnityEngine;

abstract public class Weapon : MonoBehaviour {
  abstract public void attack();
  [SerializeField] int level = 1;
  [SerializeField] protected int damage;

  // Start is called before the first frame update
  void Start() {

  }

  // Update is called once per frame
  void Update() {
    
  }
}
