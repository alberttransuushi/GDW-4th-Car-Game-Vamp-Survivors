using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReachTheGoalEvent : Event
{
  [SerializeField] GameObject goal;
  float timer;
  [SerializeField] float timeLimit;
  override public void UpdateFailCondition() {
    timer += Time.deltaTime;
    if (timer > timeLimit) failCondition = true;
  }
  override public void UpdatePassCondition() {

  }
}
