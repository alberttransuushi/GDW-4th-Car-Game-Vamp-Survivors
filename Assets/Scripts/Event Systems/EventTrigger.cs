using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventTrigger : MonoBehaviour
{
  [SerializeField] float timer;
  //[SerializeField] List<int> eventTimes;
  [SerializeField] float eventTriggerTimer;
  private void Update() {
    timer += Time.deltaTime;
    if (timer > eventTriggerTimer) {//player didnt reach event trigger in time
      //fail the event appearence
      GetComponentInParent<EventManager>().EventTriggerFaded();
      this.gameObject.SetActive(false);
    }
  }
  public void SetTimer(float timer) {
    eventTriggerTimer = timer;
    timer = 0;
  }

  private void OnTriggerEnter(Collider other) {
    if (other.tag == "Player") {
      GetComponentInParent<EventManager>().EventTriggerReached(transform.parent.gameObject);
      this.gameObject.SetActive(false);
    }
  }
}
