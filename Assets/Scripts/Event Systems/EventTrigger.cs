using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventTrigger : MonoBehaviour
{
  [SerializeField] float timer;
  [SerializeField] List<int> eventTimes;
  List<int> eventTimeDuplicate;
  private void Start() {
    eventTimeDuplicate = new List<int>(eventTimes);
  }
  private void Update() {
    timer += Time.deltaTime;
    if (timer >= eventTimeDuplicate[0]) {

    }
  }


  private void OnTriggerEnter(Collider other) {
    if (other.tag == "Player") {
      GetComponentInParent<EventArea>().CreateEvent();
      this.gameObject.SetActive(false);
    }
  }
}
