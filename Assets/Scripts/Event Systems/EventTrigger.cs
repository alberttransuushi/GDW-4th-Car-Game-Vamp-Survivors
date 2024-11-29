using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventTrigger : MonoBehaviour
{

  private void OnTriggerEnter(Collider other) {
    if (other.tag == "Player") {
      GetComponentInParent<EventArea>().CreateEvent();
      this.gameObject.SetActive(false);
    }
  }
}
