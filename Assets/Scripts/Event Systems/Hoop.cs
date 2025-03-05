using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hoop : MonoBehaviour {
  [SerializeField] float lowerRange = -1;
  [SerializeField] float upperRange = 1;

  public void AdjustLeftRight(float adjust) {//adjust is a float from 0-1
    float range = upperRange - lowerRange;
    adjust = adjust * range;
    adjust += lowerRange;
    transform.localPosition = new Vector3(adjust, transform.localPosition.y, transform.localPosition.z);
  }
  
}
