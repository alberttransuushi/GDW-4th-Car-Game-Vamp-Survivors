using System.Collections;
using System.Collections.Generic;
using UnityEngine;

abstract public class Event : MonoBehaviour
{
  [SerializeField] string objective;
  [SerializeField] string description;
  public bool failCondition;
  public bool passCondition;
  private void Start() {
    //show player new updated objective
  }
  private void Update() {
    UpdateFailCondition();
    UpdatePassCondition();
    if (failCondition) {

    } else if (passCondition) {

    }
  }
  abstract public void UpdateFailCondition();
  abstract public void UpdatePassCondition();

}
